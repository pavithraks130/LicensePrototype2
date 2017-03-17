using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.MetCalDesktop.Common;
using License.Model;
namespace License.MetCalDesktop.ViewModel
{
    public class DashboardViewModel : BaseEntity
    {
        public List<Feature> FeataureList { get; set; }

        public DashboardViewModel()
        {
            FeataureList = new List<Feature>();
            LoadFeatures();
        }
        public void LoadFeatures()
        {
            if (AppState.Instance.UserLicenseList != null)
            {
                foreach (var data in AppState.Instance.UserLicenseList)
                {
                    foreach (var pro in data.ProductList)
                    {
                        foreach (var fet in pro.Features)
                        {
                            FeataureList.Add(fet);
                        }
                    }
                }
            }
        }

    }
}
