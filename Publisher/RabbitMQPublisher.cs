using Common.Entities;
using Common.Interfaces;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    /// <summary>
    /// Publisher using RabbitMQ
    /// </summary>
    public class RabbitMQPublisher : ICanPublish, IDisposable
    {
        public IBus m_Bus;

        public RabbitMQPublisher()
        {

        }

        public void Initialize()
        {
            // TODO: Add connection string to the config
            m_Bus = RabbitHutch.CreateBus("host=localhost");
        }

        public void Dispose()
        {
            m_Bus.Dispose();
        }

        public void Publish(JobDetails j)
        {
            try
            {
                // Add to the queue
                m_Bus.Publish<IdWrapper>(new IdWrapper(j.Id));
            }
            catch (Exception ex)
            {
                // Handle, write to log, write to error report
                throw;
            }
        }
    }
}
