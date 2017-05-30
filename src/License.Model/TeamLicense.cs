using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class TeamLicense
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public int TeamId { get; set; }
        public int ProductId { get; set; }
        public bool IsMapped { get; set; }
        public virtual LicenseData License { get; set; }
    }

    public class TeamLicenseDetails
    {
        public Team Team { get; set; }

        public List<SubscriptionDetails> SubscriptionDetails { get; set; }
    }

    public class Products
    {
        public Product Product { get; set; }
        public int AvailableProductCount { get; set; }
        public bool IsSelected { get; set; }
    }

    public class DeleteTeamDetails
    {
        public int TeamId { get; set; }
        public List<int> productIdList { get; set; }
        public DeleteTeamDetails()
        {
            productIdList = new List<int>();
        }
    }
}
