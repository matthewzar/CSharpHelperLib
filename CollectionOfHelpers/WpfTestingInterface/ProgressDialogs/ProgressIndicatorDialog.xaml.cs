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
    /// Interaction logic for ProgressIndicatorDialog.xaml
    /// </summary>
    public partial class ProgressIndicatorDialog : Window
    {
        /// <summary>
        /// This is the instance of the BackGroundWorker which will be cancelled if the cancel button is clicked.
        /// </summary>
        BackgroundWorker worker;

        public ProgressIndicatorDialog(BackgroundWorker bw)
        {
            InitializeComponent();
            WindowSetup();

            worker = bw;
        }

        public ProgressIndicatorDialog(DoWorkEventHandler doWork, RunWorkerCompletedEventHandler workerComplete)
        {
            InitializeComponent();
            WindowSetup();

            InitialiseWorker(doWork, workerComplete);
        }

        private void InitialiseWorker(DoWorkEventHandler doWork, RunWorkerCompletedEventHandler workerComplete)
        {
            worker = new BackgroundWorker();

            worker.DoWork += doWork;
            worker.RunWorkerCompleted += workerComplete;
            worker.WorkerSupportsCancellation = true;
        }

        private void WindowSetup()
        {
            InitializeComponent();

            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.Width) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.Height) / 2;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            worker.CancelAsync();
        }
    }
}
