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
    /// Interaction logic for PinWheelDialog.xaml
    /// </summary>
    public partial class PinWheelDialog : Window
    {
        /// <summary>
        /// This is the instance of the BackGroundWorker which will be cancelled if the cancel button is clicked.
        /// </summary>
        BackgroundWorker worker;
        //Duration time = new Duration(TimeSpan.FromSeconds(2));

        public PinWheelDialog(BackgroundWorker bw, TimeSpan animationSpeed)//, int animationDuration = 2)
        {
            WindowSetup();

            worker = bw;
            //time = new Duration(TimeSpan.FromSeconds(animationDuration));
        }

        public PinWheelDialog(DoWorkEventHandler doWork, RunWorkerCompletedEventHandler workerComplete, int animationDuration = 2)
        {
            WindowSetup();

            InitialiseWorker(doWork, workerComplete);
            
            //time = new Duration(TimeSpan.FromSeconds(animationDuration));
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

            Duration speed = (Duration)FindResource("AnimationSpeed");

            speed = new Duration(TimeSpan.FromSeconds(1));

            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.Width) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.Height) / 2;

            speed = (Duration)FindResource("AnimationSpeed");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void WinPinWheel_Closing(object sender, CancelEventArgs e)
        {
            worker.CancelAsync();
        }
    }
}
