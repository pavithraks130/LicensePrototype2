using License.MetCalDesktop.Common;
using License.MetCalDesktop.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace License.MetCalDesktop.Views
{
    /// <summary>
    /// Interaction logic for VISMAData.xaml
    /// </summary>
    public partial class VISMAData : UserControl
    {
        public VISMAData()
        {
            InitializeComponent();
            var viewmodel = new VISMADataViewModel();
            DataContext = viewmodel;
        }
        /// <summary>
        /// Text box water mark text handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_MouseEnter(object sender, MouseEventArgs e)
        {
            if (txtUserName.Text == Constants.SEARCHDATA)
            {
                txtUserName.Text = "";
            }
        }

        /// <summary>
        /// Text box water mark text handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_MouseLeave(object sender, MouseEventArgs e)
        {
            if (txtUserName.Text ==string.Empty)
            {
                txtUserName.Text = Constants.SEARCHDATA;
            }
        }
    }
}
