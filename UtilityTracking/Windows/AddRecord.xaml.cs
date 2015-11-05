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
using System.Windows.Shapes;
using System.Xml.Linq;
using UtilityTracking.XML;

namespace UtilityTracking.Windows {
    /// <summary>
    /// Interaction logic for AddRecord.xaml
    /// </summary>
    public partial class AddRecord : Window {

        public event EventHandler Submit;

        private double m_Balance;
        private double m_TopUp;
        private DateTime m_DateTime;
        private string m_Selection;

        private bool m_Submitted = false;
        private User_Controls.RecordObject m_RecordObject = new User_Controls.RecordObject();

        public AddRecord()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            InitializeComponent();

            m_RecordTypeBox.Items.Add("Electricity");
            m_RecordTypeBox.Items.Add("Gas");

            m_RecordTypeBox.SelectedIndex = 0;

            m_RecordDateTime.Value = DateTime.Now;
        }

        private void m_RecordTopUp_LostFocus(object sender, RoutedEventArgs e) {
            double value;
            if (!double.TryParse(m_RecordTopUp.Text, out value)) {
                m_RecordTopUp.Text = "0.00";
            } else {
                m_TopUp = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                m_RecordTopUp.Text = m_TopUp.ToString("F");
            }
        }

        private void m_RecordBalance_LostFocus(object sender, RoutedEventArgs e) {
            double value;
            if (!double.TryParse(m_RecordBalance.Text, out value)) {
                m_RecordBalance.Text = "0.00";
            } else {
                m_Balance = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                m_RecordBalance.Text = m_Balance.ToString("F");
            }
        }

        private void m_RecordDateTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            if (m_RecordDateTime != null) {
                m_DateTime = (DateTime)m_RecordDateTime.Value;
            } else {
                m_RecordDateTime.Value = DateTime.Now;
                m_DateTime = DateTime.Now;
            }
        }

        private void m_RecordTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            m_Selection = m_RecordTypeBox.SelectedItem.ToString();
        }

        private void m_SubmitButton_Click(object sender, RoutedEventArgs e) {
            if (m_Selection == "Gas") {
                m_RecordObject = new User_Controls.RecordObject(User_Controls.RecordObject.recordTypes.e_Gas, m_Balance, m_TopUp, m_DateTime, Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + ".xml");
                m_Submitted = true;
                if (Submit != null)
                {
                    Submit(this, EventArgs.Empty);
                }
                XMLHandle.addXElement("Record", "Records", new XElement[0] { }, new XAttribute[4] { new XAttribute("Type", (int)User_Controls.RecordObject.recordTypes.e_Gas), new XAttribute("DateTime", m_DateTime), new XAttribute("Balance", m_Balance), new XAttribute("TopUp", m_TopUp) });
            }
            else if (m_Selection == "Electricity")
            {
                m_RecordObject = new User_Controls.RecordObject(User_Controls.RecordObject.recordTypes.e_Electricity, m_Balance, m_TopUp, m_DateTime, Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + ".xml");
                m_Submitted = true;
                if (Submit != null)
                {
                    Submit(this, EventArgs.Empty);
                }
                XMLHandle.addXElement("Record", "Records", new XElement[0] { }, new XAttribute[4] { new XAttribute("Type", (int)User_Controls.RecordObject.recordTypes.e_Electricity), new XAttribute("DateTime", m_DateTime), new XAttribute("Balance", m_Balance), new XAttribute("TopUp", m_TopUp) });
            }
        }
        public User_Controls.RecordObject fetchRecord()
        {
            
            if (m_Submitted)
            {
                return m_RecordObject;
            }
            else
            {
                throw new Exception("Record Not Available");
            }
        }
    }
}
