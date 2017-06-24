using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common.Entities;
using Publisher;
using Common.Contracts;
using Common.Interfaces;

namespace EncodingWCFService
{
    public class EncodingService : IEncodingService
    {
        #region Members

        private IJobQueueProducer p;

        private DAL.DAL m_DAL;

        #endregion

        #region Ctor

        public EncodingService()
        {
            try
            {
                // TODO: Dependency injection here. Set RabbitMQPublisher to be choosen as plugin during runtime
                p = new RabbitMQPublisher();

                p.Initialize();

                m_DAL = new DAL.DAL();
            }
            catch (Exception)
            {
                // Handle exception here and log
            }
        }

        #endregion

        #region Methods

        public long ProcessJob(JobDetails j)
        {
            long id = -1;
            try
            {
                id = m_DAL.InsertJob(j);

                // Add to DB. Let the queue to publish only ID
                p.Publish(j);
            }
            catch (Exception ex)
            {
                // Handle exceptions here and log
            }

            return id;
        } 

        #endregion
    }
}
