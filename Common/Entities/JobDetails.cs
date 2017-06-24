using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    /// <summary>
    /// Represents an encoding job
    /// </summary>
    [Serializable]
    [DataContract]
    public class JobDetails
    {
        #region Private Members

        private long m_Id;

        private Uri m_Uri;

        private EncodingJobStatus m_Status;

        private List<Resolution> m_Resolutions;

        #endregion

        #region Public Members

        [DataMember]
        public long Id
        {
            get
            {
                return m_Id;
            }

            set
            {
                m_Id = value;
            }
        }

        [DataMember]
        public Uri Uri
        {
            get
            {
                return m_Uri;
            }

            set
            {
                m_Uri = value;
            }
        }

        /// <summary>
        /// TODO: Change to enum
        /// </summary>
        [DataMember]
        public EncodingJobStatus Status
        {
            get
            {
                return m_Status;
            }

            set
            {
                m_Status = value;
            }
        }

        [DataMember]
        public List<Resolution> Resolutions
        {
            get
            {
                return m_Resolutions;
            }

            set
            {
                m_Resolutions = value;
            }
        }

        #endregion

        #region Ctor

        public JobDetails()
        {
            m_Resolutions = new List<Resolution>();
        }

        public JobDetails(Uri uri)
        {
            m_Resolutions = new List<Resolution>();

            Uri = uri;
        }

        public JobDetails(Uri uri, List<Resolution> lst)
        {
            m_Resolutions = (lst != null) ? m_Resolutions = lst : new List<Resolution>();

            Uri = uri;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            string res = string.Join(", ", m_Resolutions.Select(x => x.ToString()));

            return String.Format("Id: {0}, Uri: {1}, Res: {2}", Id , m_Uri, res);
        }

        #endregion
    }
}
