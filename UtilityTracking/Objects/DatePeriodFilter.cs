using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTracking.Objects
{
    class DatePeriodFilter
    {
        string m_Name;
        DateTime m_StartDate;
        DateTime m_EndDate;

        public string Name
        {
            get { return m_Name; }
        }

        public DatePeriodFilter(string name, DateTime start, DateTime end)
        {
            m_Name = name;
            m_StartDate = start;
            m_EndDate = end;
        }

        public bool Filter(object obj)
        {
            if (obj is DateTime)
            {
                if ((DateTime)obj >= m_StartDate && (DateTime)obj < m_EndDate)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return m_Name;
        }
    }
}
