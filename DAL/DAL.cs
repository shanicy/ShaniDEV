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

        #region Public Functions

        public bool Initialize()
        {
            return true;
        }

        #endregion

        public void UpdateStatus(long id, string status)
        {
            m_DB.UpdateStatus(id, status);
        }

        public long InsertJob(JobDetails jd)
        {
            return m_DB.InsertJob(jd);
        }

        public JobDetails GetJobDetails(long id)
        {
            return m_DB.GetJobDetails(id);
        }

        #endregion

    }
}
