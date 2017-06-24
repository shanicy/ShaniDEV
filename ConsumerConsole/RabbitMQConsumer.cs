using Common.Entities;
using Common.Interfaces;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerConsole
{
    /// <summary>
    /// Represents a RabbitMQConsumer msg consumer
    /// </summary>
    public class RabbitMQConsumer : ICanConsume
    {
        public event EventHandler<IdWrapper> HandleData;

        #region Ctor

        public RabbitMQConsumer()
        {
        }

        #endregion

        #region Methods

        public void Consume()
        {
            Console.WriteLine("Listening for messages. Hit <return> to quit.");

            // TODO: Set RabbitMQ connection string in the config
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Subscribe<IdWrapper>("shani", HandleMessage);

                Console.ReadLine();
            }
        }

        public void HandleMessage(IdWrapper idw)
        {
            Console.WriteLine("Got message: {0}", idw.ToString());

            HandleData?.Invoke(this, idw);
        }

        #endregion
    }
}
