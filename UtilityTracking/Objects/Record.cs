using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UtilityTracking.Objects
{
    public class Record : INotifyPropertyChanged
    {
        private DateTime m_Date;
        private double m_Balance;
        private double m_TopUp;
        private string m_FileName;
        private RecordType m_Type;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Remove;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RecordType Type
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                NotifyPropertyChanged("Type");
            }
        }

        public string FileName
        {
            get { return m_FileName; }
            set 
            {
                m_FileName = value;
                NotifyPropertyChanged("FileName");
            }
        }
        public double Balance
        {
            get { return m_Balance; }
            set
            {
                m_Balance = value;
                NotifyPropertyChanged("Balance");
            }
        }
        public double TopUp
        {
            get { return m_TopUp; }
            set
            {
                m_TopUp = value;
                NotifyPropertyChanged("TopUp");
            }
        }
        public DateTime Date
        {
            get { return m_Date; }
            set
            {
                m_Date = value;
                NotifyPropertyChanged("Date");
            }
        }
        
        public Record(RecordType recordType, double balance, double topUp, DateTime recordTime, string filename) {

            Date = recordTime;
            Balance = balance;
            TopUp = topUp;
            Type = recordType;

            FileName = filename;                       
        }
    }
}
