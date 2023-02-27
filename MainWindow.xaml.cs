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
//Satbir Singh
//Date 20/02/2023
//This is a WPF app for multi platform that is collecting the data from the sensors of sattelite and allow user to sort and search that data 

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
        LinkedList<double> SensorBList = new LinkedList<double>();

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
            try
            {
                for (int i = 0; i < ListSize; i++)
                {
                    SensorAList.AddLast(readSensorData.SensorA((double)MuValue.Value, (double)SigmaValue.Value));
                    SensorBList.AddLast(readSensorData.SensorB((double)MuValue.Value, (double)SigmaValue.Value));
                }
            }
            catch (Exception ex)
            {
                StatusBar.Text = "Something went wrong in loading data please try again";
            }
        }
        #endregion Load Data


        //4.3	Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a ListView.
        //Add column titles “Sensor A” and “Sensor B” to the ListView.
        //The input parameters are empty, and the return type is void.

        #region Show Sensors Data
        private void ShowAllSensorData()// This Method is displaying the data in listview that was filled using Galileo6
        {
            try
            {

                ListViewDisplay.Items.Clear();
                for (int i = 0; i < SensorAList.Count; i++)
                {
                    ListViewDisplay.Items.Add(new //Data has been added using display binding and these are declared in XAML  
                    {
                        SensorAList = SensorAList.ElementAt(i).ToString(),
                        SensorBList = SensorBList.ElementAt(i).ToString()
                    });
                }
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong in Displaying Data Please Try Again";
            }
        }
        #endregion Show Sensors Data

        //4.4	Create a button and associated click method that will call
        //the LoadData and ShowAllSensorData methods.
        //The input parameters are empty, and the return type is void.

        #region Load Button Click
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDataList();
                ShowAllSensorData();
                StatusBar.Text = "Data has Been Loaded Successfully";
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong with Load Button Please Try Again";
            }
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
            if (index >= 0 && index <= 399)
            {
                try
                {
                    if (index < 2)
                    {
                        for (int i = index; i <= index + 3; i++)
                        {
                            highlightList.SelectedItems.Add(list.ElementAt(i));
                            highlightList.Focus();
                            highlightList.ScrollIntoView(highlightList.Items[highlightList.SelectedIndex + 1]);
                        }
                    }
                    else if (index > 396 && index <= 399)
                    {
                        for (int i = index - 2; i <= index; i++)
                        {
                            highlightList.SelectedItems.Add(list.ElementAt(i));
                            highlightList.Focus();
                            highlightList.ScrollIntoView(highlightList.Items[highlightList.SelectedIndex + 0]);
                        }
                    }
                    else if (index >= 2 || index <= 396)
                    {
                        for (int i = index - 2; i < index + 3; i++)
                        {
                            highlightList.SelectedItems.Add(list.ElementAt(i));
                            highlightList.Focus();
                            highlightList.ScrollIntoView(highlightList.Items[highlightList.SelectedIndex + 4]);
                        }
                    }
                }
                catch (Exception)
                {
                    StatusBar.Text = "Something Went Wrong With Highlighting Data Please try again";
                }
            }
            else
            {
                StatusBar.Text = "Search Number is not in the Range";
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "";
        }
        #endregion Utility Methods

        //4.6	Create a method called “DisplayListboxData” that will display the
        //content of a LinkedList inside the appropriate
        //ListBox. The method signature will have two input parameters;
        //a LinkedList, and the ListBox name.  The calling
        //code argument is the linkedlist name and the listbox name.

        #region Display List Box Data
        private void DisplayListBoxData(LinkedList<double> list, ListBox listBox)// This Function is displaying the Data
        {
            try
            {
                listBox.Items.Clear();
                foreach (var loadListBox in list)
                {
                    listBox.Items.Add(loadListBox);
                }
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong with the Individual Display of sensor data Please Try Again";
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
            try
            {
                for (int i = 0; i < max - 1; i++)
                {
                    min = i;
                    for (int j = i + 1; j < max; j++)
                    {
                        if (unsortedLinkList.ElementAt(j) < unsortedLinkList.ElementAt(min))// comparing the elements found in looping through
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
                        sorted = true;
                    }
                }
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went wrong with Selection Sort Please Try Again";
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
            try
            {
                for (int i = 0; i < max - 1; i++)
                {
                    for (int j = i + 1; j > 0; j--)
                    {
                        if (unsortedLinkList.ElementAt(j - 1) > unsortedLinkList.ElementAt(j))
                        {
                            LinkedListNode<double> current = unsortedLinkList.Find(unsortedLinkList.ElementAt(j));
                            var temp = current.Previous.Value;
                            current.Previous.Value = current.Value;
                            current.Value = temp;
                        }
                    }
                }
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong in Insertion Sort Please Try Again";
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
            try
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
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong in Iterative Search Please Try again";
            }
            return min;
        }

        //4.10	Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        //This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        //The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list. The method
        //code must follow the pseudo code supplied below in the Appendix.
        private int BinarySearchRecursive(LinkedList<double> list, int min, int max, int searchValue)
        {
            try
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
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong With recursive search please try again";
            }
            return min;
        }
        #endregion Searching Methods

        #region Search Buttons Methods
        //4.11,1 
        //Method for Sensor A and Binary Search Iterative

        private void IterativeSearchA_Click(object sender, RoutedEventArgs e)
        {
            if (Search_Box_Input(SearchBoxA))
            {
                try
                {
                    if (SelectionSort(SensorAList) || InsertionSort(SensorAList))
                    {
                        IterativeTime.Clear();
                        var stopWatch = Stopwatch.StartNew();
                        int found = BinarySearchIterative(SensorAList, 0, NumberOfNodes(SensorAList), Int32.Parse(SearchBoxA.Text));
                        stopWatch.Stop();
                        HighlightSearchData(SensorAList, ListBoxDisplayA, found);
                        IterativeTime.Text = stopWatch.ElapsedTicks.ToString() + " Ticks";
                    }
                    else
                    {
                        StatusBar.Text = "Data Need To be Sorted";
                    }
                }
                catch (Exception)
                {
                    StatusBar.Text = "Something Went Wrong with SensorA Iterative Button Please try again";
                }
            }
        }
        //4.11, 2
        //Method for Sensor A and Binary Search Recursive
        private void RecursiveSearchA_Click(object sender, RoutedEventArgs e)
        {
            if (Search_Box_Input(SearchBoxA))
            {
                try
                {
                    if (SelectionSort(SensorAList) || InsertionSort(SensorAList))
                    {
                        RecursiveTime.Clear();
                        var stopWatch = Stopwatch.StartNew();
                        int found = BinarySearchRecursive(SensorAList, 0, NumberOfNodes(SensorAList), Int32.Parse(SearchBoxA.Text));
                        stopWatch.Stop();
                        HighlightSearchData(SensorAList, ListBoxDisplayA, found);
                        RecursiveTime.Text = stopWatch.ElapsedTicks.ToString() + " Ticks";
                    }
                }
                catch (Exception)
                {
                    StatusBar.Text = "Something Went Wrong with SensorA recursive Button Please try again";
                }
            }
        }
        //4.11, 3
        //Method for Sensor B and Binary Search Iterative
        private void IterativeSearchB_Click(object sender, RoutedEventArgs e)
        {
            if (Search_Box_Input(SearchBoxB))
            {
                try
                {
                    if (SelectionSort(SensorBList) || InsertionSort(SensorBList))
                    {
                        IterativeTimeB.Clear();
                        var stopWatch = Stopwatch.StartNew();
                        int found = BinarySearchIterative(SensorBList, 0, NumberOfNodes(SensorBList), Int32.Parse(SearchBoxB.Text));
                        stopWatch.Stop();
                        HighlightSearchData(SensorBList, ListBoxDisplayB, found);
                        IterativeTimeB.Text = stopWatch.ElapsedTicks.ToString() + " Ticks";
                    }

                }
                catch (Exception)
                {
                    StatusBar.Text = "Something Went Wrong With SensorB Iterative Button Please Try again";
                }
            }
        }
        //4.11, 4
        //Method for Sensor B and Binary Search Recursive
        private void RecursiveSearchB_Click(object sender, RoutedEventArgs e)
        {
            if (Search_Box_Input(SearchBoxB))
            {

                try
                {
                    if (SelectionSort(SensorBList) || InsertionSort(SensorBList))
                    {
                        RecursiveTimeB.Clear();
                        var stopWatch = Stopwatch.StartNew();
                        int found = BinarySearchRecursive(SensorBList, 0, NumberOfNodes(SensorBList), Int32.Parse(SearchBoxB.Text));
                        stopWatch.Stop();
                        HighlightSearchData(SensorBList, ListBoxDisplayB, found);
                        RecursiveTimeB.Text = stopWatch.ElapsedTicks.ToString() + " Ticks";
                    }
                }
                catch (Exception)
                {
                    StatusBar.Text = "Something Went Wrong With SensorB Recursive Button Please Try Again";
                }
            }
        }
        #endregion Search Buttons Methods

        #region Sort Buttons Methods
        //4.12, 1
        //Method for Sensor A and Selection Sort
        private void SelectionSortA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                SelectionSort(SensorAList);
                stopWatch.Stop();
                SelectionSortTime.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
                DisplayListBoxData(SensorAList, ListBoxDisplayA);
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong With SensorA Selection Sort Button Please Try Again";
            }
        }
        //4.12, 2
        //Method for Sensor A and Insertion Sort
        private void InsertionSortA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                InsertionSort(SensorAList);
                stopWatch.Stop();
                InsertionSortTime.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
                DisplayListBoxData(SensorAList, ListBoxDisplayA);
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong With SensorA Insertion Sort Button Please Try Again";
            }
        }
        //4.12, 3
        //Method for Sensor B and Selection Sort
        private void SelectionSortB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                SelectionSort(SensorBList);
                SelectionSortTimeB.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
                DisplayListBoxData(SensorBList, ListBoxDisplayB);
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong With SensorB Selection Sort Button Please Try Again";
            }
        }
        //4.12, 4
        //Method for Sensor B and Insertion Sort
        private void InsertionSortB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                InsertionSort(SensorBList);
                stopWatch.Stop();
                InsertionSortTimeB.Text = stopWatch.ElapsedMilliseconds.ToString() + " miliseconds";
                DisplayListBoxData(SensorBList, ListBoxDisplayB);
            }
            catch (Exception)
            {
                StatusBar.Text = "Something Went Wrong With SensorB Insertion Sort Button Please Try Again";
            }
        }
        #endregion Sort Buttons Methods

        #region Input Validation
        private void Preview_Text_Input(object sender, TextCompositionEventArgs e)// This Custom made method is
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
            e.Handled = true;
        }

        private void SearchBoxA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Preview_Text_Input(sender, e);
        }

        private void SearchBoxB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Preview_Text_Input(sender, e);
        }
        private bool Search_Box_Input(System.Windows.Controls.TextBox textBox)// This Custom Made Method will be restricting 
                                                                              // the Search Text Boxes Input to two digits 
                                                                              // it allows to enter but won't perform any operations
                                                                              // just returning the message to enter two digit numbers  
        {
            if (textBox.Text == "")
            {
                StatusBar.Text = "Enter the Number To Search";
                return false;
            }
            if (textBox.Text.Length > 3)
            {
                StatusBar.Text = "Only Three Digits are allowed";
                //textBox.Clear();
                textBox.Focus();
                return false;
            }
            else
            {
                return true;
            }


        }
        #endregion Input Validation
    }
}

