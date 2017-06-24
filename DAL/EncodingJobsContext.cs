using Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// This is a fake DataContext object. Will be created "Schema first" using visual studio.
    /// </summary>
    public partial class EncodingJobsContext : DataContext
    {
        #region Public Members

        public Table<JobDetails> EncodingJobs;

        #endregion

        #region Ctor

        public EncodingJobsContext(string connection) : base(connection)
        {
        } 

        #endregion
    }
}
