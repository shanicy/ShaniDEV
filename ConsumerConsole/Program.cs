using Common.Entities;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerConsole
{
    class Program
    {
        #region Members

        private static IEncodingFlow s_Workflow;

        private static IJobQueueConsumer s_Consumer;

        private static DAL.DAL s_DAL;

        #endregion

        #region Methods

        static void Main(string[] args)
        {           
            s_Workflow = new EncodingWorkflow();
            s_Workflow.InitFlow();

            s_DAL = new DAL.DAL();

            // TODO: Dependecy Injection - Load plugin using Unity (Who implements IJobQueueConsumer)
            s_Consumer = new RabbitMQConsumer();
            s_Consumer.HandleData += Consumer_HandleData;

            s_Consumer.Consume();
        }

        private static void Consumer_HandleData(object sender, IdWrapper e)
        {
            HandleMessage(e);
        }

        private static void HandleMessage(IdWrapper wid)
        {
            Console.WriteLine("Got message: {0}", wid.ToString());
            try
            {
                JobDetails j = s_DAL.GetJobDetails(wid.Id);

                s_Workflow.PostToFlow(j);
            }
            catch (Exception ex)
            {
                // Handle Flow exceptions: State the job as faulted, write to log...
                // Handle DB exceptions
            }
        }

        #endregion
    }
}
