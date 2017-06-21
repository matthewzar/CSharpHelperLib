using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace WpfTestingInterface.ProgressDialogs
{
    /// <summary>
    /// Interaction logic for BusyIndicator.xaml
    /// </summary>
    public partial class BusyIndicator : Window
    {
        /// <summary>
        /// This is the instance of the BackGroundWorker which will be cancelled if the cancel button is clicked.
        /// </summary>
        BackgroundWorker worker;

        public BusyIndicator(BackgroundWorker bw)
        {
            InitializeComponent();

            BusyBar.IsBusy = true;

            worker = bw;
        }

        public BusyIndicator(DoWorkEventHandler doWork, RunWorkerCompletedEventHandler workerComplete)
        {
            InitializeComponent();

            BusyBar.IsBusy = true;

            InitialiseWorker(doWork, workerComplete);
        }

        /// <summary>
        /// Initialises the settings for the BackGroundWorker
        /// </summary>
        /// <param name="doWork"></param>
        /// <param name="workerComplete"></param>
        private void InitialiseWorker(DoWorkEventHandler doWork, RunWorkerCompletedEventHandler workerComplete)
        {
            worker = new BackgroundWorker();

            worker.DoWork += doWork;
            worker.RunWorkerCompleted += workerComplete;
            worker.WorkerSupportsCancellation = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BusyBar.IsBusy = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }
    }
}
