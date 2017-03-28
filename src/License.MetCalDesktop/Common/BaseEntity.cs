
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace License.MetCalDesktop.Common
{
   public delegate void NavigateNextPage(string screenName, Dictionary<string, string> additionalInfo);

    /// <summary>
    /// This calss is used to perform a comman action in all other viewmodel class.
    /// </summary>
    public class BaseEntity : INotifyPropertyChanged
    {
        /// <summary>
        /// This is used for page navigation.
        /// </summary>
        public NavigateNextPage NavigateNextPage;

        /// <summary>
        /// This will update UI fields
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On PropertyChanged event 
        /// </summary>
        /// <param name="propertyName">propertyName</param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {

        private readonly Action<object> _action;

        /// <summary>
        /// used to perform button click action.
        /// </summary>
        /// <param name="action">action</param>
        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        /// <summary>
        /// CanExecuteChanged event handler
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// This method is used to make the button enable or disable.
        /// </summary>
        /// <param name="parameter">input parameter</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// To execute any button click action
        /// </summary>
        /// <param name="parameter">input parameter</param>
        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }
    }
}
