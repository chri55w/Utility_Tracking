using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Xml.Linq;
using System.Windows.Input;

using UtilityTracking.Objects;
using UtilityTracking.XML;
using UtilityTracking.StaticObjects;

namespace UtilityTracking.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region Variable Declarations

        private ObservableCollection<Record> m_GasRecords = new ObservableCollection<Record>();
        private ObservableCollection<Record> m_ElectricRecords = new ObservableCollection<Record>();

        private List<DatePeriodFilter> m_DateFilters = new List<DatePeriodFilter>();
        private DatePeriodFilter m_SelectedDatePeriodFilter = null;

        private Dictionary<string, RecordType> m_RecordTypes = new Dictionary<string, RecordType>();

        private bool m_AddNewRecordFlyoutIsOpen = false;

        private KeyValuePair<string, RecordType> m_NewRecordSelectedType;
        private DateTime m_NewRecordDateTime;
        private string m_NewRecordBalance;
        private string m_NewRecordTopUp;

        #endregion

        #region Event Declarations

        public event PropertyChangedEventHandler PropertyChanged;
                
        #endregion

        #region Event Firing Methods

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Command Declarations

        public ICommand SelectedDatePeriodFilterChangedCommand { get; set; }

        public ICommand AddNewRecordCommand { get; set; }

        public ICommand SubmitAddNewRecordCommand { get; set; }

        public ICommand NewRecordBalanceLostFocusCommand { get; set; }
        public ICommand NewRecordTopUpLostFocusCommand { get; set; }

        #endregion

        #region Accessor Properties

        public ObservableCollection<DatePeriodFilter> DatePeriodFilters
        {
            get { return new ObservableCollection<DatePeriodFilter>(m_DateFilters); }
        }

        public bool AddNewRecordFlyoutIsOpen
        {
            get { return m_AddNewRecordFlyoutIsOpen; }
            set
            {
                m_AddNewRecordFlyoutIsOpen = value;
                NotifyPropertyChanged("AddNewRecordFlyoutIsOpen");
            }
        }

        public DatePeriodFilter SelectedDatePeriodFilter
        {
            get { return m_SelectedDatePeriodFilter; }
            set
            {
                m_SelectedDatePeriodFilter = value;
                NotifyPropertyChanged("SelectedDatePeriodFilter");
            }
        }
        public ObservableCollection<Record> FilteredGasRecords
        {
            get { return new ObservableCollection<Record>(m_GasRecords.Where(gr => SelectedDatePeriodFilter.Filter(gr.Date)).OrderByDescending(gr => gr.Date)); }
        }
        public ObservableCollection<Record> FilteredElectricRecords
        {
            get { return new ObservableCollection<Record>(m_ElectricRecords.Where(er => SelectedDatePeriodFilter.Filter(er.Date)).OrderByDescending(er => er.Date)); }
        }

        public Dictionary<string, RecordType> RecordTypes
        {
            get { return m_RecordTypes; }
            set
            {
                m_RecordTypes = value;
                NotifyPropertyChanged("RecordTypes");
            }
        }

        public KeyValuePair<string, RecordType> NewRecordSelectedType
        {
            get { return m_NewRecordSelectedType; }
            set
            {
                m_NewRecordSelectedType = value;
                NotifyPropertyChanged("NewRecordSelectedType");
            }
        }

        public DateTime NewRecordDateTime
        {
            get { return m_NewRecordDateTime; }
            set
            {
                m_NewRecordDateTime = value;
                NotifyPropertyChanged("NewRecordDateTime");
            }
        }

        public string NewRecordBalance
        {
            get { return m_NewRecordBalance; }
            set
            {
                m_NewRecordBalance = value;
                NotifyPropertyChanged("NewRecordBalance");
            }
        }
        public string NewRecordTopUp
        {
            get { return m_NewRecordTopUp; }
            set
            {
                m_NewRecordTopUp = value;
                NotifyPropertyChanged("NewRecordTopUp");
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            UpdateDatePeriodFilters();

            XMLHandle.loadSchema(Properties.Settings.Default.SchemaFileLoc);
            LoadXMLFileData();
            try
            {
                XMLHandle.loadXMLDoc(Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + ".xml");
            }
            catch (Exception e_File)
            {
                Console.WriteLine("Error Locating File, Creating New Record File. " + e_File.Message);
                XMLHandle.CreateXMLDoc(Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + ".xml");
                XMLHandle.loadXMLDoc(Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + ".xml");
            }
            XMLHandle.validateXML();

            XMLHandle.FileChange += XMLHandle_FileChange;

            AddNewRecordCommand = new RelayCommand(ToggleAddNewRecordFlyout);

            SelectedDatePeriodFilterChangedCommand = new RelayCommand(SelectedDatePeriodFilterChanged);

            NewRecordBalanceLostFocusCommand = new RelayCommand(NewRecordBalanceLostFocus);
            NewRecordTopUpLostFocusCommand = new RelayCommand(NewRecordTopUpLostFocus);

            SubmitAddNewRecordCommand = new RelayCommand(AddNewRecord);

            RecordTypes.Add("Electricity", RecordType.e_Electricity);
            RecordTypes.Add("Gas", RecordType.e_Gas);
        }

        #endregion

        #region Functions
        
        private string[] LoadRecordFileNames()
        {
            string[] filenames = Directory.GetFiles(Properties.Settings.Default.RecordFileLocation);

            return filenames;
        }

        private void LoadXMLFileData()
        {
            foreach (string FName in LoadRecordFileNames())
            {
                try
                {
                    XMLHandle.loadXMLDoc(FName);
                    XMLHandle.validateXML();
                    foreach (XElement xe in XMLHandle.getElementsOfType("Record"))
                    {
                        try
                        {
                            Record newRecord = new Record((RecordType)((int)xe.Attribute("Type")), (double)xe.Attribute("Balance"), (double)xe.Attribute("TopUp"), (DateTime)xe.Attribute("DateTime"), FName);
                            if (newRecord.Type == RecordType.e_Electricity)
                            {
                                m_ElectricRecords.Add(newRecord);
                                newRecord.Remove += record_Remove;
                                NotifyPropertyChanged("FilteredElectricRecords");
                            }
                            else if (newRecord.Type == RecordType.e_Gas)
                            {
                                m_GasRecords.Add(newRecord);
                                newRecord.Remove += record_Remove;
                                NotifyPropertyChanged("FilteredGasRecords");
                            }
                        }
                        catch (Exception e_Record)
                        {
                            Console.WriteLine("Error Loading Record in File '" + FName + "' (" + e_Record.Message + ")");
                        }
                    }
                }
                catch (Exception e_File)
                {
                    Console.WriteLine("Error Loading File '" + FName + ". (" + e_File.Message + ")");
                }
            }
        }
        
        public void UpdateDatePeriodFilters()
        {
            DatePeriodFilter today = new DatePeriodFilter("Today", DateTime.Today, DateTime.Today.AddDays(1));

            m_DateFilters.Add(today);
            m_DateFilters.Add(new DatePeriodFilter("This Week", DateTime.Today.StartOfWeek(), DateTime.Today.EndOfWeek()));
            m_DateFilters.Add(new DatePeriodFilter("This Month", DateTime.Today.StartOfMonth(), DateTime.Today.EndOfMonth()));
            m_DateFilters.Add(new DatePeriodFilter("Last Week", DateTime.Today.AddDays(-7).StartOfWeek(), DateTime.Today.AddDays(-7).EndOfWeek()));
            m_DateFilters.Add(new DatePeriodFilter("Last Month", DateTime.Today.AddMonths(-1).StartOfMonth(), DateTime.Today.AddMonths(-1).EndOfMonth()));
            m_DateFilters.Add(new DatePeriodFilter("All", DateTime.MinValue, DateTime.MaxValue));

            if (m_DateFilters.Where(f => f.Name == Properties.Settings.Default.SelectedDatePeriodFilterName).Count() > 0)
            {
                SelectedDatePeriodFilter = m_DateFilters.Where(f => f.Name == Properties.Settings.Default.SelectedDatePeriodFilterName).First();
            }
            else
            {
                SelectedDatePeriodFilter = m_DateFilters.Last();
            }
            NotifyPropertyChanged("FilteredElectricRecords");
            NotifyPropertyChanged("FilteredGasRecords");
            NotifyPropertyChanged("DateFilterPeriods");
        }

        #endregion

        #region Event Handlers

        void XMLHandle_FileChange(FileChangedEventArgs e)
        {
            foreach (Record record in m_GasRecords)
            {
                if (record.FileName == e.oldFileName)
                {
                    record.FileName = e.newFileName;
                }
            }
            foreach (Record record in m_ElectricRecords)
            {
                if (record.FileName == e.oldFileName)
                {
                    record.FileName = e.newFileName;
                }
            }
        }

        void record_Remove(object sender, EventArgs e)
        {
            Record toRemove = sender as Record;

            if (toRemove != null && toRemove.Type == RecordType.e_Electricity)
            {
                m_ElectricRecords.Remove(toRemove);
                XElement recordXE = new XElement("Record");
                recordXE.Add(new XAttribute("Type", (int)toRemove.Type));
                recordXE.Add(new XAttribute("DateTime", toRemove.Date));
                recordXE.Add(new XAttribute("Balance", toRemove.Balance.ToString()));
                recordXE.Add(new XAttribute("TopUp", toRemove.TopUp.ToString()));
                XMLHandle.deleteFromFile(toRemove.FileName, recordXE);
                NotifyPropertyChanged("FilteredElectricRecords");
            }
            else if (toRemove != null)
            {
                m_GasRecords.Remove(toRemove);
                XElement recordXE = new XElement("Record");
                recordXE.Add(new XAttribute("Type", (int)toRemove.Type));
                recordXE.Add(new XAttribute("DateTime", toRemove.Date));
                recordXE.Add(new XAttribute("Balance", toRemove.Balance.ToString()));
                recordXE.Add(new XAttribute("TopUp", toRemove.TopUp.ToString()));
                XMLHandle.deleteFromFile(toRemove.FileName, recordXE);
                NotifyPropertyChanged("FilteredGasRecords");
            }
        }

        #endregion

        #region Command Functions

        private void AddNewRecord(object obj)
        {
            XMLHandle.addXElement("Record", "Records", new XElement[0] { }, new XAttribute[4] { new XAttribute("Type", (int)NewRecordSelectedType.Value), new XAttribute("DateTime", NewRecordDateTime), new XAttribute("Balance", NewRecordBalance.Remove(0, 1)), new XAttribute("TopUp", NewRecordTopUp.Remove(0, 1)) });

            if (NewRecordSelectedType.Value == RecordType.e_Gas)
                m_GasRecords.Add(new Record(NewRecordSelectedType.Value, Double.Parse(NewRecordBalance.Remove(0, 1)), Double.Parse(NewRecordTopUp.Remove(0, 1)), NewRecordDateTime, (Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + ".xml")));
            else
                m_ElectricRecords.Add(new Record(NewRecordSelectedType.Value, Double.Parse(NewRecordBalance.Remove(0, 1)), Double.Parse(NewRecordTopUp.Remove(0, 1)), NewRecordDateTime, (Properties.Settings.Default.RecordFileLocation + "\\" + Properties.Settings.Default.XMLRecordName + ".xml")));

            NotifyPropertyChanged("FilteredElectricRecords");
            NotifyPropertyChanged("FilteredGasRecords");
        }

        private void ToggleAddNewRecordFlyout(object obj)
        {
            AddNewRecordFlyoutIsOpen = !AddNewRecordFlyoutIsOpen;

            NewRecordSelectedType = RecordTypes.Where(rt => rt.Key == "Gas").First();

            NewRecordDateTime = DateTime.Now;

            NewRecordBalance = Properties.Settings.Default.CurrencySymbol + "0.00";
            NewRecordTopUp = Properties.Settings.Default.CurrencySymbol + "0.00";
        }

        private void NewRecordBalanceLostFocus(object obj)
        {
            double value;
            if (!double.TryParse(NewRecordBalance, out value))
            {
                NewRecordBalance = Properties.Settings.Default.CurrencySymbol + "0.00";
            }
            else
            {
                NewRecordBalance = Properties.Settings.Default.CurrencySymbol + Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString("F");
            }
        }

        private void NewRecordTopUpLostFocus(object obj)
        {
            double value;
            if (!double.TryParse(NewRecordTopUp, out value))
            {
                NewRecordTopUp = Properties.Settings.Default.CurrencySymbol + "0.00";
            }
            else
            {
                NewRecordTopUp = Properties.Settings.Default.CurrencySymbol + Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString("F");
            }
        }

        private void SelectedDatePeriodFilterChanged(object obj)
        {
            NotifyPropertyChanged("FilteredElectricRecords");
            NotifyPropertyChanged("FilteredGasRecords");
        }

        #endregion

    }
}
