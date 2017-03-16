using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
