using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace License.MetCalDesktop.ViewModel
{
   public class RedirectToAmountPaymentPageViewModel:BaseEntity
    {
        /// <summary>
        /// BackToLogin action
        /// </summary>
        public ICommand BackToLoginCommand
        { get; set; }

        /// <summary>
        ///RedirectToAmountPaymentPageViewModel initialisation
        /// </summary>
        public RedirectToAmountPaymentPageViewModel()
        {
            BackToLoginCommand = new RelayCommand(BackToLoginPageCommandExecuted);
        }

        /// <summary>
        /// redirect to loginPage 
        /// </summary>
        /// <param name="parameter">parameter</param>
        private void BackToLoginPageCommandExecuted(object parameter)
        {
            NavigateNextPage?.Invoke(null, null);
        }
    }
}
