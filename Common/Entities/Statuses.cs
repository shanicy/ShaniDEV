using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    /// <summary>
    /// Represents a job status
    /// </summary>
    [Serializable]
    public enum EncodingJobStatus
    {
        Pending = 1,
        Downloading = 2,
        Encoding = 3,
        Uploading = 4,
        Completed = 5,
        Faulted = 6
    }
}
