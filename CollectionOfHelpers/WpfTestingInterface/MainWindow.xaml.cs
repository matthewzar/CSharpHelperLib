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
using System.ComponentModel;
using WpfTestingInterface.ProgressDialogs;

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

        #region Smooth Progressbar animation
        /// <summary>
        /// Demo the animation of the progressbar from current value to 100% over 2 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowProgress_Click(object sender, RoutedEventArgs e)
        {
            PrgProgressBar.SetPercent(100, 2);
        }

        /// <summary>
        /// Demo the animation of the progressbar from current value to 0% over 2 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReverse_Click(object sender, RoutedEventArgs e)
        {
            PrgProgressBar.SetPercent(0, 2);
        }

        /// <summary>
        /// Demo the animation of the progressbar from current value to 0% over 0 seconds (instant reset)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            //Instantly sets the Progressbar percentage to 0
            PrgProgressBar.SetPercent(0, 0);
            //If you use the animation ability you won't be able to use Progressbar.Value anymore
            //PrgProgressBar.Value = 0; <-- Doesn't work
        }
        #endregion

        #region ProgressDialog examples
        /// <summary>
        /// This is the window being used to display the progress of a backgroundworker thread.
        /// The worker belongs to the window.
        /// </summary>
        Window pop;

        /// <summary>
        /// Worker Complete event for the BackGroundWorker. It closes the progressdialog and reports whether the action was cancelled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pop.Close();
            if (e.Cancelled)
            {
                MessageBox.Show("Process was cancelled");
            }
        }

        /// <summary>
        /// DoWork event for the BackGroundWorker. The thread sleeps for a while and updates progress in a loop.
        /// If the BackGroundWorker is being cancelled then we exit the loop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var theWorker = sender as BackgroundWorker;
            for (int i = 1; i < 25; i++)
            {
                if (theWorker.WorkerReportsProgress)
                {
                    theWorker.ReportProgress(i * 4);
                } 
                Thread.Sleep(2000);

                if (theWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Demo of a Dialog box with a progressbar that gets updated by a BackGroundWorker
        /// The BackGroundWorker can either be sent through to the dialog or created by the dialog
        /// Background worker reports progress
        /// Optional Label which shows the current progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProgressBarDialog_Click(object sender, RoutedEventArgs e)
        {
            pop = new ProgressBarDialog(bw_DoWork, bw_RunWorkerCompleted);
            //Makes the progressbar value label invisible
            //pop = new ProgressBarDialog(bw_DoWork, bw_RunWorkerCompleted, false);

            pop.ShowDialog();
        }

        /// <summary>
        /// Demo of a Dialog box with a busy indicator
        /// The BackGroundWorker can either be sent through to the dialog or created by the dialog
        /// Background worker doesn't report progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBusyIndicator_Click(object sender, RoutedEventArgs e)
        {
            pop = new BusyIndicator(bw_DoWork, bw_RunWorkerCompleted);

            pop.ShowDialog();
        }

        /// <summary>
        /// Demo of a Dialog box with a progressbar set to IsIndeterminate="True"
        /// The BackGroundWorker can either be sent through to the dialog or created by the dialog
        /// Background worker doesn't report progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProgressIndicatorDialog_Click(object sender, RoutedEventArgs e)
        {
            pop = new ProgressIndicatorDialog(bw_DoWork, bw_RunWorkerCompleted);

            pop.ShowDialog();
        }

        /// <summary>
        /// Demo of a Dialog box with a spinning image (pin wheel animation)
        /// The BackGroundWorker can either be sent through to the dialog or created by the dialog
        /// Background worker doesn't report progress
        /// The speed of the animation can't be set on creation of the window, it is a static resource in the animation style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPinWheelDialog_Click(object sender, RoutedEventArgs e)
        {
            pop = new PinWheelDialog(bw_DoWork, bw_RunWorkerCompleted);

            pop.ShowDialog();
        }
        #endregion
    }
}