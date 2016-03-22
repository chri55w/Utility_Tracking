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
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
