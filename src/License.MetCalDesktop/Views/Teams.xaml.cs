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
using System.Windows.Shapes;

namespace License.MetCalDesktop.Views
{
    /// <summary>
    /// Interaction logic for Teams.xaml
    /// </summary>
    public partial class Teams : Window
    {
        public EventHandler ClosePopupWindow;
        public Teams()
        {
            InitializeComponent();
            var dataModel = new ViewModel.TeamViewModel();
            dataModel.ClosepoupWindow += CloseCurrentWindow;
            this.DataContext = dataModel;
        }

        public void CloseCurrentWindow(Object src, EventArgs arg)
        {
            if (ClosePopupWindow != null)
                ClosePopupWindow(this, new EventArgs());
        }
    }
}
