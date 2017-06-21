using CollectionOfHelpers.GeneralExtensions;
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
    /// Interaction logic for ProgressBarDialog.xaml
    /// </summary>
    public partial class ProgressBarDialog : Window
    {
        /// <summary>
        /// This is the instance of the BackGroundWorker which will be cancelled if the cancel button is clicked.
        /// </summary>
        BackgroundWorker worker;

        public ProgressBarDialog(BackgroundWorker bw)
        {
            InitializeComponent();

            PrgProgressBar.SetPercent(0);

            worker = bw;
        }

        public ProgressBarDialog(DoWorkEventHandler doWork, RunWorkerCompletedEventHandler workerComplete)
        {
            InitializeComponent();

            PrgProgressBar.SetPercent(0);

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
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += bw_ProgressChanged;

            worker.DoWork += doWork;
            worker.RunWorkerCompleted += workerComplete;
            worker.WorkerSupportsCancellation = true;          
        }

        /// <summary>
        /// Button that will cancel the BackGroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }

        /// <summary>
        /// Progress Changed event for the BackGroundWorker. If the worker isn't being cancelled then it updates the progressbar.
        /// This will change as different dialogs are created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!worker.CancellationPending)
            {
                PrgProgressBar.SetPercent(e.ProgressPercentage);
            }
        }

        /// <summary>
        /// The worker needs to begin executing once the window is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }
    }
}
