using Common.Entities;
using Common.Interfaces;
using DAL.Properties;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// Represents the Data Access Layer
    /// </summary>
    public class DAL
    {
        #region Members

        private IEncodingDB m_DB; 

        #endregion

        #region Ctor

        public DAL()
        {
            // TODO: Dependency injection here. Set SqlServerDB to be choosen as plugin during runtime
            m_DB = new SqlServerDB();
        }

        #endregion

        #region Public Functions

        public bool Initialize()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Update status in the DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public void UpdateStatus(long id, string status)
        {
            m_DB.UpdateStatus(id, status);
        }

        /// <summary>
        /// Insert a new job details to the DB
        /// </summary>
        /// <param name="jd"></param>
        /// <returns></returns>
        public long InsertJob(JobDetails jd)
        {
            return m_DB.InsertJob(jd);
        }

        /// <summary>
        /// Get an existing JobDetails object from the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JobDetails GetJobDetails(long id)
        {
            return m_DB.GetJobDetails(id);
        }
    }
}
