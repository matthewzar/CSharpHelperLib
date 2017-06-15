using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using CollectionOfHelpers.GeneralExtensions;
using CollectionOfHelpers.Threading;

namespace WpfTestingInterface
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

        /// <summary>
        /// Expand the TreeViewItem from the first root node in it's collection.
        /// The depth is specified by IupExpandDepth - a depth of 0 shouldn't expand anything.
        /// IMPORTANT: only the first node is expanded, as the extension method being tested is
        /// for TreeViewItem, not TreeView (see TreeView.ExpandToDepth for that behaviour)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExpandTest_Click(object sender, RoutedEventArgs e)
        {
            var maxDepth = IupExpandDepth.Value;
            if (maxDepth == null)
            {
                return;
            }
            (treeView.Items[0] as TreeViewItem)?.ExpandToDepth((int) maxDepth);
        }

        /// <summary>
        /// Check that the treeView.ExpandToDepth works as expected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExpandTreeTest_Click(object sender, RoutedEventArgs e)
        {
            var maxDepth = IupExpandTreeDepth.Value;
            if (maxDepth == null)
            {
                return;
            }
            treeView.ExpandToDepth((int) maxDepth);
        }

        /// <summary>
        /// Demo that the top node (and all it's children) are contracted if they
        /// were expanded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnContractFirst_Click(object sender, RoutedEventArgs e)
        {
            (treeView.Items[0] as TreeViewItem)?.ContractAll();
        }

        private void BtnContractTree_Click(object sender, RoutedEventArgs e)
        {
            treeView.ContractAll();
        }

        private Thread[] ThreadsToMonitor;
        //private ThreadedEventCounter threadedEventCounter;

        private void BtnStartThreadMonitoring_Click(object sender, RoutedEventArgs e)
        {
            if (ThreadsToMonitor != null)
            {
                return;
            }

            //notice that this is declared locally, and not directly stored, but it's outputs continue to show up.
            //This is because a reference to it exists inside each of the child threads.
            //However even without that reference the isntance  and it's thread will continue to work as they are self-referential (at least until the first timeout event)
            ThreadedEventCounter threadedEventCounter = new ThreadedEventCounter(new[]{"Odd","Even"}, 5, 5000, 100);
            ThreadsToMonitor = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                var threadID = i;
                ThreadsToMonitor[i] = new Thread(() =>
                {
                    while (true)
                    {
                        Console.WriteLine(threadID + "");
                        Thread.Sleep(1000+threadID*10);
                        threadedEventCounter.IncrementEvent(threadID%2 == 0 ? "Even" : "Odd");
                    }
                });

                ThreadsToMonitor[i].Start();
            }
        }

        private void BtnKillThreads_Click(object sender, RoutedEventArgs e)
        {
            if (ThreadsToMonitor == null)
            {
                return;
            }

            foreach (var thread in ThreadsToMonitor)
            {
                thread.Abort();
            }
            ThreadsToMonitor = null;

        }

        private void BtnShowProgress_Click(object sender, RoutedEventArgs e)
        {
            PrgProgressBar.SetPercent(100, 2);
        }

        private void BtnReverse_Click(object sender, RoutedEventArgs e)
        {
            PrgProgressBar.SetPercent(0, 2);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            //Instantly sets the Progressbar percentage to 0
            PrgProgressBar.SetPercent(0, 0);
            //If you use the animation ability you won't be able to use Progressbar.Value anymore
            //PrgProgressBar.Value = 0; <-- Doesn't work
        }
    }
}