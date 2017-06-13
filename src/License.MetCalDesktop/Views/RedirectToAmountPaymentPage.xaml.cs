using License.MetCalDesktop.Common;
using License.MetCalDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace License.MetCalDesktop.Views
{
    /// <summary>
    /// Interaction logic for RedirectToAmountPaymentPage.xaml
    /// </summary>
    public partial class RedirectToAmountPaymentPage : Page
    {

        /// <summary>
        /// The backgroundworker object on which the time consuming operation shall be executed
        /// </summary>
        BackgroundWorker m_oWorker;
        public RedirectToAmountPaymentPage()
        {
            InitializeComponent();
            var viewModel = new RedirectToAmountPaymentPageViewModel();
            viewModel.NavigateNextPage +=  delegate(string screenName, Dictionary<string, string> additionalInfo) { this.NavigationService.Navigate(new LoginUser()); };
            DataContext = viewModel;
            m_oWorker = new BackgroundWorker();
            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            m_oWorker.ProgressChanged += new ProgressChangedEventHandler(m_oWorker_ProgressChanged);
            m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_oWorker_RunWorkerCompleted);
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;

            ProgressBarPayment.Visibility = Visibility.Visible;
            lblProgress.Visibility = Visibility.Visible;
            statusBarPayment.Visibility = Visibility.Collapsed;
            //Start the async operation here
            m_oWorker.RunWorkerAsync();
        }

        /// <summary>
        /// On completed do the appropriate task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
            {
                lblProgress.Content = "Transaction Cancelled.";
            }
            else if (e.Error != null)
            {
                lblProgress.Content = "Error while performing background action.";
            }
            else
            {
                lblProgress.Content = "Payment Success";
            }
            statusBarPayment.Visibility = Visibility.Visible;
            System.Threading.Thread.Sleep(1000);
            if (AppState.Instance.IsUserLoggedIn)
                this.NavigationService.Navigate(new Dashboard());

        }

        /// <summary>
        /// Notification is performed here to the progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Here you play with the main UI thread
            ProgressBarPayment.Value = e.ProgressPercentage;

            lblProgress.Content = ProgressBarPayment.Value < 60 ? "Authentication is in Progress......" + ProgressBarPayment.Value.ToString() + "%" : "Payment is in Progress......" + ProgressBarPayment.Value.ToString() + "%";
        }

        /// <summary>
        /// Time consuming operations go here 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //time consuming operation
            for (int i = 0; i < 80; i++)
            {
                if (i < 60)
                    Thread.Sleep(50);
                m_oWorker.ReportProgress(i);

                //If cancel button was pressed while the execution is in progress
                //Change the state from cancellation ---> cancel'ed
                if (m_oWorker.CancellationPending)
                {
                    e.Cancel = true;
                    m_oWorker.ReportProgress(0);
                    return;
                }

            }
            //LicenseLogic logic = new LicenseLogic();
            //logic.ActivateSubscription();
            //Report 100% completion on operation completed
            m_oWorker.ReportProgress(100);
        }
    }
}
