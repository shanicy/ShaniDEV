using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ICanConsume
    {
        event EventHandler<IdWrapper> HandleData;

        void Consume();
    }
}
