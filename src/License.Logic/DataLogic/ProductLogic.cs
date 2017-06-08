using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using License.Logic.BusinessLogic;
namespace License.Logic.DataLogic
{
    /// <summary>
    /// History
    /// Created By:
    /// Created Date: 
    /// Purpose:    Update the Product details on the ONPremise if any changes made in the centralized DB for the Products.
    ///             1. Get the Product details from the ON Premise Json Files
    ///             2. Update the Changes to the respect product json file which is stored in the system
    /// </summary>
    public class ProductLogic : BaseLogic
    {
        /// <summary>
        /// Gets the Product List from the local based on the admin Id. 
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public List<Product> GetProductbyAdmin(string adminId)
        {
            var userSubscriptionList = Work.UserSubscriptionRepository.GetData(us => us.UserId == adminId).ToList();
            if (userSubscriptionList != null && userSubscriptionList.Count > 0)
            {
                var subscriptionIds = userSubscriptionList.Select(u => u.SubscriptionId);
                SubscriptionFileIO subscriptionLogic = new SubscriptionFileIO();
                var subList = subscriptionLogic.GetSubscriptionFromFile().Where(s => subscriptionIds.Contains(s.Id));
                List<Product> productList = new List<Product>();
                foreach (var sub in subList)
                    productList.AddRange(sub.Products);
                return productList.Distinct().ToList();
            }
            return null;
        }

        /// <summary>
        /// Updates the Product changes to the Local Product json file.
        /// </summary>
        /// <param name="products"></param>
        public void UpdateProducts(List<Product> products)
        {
            SubscriptionFileIO subLogic = new SubscriptionFileIO();
            foreach (var pro in products)
                subLogic.SaveProductToJson(pro);
        }

        
    }
}
