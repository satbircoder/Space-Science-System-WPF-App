using Galileo6;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Stopwatch stopWatch = new Stopwatch();
        private void LoadDataList()
        {
            SensorAList.Clear();
            SensorBList.Clear();
            int ListSize = 400;
            ReadData readSensorData = new ReadData();
            for(int i = 0; i< ListSize; i++)
            {
                SensorAList.AddLast(readSensorData.SensorA((double)MuValue.Value, (double)SigmaValue.Value));
                SensorBList.AddLast(readSensorData.SensorB((double)MuValue.Value, (double)SigmaValue.Value));
            }
        }
        private void ShowAllSensorData()
        {
            ListViewDisplay.Items.Clear();
            for(int i=0; i< SensorAList.Count; i++)
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
            DisplayListBoxData(SensorAList,ListBoxDisplayA);
            DisplayListBoxData(SensorBList,ListBoxDisplayB);
        }
        private int NumberOfNodes(LinkedList<double> nodeCounter)
        {
            return nodeCounter.Count;
        }
        private void DisplayListBoxData(LinkedList<double> list, ListBox listBox)
        {
            listBox.Items.Clear();
            foreach (var loadListBox in list)
            {
                listBox.Items.Add(loadListBox);
            }
        }
        private bool SelectionSort(LinkedList<double> unsortedLinkList)
        {
            bool sorted = false;
            int min = 0;
            int max = NumberOfNodes(unsortedLinkList);
            for(int i =0; i < max-1; i++)
            {
                min = i;
                for(int j = i+1; j < max; j++)
                {
                    if(unsortedLinkList.ElementAt(j).CompareTo(unsortedLinkList.ElementAt(min)) < 0)
                    {
                        min = j;
                    }
                }
                if(min != i)
                {
                    LinkedListNode<double> currentMin = unsortedLinkList.Find(unsortedLinkList.ElementAt(min));
                    LinkedListNode<double> currentI = unsortedLinkList.Find(unsortedLinkList.ElementAt(i));
                    var temp = currentMin.Value;
                    currentMin.Value = currentI.Value;
                    currentI.Value = temp;
                    sorted= true;
                }
            }
            return sorted;
        }
        private bool InsertionSort(LinkedList<double> unsortedLinkList)
        {
            int max = NumberOfNodes(unsortedLinkList);
            for(int i = 0; i < max-1; i++)
            {
                for(int j = i+1; j > 0; j--)
                {
                    if(unsortedLinkList.ElementAt(j-1).CompareTo(unsortedLinkList.ElementAt(j)) > 0)
                    {
                        LinkedListNode<double> current = unsortedLinkList.Find(unsortedLinkList.ElementAt(j));
                        var temp = current.Previous.Value;
                        current.Previous.Value = current.Value;
                        current.Value = temp;
                    }
                }
            }
            return true;
        }
        private void SelectionSortB_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();
            stopWatch.Start();
            SelectionSort(SensorBList);
            SelectionSortTimeB.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
            DisplayListBoxData(SensorBList, ListBoxDisplayB);
        }
        private void SelectionSortA_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();
            stopWatch.Start();
            SelectionSort(SensorAList);
            stopWatch.Stop();
            SelectionSortTime.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
            DisplayListBoxData(SensorAList,ListBoxDisplayA);
        }
        private void InsertionSortA_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();
            stopWatch.Start();
            InsertionSort(SensorAList);
            stopWatch.Stop();
            InsertionSortTime.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
            DisplayListBoxData(SensorAList,ListBoxDisplayA);
        }
        private void InsertionSortB_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();           
            stopWatch.Start();
            InsertionSort(SensorBList);
            stopWatch.Stop();
            InsertionSortTimeB.Text =  stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
            DisplayListBoxData(SensorBList,ListBoxDisplayB);
        }
        private int BinarySearchIterative(LinkedList<double> list, int min, int max, int searchValue)
        {
            while(min <= max-1)
            {
                int middle = (min + max) / 2;
                if(searchValue == list.ElementAt(middle))
                {
                    return ++middle;
                }
                else if(searchValue < list.ElementAt(middle))
                {
                    max = middle - 1;
                }
                else
                {
                    min = middle + 1;
                }
            }
            return min;
        }
        private int BinarySearchRecursive(LinkedList<double> list, int min, int max, int searchValue)
        {
            if (min <= max - 1)
            {
                int middle = (min + max) / 2;
                if(searchValue == list.ElementAt(middle)){
                    return middle;
                }
                else if(searchValue < list.ElementAt(middle))
                {
                    return BinarySearchRecursive(list, 0, middle-1, searchValue);
                }
                else
                {
                    return BinarySearchRecursive(list,middle+1, max, searchValue);
                }
            }
            return min;
        }
        private void HighlightSearchData(LinkedList<double> list, ListBox highlightList, int index)
        {
            highlightList.SelectedIndex = -1;
            for(int i = index-2; i<index+3; i++)
            {
                highlightList.SelectedItems.Add(list.ElementAt(i));
                highlightList.Focus();
                highlightList.ScrollIntoView(highlightList.Items[highlightList.SelectedIndex+2]);
            }
        }
        private void IterativeSearchA_Click(object sender, RoutedEventArgs e)
        {
            IterativeTime.Clear();
            stopWatch.Reset();
            stopWatch.Start();
            int found = BinarySearchIterative(SensorAList, 0, NumberOfNodes(SensorAList), Int32.Parse(SearchBoxA.Text));
            stopWatch.Stop();
            HighlightSearchData(SensorAList, ListBoxDisplayA, found);
            IterativeTime.Text = stopWatch.ElapsedTicks.ToString() + " Ticks";
        }
        private void IterativeSearchB_Click(object sender, RoutedEventArgs e)
        {
            IterativeTimeB.Clear();
            stopWatch.Reset();
            stopWatch.Start();
            int found = BinarySearchIterative(SensorBList, 0, NumberOfNodes(SensorBList), Int32.Parse(SearchBoxB.Text));
            stopWatch.Stop();
            HighlightSearchData(SensorBList, ListBoxDisplayB, found);
            IterativeTimeB.Text = stopWatch.ElapsedTicks.ToString() + " Ticks";
        }
        private void RecursiveSearchA_Click(object sender, RoutedEventArgs e)
        {
            RecursiveTime.Clear();
            stopWatch.Reset();
            stopWatch.Start();
            int found = BinarySearchRecursive(SensorAList, 0, NumberOfNodes(SensorAList), Int32.Parse(SearchBoxA.Text));
            stopWatch.Stop();
            HighlightSearchData(SensorAList, ListBoxDisplayA, found);
            RecursiveTime.Text = stopWatch.ElapsedTicks.ToString() + " Ticks";
        }
        private void RecursiveSearchB_Click(object sender, RoutedEventArgs e)
        {
            RecursiveTimeB.Clear();
            stopWatch.Reset();
            stopWatch.Start();
            int found = BinarySearchRecursive(SensorBList, 0, NumberOfNodes(SensorBList), Int32.Parse(SearchBoxB.Text));
            stopWatch.Stop();
            HighlightSearchData(SensorBList, ListBoxDisplayB, found);
            RecursiveTimeB.Text = stopWatch.ElapsedTicks.ToString() + " Ticks";
        }
    }
}

