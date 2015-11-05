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
using System.IO;
using System.Xml.Linq;
using UtilityTracking.XML;
using System.Collections.ObjectModel;
using System.Globalization;

using MahApps.Metro.Controls;

namespace UtilityTracking
{
    public struct TimePeriod
    {
        public DateTime Start;
        public DateTime End;

        public TimePeriod(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private List<User_Controls.RecordObject> Gas_Records = new List<User_Controls.RecordObject>();
        private List<User_Controls.RecordObject> Electric_Records = new List<User_Controls.RecordObject>();

        private TimePeriod RecordViewPeriod = new TimePeriod(DateTime.Now, DateTime.Now);

        

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            InitializeComponent();

            TimePeriodSelector.SelectedIndex = 6;

            this.Closed += MainWindow_Closed;

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

            CalculateElecData();
            CalculateGasData();
        }

        void XMLHandle_FileChange(FileChangedEventArgs e)
        {
            foreach (User_Controls.RecordObject record in Gas_Records)
            {
                if (record.FileName == e.oldFileName)
                {
                    record.FileName = e.newFileName;
                }
            }
            foreach (User_Controls.RecordObject record in Electric_Records)
            {
                if (record.FileName == e.oldFileName)
                {
                    record.FileName = e.newFileName;
                }
            }

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
                            User_Controls.RecordObject newRecord = new User_Controls.RecordObject((User_Controls.RecordObject.recordTypes)((int)xe.Attribute("Type")), (double)xe.Attribute("Balance"), (double)xe.Attribute("TopUp"), (DateTime)xe.Attribute("DateTime"), FName);
                            if (newRecord.Type == User_Controls.RecordObject.recordTypes.e_Electricity)
                            {
                                Electric_Records.Add(newRecord);
                                newRecord.Remove += record_Remove;
                                updateElectricRecordsList();
                            }
                            else if (newRecord.Type == User_Controls.RecordObject.recordTypes.e_Gas)
                            {
                                Gas_Records.Add(newRecord);
                                newRecord.Remove += record_Remove;
                                updateGasRecordsList();
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

        void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AppAddItem_Click(object sender, RoutedEventArgs e)
        {

            if (!App.Current.Windows.OfType<Windows.AddRecord>().Any())
            {
                Windows.AddRecord AddRecordWindow = new Windows.AddRecord();
                AddRecordWindow.Owner = this;
                AddRecordWindow.Submit += AddRecordWindow_Submit;

                AddRecordWindow.Show();
            }
            else
            {
                App.Current.Windows.OfType<Windows.AddRecord>().First().Activate();
            }
        }

        void AddRecordWindow_Submit(object sender, EventArgs e)
        {
            User_Controls.RecordObject record = (sender as Windows.AddRecord).fetchRecord();
            if (User_Controls.RecordObject.AsText(record.Type) == "Gas")
            {
                Gas_Records.Add(record);
                record.Remove += record_Remove;
                updateGasRecordsList();
            }
            else if (User_Controls.RecordObject.AsText(record.Type) == "Electricity")
            {
                Electric_Records.Add(record);
                record.Remove += record_Remove;
                updateElectricRecordsList();
            }
            (sender as Window).Close();
        }

        void record_Remove(object sender, EventArgs e)
        {
            if ((sender as User_Controls.RecordObject).Type == User_Controls.RecordObject.recordTypes.e_Electricity)
            {
                Electric_Records.Remove(sender as User_Controls.RecordObject);
                User_Controls.RecordObject record = (sender as User_Controls.RecordObject);
                XElement recordXE = new XElement("Record");
                recordXE.Add(new XAttribute("Type", (int)record.Type));
                recordXE.Add(new XAttribute("DateTime", record.RecordDateTime));
                recordXE.Add(new XAttribute("Balance", record.Balance.ToString()));
                recordXE.Add(new XAttribute("TopUp", record.TopUp.ToString()));
                XMLHandle.deleteFromFile(record.FileName, recordXE);
                updateElectricRecordsList();
            }
            else
            {
                Gas_Records.Remove(sender as User_Controls.RecordObject);
                User_Controls.RecordObject record = (sender as User_Controls.RecordObject);
                XElement recordXE = new XElement("Record");
                recordXE.Add(new XAttribute("Type", (int)record.Type));
                recordXE.Add(new XAttribute("DateTime", record.RecordDateTime));
                recordXE.Add(new XAttribute("Balance", record.Balance.ToString()));
                recordXE.Add(new XAttribute("TopUp", record.TopUp.ToString()));
                XMLHandle.deleteFromFile(record.FileName, recordXE);
                updateGasRecordsList();
            }
        }

        private void OrderStackPanel(ListBox RecordList)
        {
            RecordList.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("RecordDateTime", System.ComponentModel.ListSortDirection.Descending));
        }

        private void AppSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private string[] LoadRecordFileNames()
        {
            string[] filenames = Directory.GetFiles(Properties.Settings.Default.RecordFileLocation);

            return filenames;
        }

        public bool withinTimePeriod(DateTime testDateTime, TimePeriod timePeriod)
        {
            if (testDateTime >= timePeriod.Start)
            {
                if (testDateTime < timePeriod.End)
                {
                    return true;
                }
            }
            return false;
        }

        private void updateGasRecordsList()
        {
            GasRecordListBox.Items.Clear();
            foreach (User_Controls.RecordObject record in Gas_Records.Where(r => withinTimePeriod(r.RecordDateTime, RecordViewPeriod)))
            {
                GasRecordListBox.Items.Add(record);
            }
            OrderStackPanel(GasRecordListBox);
            CalculateGasData();
        }
        private void updateElectricRecordsList()
        {
            ElectricRecordListBox.Items.Clear();
            foreach (User_Controls.RecordObject record in Electric_Records.Where(r => withinTimePeriod(r.RecordDateTime, RecordViewPeriod)))
            {
                ElectricRecordListBox.Items.Add(record);
            }
            OrderStackPanel(ElectricRecordListBox);
            CalculateElecData();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string SelectedText = (TimePeriodSelector.SelectedItem as ComboBoxItem).Content.ToString();
            if (SelectedText == "This Week")
            {
                RecordViewPeriod.Start = DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek));
                RecordViewPeriod.End = DateTime.Today.AddDays(1);
            }
            else if (SelectedText == "This Month")
            {
                RecordViewPeriod.Start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                RecordViewPeriod.End = DateTime.Today.AddDays(1);
            }
            else if (SelectedText == "This Year")
            {
                RecordViewPeriod.Start = new DateTime(DateTime.Today.Year, 1, 1);
                RecordViewPeriod.End = DateTime.Today.AddDays(1);
            }
            else if (SelectedText == "Past Week")
            {
                RecordViewPeriod.Start = DateTime.Today.AddDays(-7);
                RecordViewPeriod.End = DateTime.Today.AddDays(1);
            }
            else if (SelectedText == "Past Month")
            {
                RecordViewPeriod.Start = DateTime.Today.AddMonths(-1);
                RecordViewPeriod.End = DateTime.Today.AddDays(1);
            }
            else if (SelectedText == "Past Year")
            {
                RecordViewPeriod.Start = DateTime.Today.AddYears(-1);
                RecordViewPeriod.End = DateTime.Today.AddDays(1);

            }
            else if (SelectedText == "All Records")
            {
                RecordViewPeriod.Start = DateTime.MinValue;
                RecordViewPeriod.End = DateTime.MaxValue;                
            }
            updateElectricRecordsList();
            updateGasRecordsList();
        }
        private void CalculateElecData()
        {
            if (ElectricRecordListBox.Items.Count > 0)
            {
                ElectricCalculations.Calculate(ElectricRecordListBox.Items.Cast<User_Controls.RecordObject>().ToList());
            }
            else
            {
                ElectricCalculations.ClearData();
            }
        }
        private void CalculateGasData()
        {
            if (GasRecordListBox.Items.Count > 0)
            {
                GasCalculations.Calculate(GasRecordListBox.Items.Cast<User_Controls.RecordObject>().ToList());
            }
            else 
            {
                GasCalculations.ClearData();
            }
        }

    }
}
