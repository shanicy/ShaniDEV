using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using DAL.Properties;

namespace DAL
{
    /// <summary>
    /// Represents Sql Server DB Access layer
    /// </summary>
    public class SqlServerDB : IEncodingDB
    {
        #region Ctor

        public SqlServerDB()
        {
            // Get coonection string from config. Decrypt if necessary.
            // Validate connection
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get details of a job
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JobDetails GetJobDetails(long id)
        {
            JobDetails jd = null;

            try
            {
                using (var md = new EncodingJobsContext(Settings.Default.ConString))
                {
                    jd = md.EncodingJobs.FirstOrDefault(x => x.Id == id);

                    if (jd == null)
                    {
                        // TODO: Handle not existing data
                    }
                }
            }
            catch (Exception ex)
            {
                // Add extra information to the exception. Throw again
                throw ex;
            }

            return jd;
        }

        /// <summary>
        /// Insert new job to the DB
        /// </summary>
        /// <param name="jd"></param>
        /// <returns></returns>
        public long InsertJob(JobDetails jd)
        {
            try
            {
                using (var md = new EncodingJobsContext(Settings.Default.ConString))
                {
                    md.EncodingJobs.InsertOnSubmit(jd);
                    md.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                // Add extra information to the exception. Throw again
                throw ex;
            }

            return jd.Id;
        }

        /// <summary>
        /// Update job status in the DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public void UpdateStatus(long id, EncodingJobStatus status)
        {
            try
            {
                using (var md = new EncodingJobsContext(Settings.Default.ConString))
                {
                    var job = md.EncodingJobs.FirstOrDefault(x => x.Id == id);

                    if (job != null)
                    {
                        job.Status = status;

                        md.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // Add extra information to the exception. Throw again
                throw ex;
            }
        } 

        #endregion
    }
}
