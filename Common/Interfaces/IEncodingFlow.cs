using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IEncodingFlow
    {
        JobDetails Download(JobDetails jd);

        JobDetails Encode(JobDetails jd);

        void UploadToFTP(JobDetails jd);
    }
}
