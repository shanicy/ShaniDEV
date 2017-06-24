using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IEncodingService
    {
        [OperationContract]
        long ProcessJob(JobDetails j);
    }
}
