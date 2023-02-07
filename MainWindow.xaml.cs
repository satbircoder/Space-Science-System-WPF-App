using Galileo6;
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
//using Galileo6;

namespace Marlin_Space_System_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        LinkedList<double> SensorAList = new LinkedList<double>();
        LinkedList<double> SensorBList= new LinkedList<double>();
        private void LoadDataList()
        {
            SensorAList.Clear();
            SensorBList.Clear();
            int ListSize = 400;
            ReadData readSensorData = new ReadData();
            for(int i = 0; i< ListSize; i++)
            {
                SensorAList.AddLast(readSensorData.SensorA(MuValue.Value, SigmaValue.Value));
                SensorBList.AddLast(readSensorData.SensorB(MuValue.Value, SigmaValue.Value));
            }
        }
        private void ShowAllSensorData()
        {
            int ListSize = 400;
            ListViewDisplay.Items.Clear();
            for(int i=0; i< ListSize; i++)
            {
                ListViewDisplay.Items.Add(new
                {
                    SensorAList = SensorAList.ElementAt(i).ToString(),
                    SensorBList = SensorBList.ElementAt(i).ToString()
                });
            }
            

        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList();
            ShowAllSensorData();
        }
    }
    

}

