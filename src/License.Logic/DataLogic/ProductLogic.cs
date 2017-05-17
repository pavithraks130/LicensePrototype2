using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using License.Logic.BusinessLogic;
namespace License.Logic.DataLogic
{
    public class ProductLogic : BaseLogic
    {
        public List<Product> GetProductbyAdmin(string adminId)
        {
            var userSubscriptionList = Work.UserSubscriptionRepository.GetData(us => us.UserId == adminId).ToList();
            if (userSubscriptionList != null && userSubscriptionList.Count > 0)
            {
                var subscriptionIds = userSubscriptionList.Select(u => u.SubscriptionId);
                SubscriptionBO subscriptionLogic = new SubscriptionBO();
                var subList = subscriptionLogic.GetSubscriptionFromFile().Where(s => subscriptionIds.Contains(s.Id));
                List<Product> productList = new List<Product>();
                foreach (var sub in subList)
                    productList.AddRange(sub.Products);
                return productList.Distinct().ToList();
            }
            return null;
        }

        public void UpdateProducts(List<Product> products)
        {
            SubscriptionBO subLogic = new SubscriptionBO();
            foreach (var pro in products)
                subLogic.SaveProductToJson(pro);
        }
    }
}
