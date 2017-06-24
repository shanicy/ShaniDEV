using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IEncodingDB
    {
        void UpdateStatus(long id, string status);

        long InsertJob(JobDetails jd);

        JobDetails GetJobDetails(long id);

    }
}
