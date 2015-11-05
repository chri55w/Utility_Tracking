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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UtilityTracking.User_Controls
{
    /// <summary>
    /// Interaction logic for CalculationPanel.xaml
    /// </summary>
    public partial class CalculationPanel : UserControl, INotifyPropertyChanged
    {
        public int m_records = 0;
        private double m_totalCost = 0.0;
        private double m_payments = 0.0;
        private double m_dailyCost = 0.0;
        private double m_monthlyCost = 0.0;
        private double m_yearlyCost = 0.0;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public CalculationPanel()
        {
            InitializeComponent();

            DataContext = this;
        }

        public int RecordsCount
        {
            get { return m_records; }
            set
            {
                m_records = value;
                NotifyPropertyChanged();
            }
        }
        public double TotalCost
        {
            get { return m_totalCost; }
            set
            {
                m_totalCost = value;
                NotifyPropertyChanged();
            }
        }
        public double TotalPayments
        {
            get { return m_payments; }
            set
            {
                m_payments = value;
                NotifyPropertyChanged();
            }
        }
        public double DailyCost
        {
            get { return m_dailyCost; }
            set
            {
                m_dailyCost = value;
                NotifyPropertyChanged();
            }
        }
        public double MonthlyCost
        {
            get { return m_monthlyCost; }
            set
            {
                m_monthlyCost = value;
                NotifyPropertyChanged();
            }
        }
        public double YearlyCost
        {
            get { return m_yearlyCost; }
            set
            {
                m_yearlyCost = value;
                NotifyPropertyChanged();
            }
        }

        public void ClearData()
        {
            RecordsCount = 0;
            TotalCost = 0.0;
            TotalPayments = 0.0;
            DailyCost = 0.0;
            MonthlyCost = 0.0;
            YearlyCost = 0.0;

        }

        public void Calculate(List<RecordObject> records)
        {
            RecordsCount = records.Count();
            
            IEnumerable<RecordObject> sortedList = SortList(records);

            TotalPayments = SumOfTopUps(sortedList.Where(r => r.TopUp > 0));

            TotalCost = ((sortedList.First().Balance - sortedList.Last().Balance) + TotalPayments) - sortedList.First().TopUp;

            DailyCost = CalculateDailyAvg(sortedList);

            MonthlyCost = m_dailyCost * Properties.Settings.Default.DaysPerMonth;

            YearlyCost = m_dailyCost * Properties.Settings.Default.DaysPerYear;

            
        }

        private IEnumerable<RecordObject> SortList(List<RecordObject> Records)
        {
            return Records.OrderBy(r => r.RecordDateTime);
        }

        private double SumOfTopUps(IEnumerable<RecordObject> RecordList)
        {
            double total = 0.0;
            foreach (RecordObject r in RecordList)
            {
                total += r.TopUp;
            }
            return total;
        }

        private double CalculateDailyAvg(IEnumerable<RecordObject> records)
        {
            double totalWeight = 0.0;
            double totalCost = 0.0;

            for (int i = 0; i < records.Count()-1; i++)
            {
                double startBalance = records.ElementAt(i).Balance;
                double endBalance = records.ElementAt(i + 1).Balance;
                double endTopUp = records.ElementAt(i + 1).TopUp;

                DateTime startDateTime = records.ElementAt(i).RecordDateTime;
                DateTime endDateTime = records.ElementAt(i + 1).RecordDateTime;

                TimeSpan timeDiff = endDateTime - startDateTime;

                totalWeight += (timeDiff.TotalSeconds / 60) / 1440;

                totalCost += startBalance - endBalance + endTopUp;
            }
            
            return totalCost / totalWeight;
        }
    }
}

