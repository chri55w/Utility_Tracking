using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtilityTracking.User_Controls {
    /// <summary>
    /// Interaction logic for RecordObject.xaml
    /// </summary>
    
    public partial class RecordObject : UserControl {

        public event EventHandler Remove;
        
        public enum recordTypes { 
            e_Gas = 0, 
            e_Electricity = 1 };

       
        private DateTime m_DateTime;
        private double m_Balance;
        private double m_TopUp;
        private recordTypes m_Type;
        private string m_FileName;

        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        public double Balance
        {
            get { return m_Balance; }
        }
        public string BalanceString
        {
            get { return Properties.Settings.Default.CurrencySymbol + m_Balance.ToString(); }
        }
        public double TopUp
        {
            get { return m_TopUp; }
        }
        public string TopUpString
        {
            get { return Properties.Settings.Default.CurrencySymbol + m_TopUp.ToString(); }
        }
        public DateTime RecordDateTime
        {
            get { return m_DateTime; }
        }
        public string FormattedRecordDateTime
        {
            get
            {
                return DateFormat(m_DateTime);
            }
        }
        public recordTypes Type
        {
            get { return m_Type; }
        }

        public static string AsText(recordTypes type)
        {
            string returnValue = "null";
            switch (type)
            {
                case RecordObject.recordTypes.e_Electricity:
                    returnValue = "Electricity";
                    break;
                case RecordObject.recordTypes.e_Gas:
                    returnValue = "Gas";
                    break;
            }
            return returnValue;
        }

        public string ToCurrency(double dbl) {
            return (Properties.Settings.Default.CurrencySymbol + dbl.ToString());
        }
        public string DateFormat(DateTime dt)
        {
            return dt.ToString(Properties.Settings.Default.RecordDateFormat);
        }

        public RecordObject(recordTypes recordType, double balance, double topUp, DateTime recordTime, string filename) {

            m_DateTime = recordTime;
            m_Balance = balance;
            m_TopUp = topUp;
            m_Type = recordType;

            m_FileName = filename;

            this.DataContext = this;

            InitializeComponent();
                       
        }

        public RecordObject() {}


        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Remove != null)
            {
                Remove(this, EventArgs.Empty);
            }
        }

    }
}
