using Galileo6;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace Marlin_Space_System_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //4.1	Create two data structures using the LinkedList<T> class from the C# Systems.Collections.
        //Generic namespace. The data must be of type “double”; you are not permitted to use any additional classes,
        //nodes, pointers or data structures (array, list, etc) in the implementation of this application.
        //The two LinkedLists of type double are to be declared as global within the “public partial class”.
        LinkedList<double> SensorAList = new LinkedList<double>();
        LinkedList<double> SensorBList= new LinkedList<double>();
        Stopwatch stopWatch = new Stopwatch();// stopwatch for time calculation 

        // 4.2	Copy the Galileo.DLL file into the root directory
        // of your solution folder and add the appropriate reference in the solution
        // explorer.Create a method called “LoadData” which will populate both LinkedLists.
        // Declare an instance of the Galileo library in the method and create the appropriate loop construct
        // to populate the two LinkedList; the data from Sensor A will populate the first LinkedList, while the
        // data from Sensor B will populate the second LinkedList.The LinkedList size will be hardcoded inside
        // the method and must be equal to 400. The input parameters are empty, and the return type is void.

        #region Load Data 
        private void LoadDataList()// This Method is filling up the data in the Both linked lists using a declared size 400 
        {
            SensorAList.Clear();
            SensorBList.Clear();
            int ListSize = 400;
            ReadData readSensorData = new ReadData();// Instance from Galileo6 Namespace to get the data 
            for(int i = 0; i< ListSize; i++)
            {
               SensorAList.AddLast(readSensorData.SensorA((double)MuValue.Value, (double)SigmaValue.Value));
               SensorBList.AddLast(readSensorData.SensorB((double)MuValue.Value, (double)SigmaValue.Value));
            }
        }
        #endregion Load Data


        //4.3	Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a ListView.
        //Add column titles “Sensor A” and “Sensor B” to the ListView.
        //The input parameters are empty, and the return type is void.

        #region Show Sensors Data
        private void ShowAllSensorData()// This Method is displaying the data in listview that was filled using Galileo6
        {
            ListViewDisplay.Items.Clear();
            for(int i=0; i< SensorAList.Count; i++)
            {
                ListViewDisplay.Items.Add(new //Data has been added using display binding and these are declared in XAML  
                {
                    SensorAList = SensorAList.ElementAt(i).ToString(),
                    SensorBList = SensorBList.ElementAt(i).ToString()
                });
            }
        }
        #endregion Show Sensors Data

        //4.4	Create a button and associated click method that will call
        //the LoadData and ShowAllSensorData methods.
        //The input parameters are empty, and the return type is void.

        #region Load Button Click
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList();
            ShowAllSensorData();
            DisplayListBoxData(SensorAList,ListBoxDisplayA);
            DisplayListBoxData(SensorBList,ListBoxDisplayB);
            StatusBar.Text = "Data has Been Loaded Successfully";
        }
        #endregion Load Button Click

        //4.5	Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
        //The method signature will have an input parameter of type LinkedList,
        //and the calling code argument is the linkedlist name.

        #region Utility Methods
        private int NumberOfNodes(LinkedList<double> nodeCounter)
        {
            return nodeCounter.Count;// Returning the number of nodes in the Linked list
        }
        private void HighlightSearchData(LinkedList<double> list, ListBox highlightList, int index)// This Method is Highlighting the range of found data
        {
            highlightList.SelectedIndex = -1;
            for (int i = index - 2; i < index + 3; i++)
            {
                highlightList.SelectedItems.Add(list.ElementAt(i));
                highlightList.Focus();
                highlightList.ScrollIntoView(highlightList.Items[highlightList.SelectedIndex + 8]);
            }
        }
        #endregion Utility Methods

        //4.6	Create a method called “DisplayListboxData” that will display the
        //content of a LinkedList inside the appropriate
        //ListBox. The method signature will have two input parameters;
        //a LinkedList, and the ListBox name.  The calling
        //code argument is the linkedlist name and the listbox name.

        #region Display List Box Data
        private void DisplayListBoxData(LinkedList<double> list, ListBox listBox)// This Function is displaying the Data
                                                                                 // of appropriate sensors in Appropriate List Box
        {
            listBox.Items.Clear();
            foreach (var loadListBox in list)
            {
                listBox.Items.Add(loadListBox);
            }
        }

        #endregion Display List Box Data

        #region Sorting Methods 
        //4.7	Create a method called “SelectionSort” which has a single input parameter of type LinkedList,
        //while the calling code argument is the linkedlist name.
        //The method code must follow the pseudo code supplied below in the Appendix.
        //The return type is Boolean.
        private bool SelectionSort(LinkedList<double> unsortedLinkList)
        {
            bool sorted = false;
            int min;
            int max = NumberOfNodes(unsortedLinkList);
            for(int i =0; i < max-1; i++)
            {
                min = i;
                for(int j = i+1; j < max; j++)
                {
                    if(unsortedLinkList.ElementAt(j) < unsortedLinkList.ElementAt(min))// comparing the elements found in looping through
                                                                                       // inner loop and outer loop and performing as required. 
                    {
                        min = j;
                    }
                }
                if (min != i)
                {
                    LinkedListNode<double> currentMin = unsortedLinkList.Find(unsortedLinkList.ElementAt(min));
                    LinkedListNode<double> currentI = unsortedLinkList.Find(unsortedLinkList.ElementAt(i));
                    var temp = currentMin.Value;//swapping of values 
                    currentMin.Value = currentI.Value;
                    currentI.Value = temp;
                    sorted= true;
                }
            }
            return sorted;
        }
        
        //4.8	Create a method called “InsertionSort” which has a single parameter of type LinkedList,
        //while the calling code argument is the linkedlist name.
        //The method code must follow the pseudo code supplied below in the Appendix.
        //The return type is Boolean.
        private bool InsertionSort(LinkedList<double> unsortedLinkList)
        {
            int max = NumberOfNodes(unsortedLinkList);
            for(int i = 0; i < max-1; i++)
            {
                for(int j = i+1; j > 0; j--)
                {
                    if(unsortedLinkList.ElementAt(j-1) > unsortedLinkList.ElementAt(j))
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
        #endregion Sorting Methods

        #region Searching Methods
        //4.9	Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        //This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        //The calling code argument is the linkedlist name,
        //search value, minimum list size and the number of nodes in the list. The method
        //code must follow the pseudo code supplied below in the Appendix.


        private int BinarySearchIterative(LinkedList<double> llist, int min, int max, int searchValue)
        {
            while (min <= max - 1)
            {
                int middle = (min + max) / 2;
                if (searchValue == llist.ElementAt(middle))
                {
                    return ++middle;
                }
                else if (searchValue < llist.ElementAt(middle))
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

        //4.10	Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        //This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        //The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list. The method
        //code must follow the pseudo code supplied below in the Appendix.
        private int BinarySearchRecursive(LinkedList<double> list, int min, int max, int searchValue)
        {
            if (min <= max - 1)
            {
                int middle = (min + max) / 2;
                if (searchValue == list.ElementAt(middle))
                {
                    return middle;
                }
                else if (searchValue < list.ElementAt(middle))
                {
                    return BinarySearchRecursive(list, min, middle - 1, searchValue);
                }
                else
                {
                    return BinarySearchRecursive(list, middle + 1, max, searchValue);
                }
            }
            return min;
        }
        #endregion Searching Methods

        #region Search Buttons Methods
        //4.11,1 
        //Method for Sensor A and Binary Search Iterative

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
        //4.11, 2
        //Method for Sensor A and Binary Search Recursive
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
        //4.11, 3
        //Method for Sensor B and Binary Search Iterative
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
        //4.11, 4
        //Method for Sensor B and Binary Search Recursive
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
        #endregion Search Buttons Methods

        #region Sort Buttons Methods
        //4.12, 1
        //Method for Sensor A and Selection Sort
        private void SelectionSortA_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();
            stopWatch.Start();
            SelectionSort(SensorAList);
            stopWatch.Stop();
            SelectionSortTime.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
            DisplayListBoxData(SensorAList, ListBoxDisplayA);
        }
        //4.12, 2
        //Method for Sensor A and Insertion Sort
        private void InsertionSortA_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();
            stopWatch.Start();
            InsertionSort(SensorAList);
            stopWatch.Stop();
            InsertionSortTime.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
            DisplayListBoxData(SensorAList, ListBoxDisplayA);
        }
        //4.12, 3
        //Method for Sensor B and Selection Sort
        private void SelectionSortB_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();
            stopWatch.Start();
            SelectionSort(SensorBList);
            SelectionSortTimeB.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
            DisplayListBoxData(SensorBList, ListBoxDisplayB);
        }
        //4.12, 4
        //Method for Sensor B and Insertion Sort
        private void InsertionSortB_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();           
            stopWatch.Start();
            InsertionSort(SensorBList);
            stopWatch.Stop();
            InsertionSortTimeB.Text =  stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
            DisplayListBoxData(SensorBList,ListBoxDisplayB);
        }





        #endregion Sort Buttons Methods

        #region Input Validation
        private void Preview_Text_Input(object sender, TextCompositionEventArgs e)// This Custom made method is
                                                                                  // used to validate the input of
                                                                                  // numeric up downs and search boxes
        {
            var regex = new Regex("[^0-9]+");// this will validate the input to numbers only 
            e.Handled = regex.IsMatch(e.Text);
        }
        private void SigmaValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Preview_Text_Input(sender, e);
        }

        private void SigmaValue_KeyDown(object sender, KeyEventArgs e)// This will disable the keyboard input in Sigma
        {
            e.Handled = true;
        }
        

        private void MuValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Preview_Text_Input(sender, e);
        }

        private void MuValue_KeyDown(object sender, KeyEventArgs e)//This will disable the keyboard input in Mu Box
        {
            e.Handled= true;
        }

        private void SearchBoxA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Preview_Text_Input(sender,e);
        }

        private void SearchBoxB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Preview_Text_Input(sender,e);
        }
        #endregion Input Validation
    }
}

