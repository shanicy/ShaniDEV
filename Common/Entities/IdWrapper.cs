using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class IdWrapper
    {
        private long m_Id;

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

        public IdWrapper(long id)
        {
            m_Id = id;
        }

        public override string ToString()
        {
            return m_Id.ToString();
        }
    }
}
