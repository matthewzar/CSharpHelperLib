﻿using System;
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

        private BackgroundWorker bw = new BackgroundWorker();
        Window pop;

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

        /// <summary>
        /// Initialising the window with a BackGroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bw.WorkerReportsProgress = true;          
            bw.DoWork += bw_DoWork; 
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        /// <summary>
        /// Demo of a Dialog box with a progressbar that gets updated by a BackGroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProgressBarDialog_Click(object sender, RoutedEventArgs e)
        {
            pop = new ProgressBarDialog(bw);

            bw.WorkerSupportsCancellation = true;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerAsync();

            pop.ShowDialog();
        }

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
        /// Progress Changed event for the BackGroundWorker. If the worker isn't being cancelled then it updates the progressbar.
        /// This will change as different dialogs are created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!bw.CancellationPending)
            {
                (pop as ProgressBarDialog).PrgProgressBar.SetPercent(e.ProgressPercentage);
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
            for (int i = 1; i < 25; i++)
            {   
                bw.ReportProgress(i*4);
                Thread.Sleep(2000);

                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}