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

        private static EncodingWorkflow s_Workflow;

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

        private static void HandleMessage(IdWrapper id)
        {
            Console.WriteLine("Got message: {0}", id.ToString());

            JobDetails j = GetJobDetailsFromDB(id);

            s_Workflow.PostToFlow(j);
        }

        private static JobDetails GetJobDetailsFromDB(IdWrapper msg)
        {
            return s_DAL.GetJobDetails(msg.Id);
        }

        #endregion
    }
}
