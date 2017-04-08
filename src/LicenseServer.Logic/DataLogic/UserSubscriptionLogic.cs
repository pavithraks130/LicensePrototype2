using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class UserSubscriptionLogic : BaseLogic
    {
        public List<UserSubscription> GetUserSubscription(string userId)
        {
            List<UserSubscription> subscriptions = new List<UserSubscription>();
            var subscriptionList = Work.UserSubscriptionRepository.GetData(us => us.UserId == userId);
            foreach (var obj in subscriptionList)
                subscriptions.Add(AutoMapper.Mapper.Map<LicenseServer.Core.Model.UserSubscription, DataModel.UserSubscription>(obj));
            return subscriptions;
        }

        public Subscription CreateUserSubscription(UserSubscription subscription, int teamId)
        {
            Subscription purchasedSubscription = new Subscription();
            SubscriptionTypeLogic typeLogic = new SubscriptionTypeLogic();
            UserSubscription sub = null;
            SubscriptionDetailLogic detailsLogic = new SubscriptionDetailLogic();

            Core.Model.UserSubscription subs = AutoMapper.Mapper.Map<DataModel.UserSubscription, Core.Model.UserSubscription>(subscription);
            var obj = Work.UserSubscriptionRepository.Create(subs);
            Work.UserSubscriptionRepository.Save();

            if (obj != null)
            {

                sub = AutoMapper.Mapper.Map<Core.Model.UserSubscription, UserSubscription>(obj);
                purchasedSubscription.LicenseKeyProductMapping = GenerateLicenseKey(sub, teamId);
                purchasedSubscription.SubscriptionTypeId = obj.SubscriptionTypeId;
                purchasedSubscription.SubscriptionDate = obj.SubscriptionDate;
                purchasedSubscription.OrderdQuantity = obj.Quantity;
                purchasedSubscription.SubscriptionType = typeLogic.GetById(obj.SubscriptionTypeId);
                var details = detailsLogic.GetSubscriptionDetails(purchasedSubscription.SubscriptionTypeId);
                var proList = new List<Product>();
                foreach (var dt in details)
                {
                    var pro = dt.Product;
                    pro.Quantity = dt.Quantity;
                    proList.Add(pro);
                }
                purchasedSubscription.SubscriptionType.Products = proList;
            }
            return purchasedSubscription;
        }

        public SubscriptionList CreateUserSubscriptionList(List<UserSubscription> subsList, string userId)
        {
            UserLogic logic = new UserLogic();
            logic.UserManager = UserManager;
            logic.RoleManager = RoleManager;
            User userObj = logic.GetUserById(userId);

            SubscriptionList userSubscriptionList = new SubscriptionList();

            userSubscriptionList.UserId = userId;
            int teamId = userObj.OrganizationId;
            foreach (var subObj in subsList)
            {
                var obj = CreateUserSubscription(subObj, teamId);
                userSubscriptionList.Subscriptions.Add(obj);
            }
            return userSubscriptionList;
        }

        public List<LicenseKeyProductMapping> GenerateLicenseKey(UserSubscription subs, int teamId)
        {
            SubscriptionDetailLogic detailLogic = new SubscriptionDetailLogic();
            ProductLogic produLogic = new ProductLogic();
            SubscriptionTypeLogic logic = new SubscriptionTypeLogic();
            SubscriptionType type = logic.GetById(subs.SubscriptionTypeId);
            List<SubscriptionDetails> details = detailLogic.GetSubscriptionDetails(subs.SubscriptionTypeId);
            int qty = subs.Quantity;
            List<LicenseKeyProductMapping> licProductMapping = new List<LicenseKeyProductMapping>();
            List<string> chuksum = new List<string>();

            foreach (var data in details)
            {
                var pro = produLogic.GetProductById(data.ProductId);
                var count = data.Quantity;
                string keyData = pro.ProductCode + "^" + DateTime.Now.AddDays(type.ActiveDays).Date.ToString() + "^" + type.Id + "^" + teamId;
                for (int i = 0; i < (data.Quantity * qty); i++)
                {
                    var key = LicenseKey.LicenseKeyGen.CryptoEngine.Encrypt(keyData, true);
                    string key1 = string.Empty;
                    do
                    {
                        var keygeneration = new LicenseKey.GenerateKey();
                        keygeneration.LicenseTemplate = "xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";
                        keygeneration.UseBase10 = false;
                        keygeneration.UseBytes = false;
                        keygeneration.CreateKey();
                        key1 = keygeneration.GetLicenseKey();
                    } while (chuksum.Contains(key1));
                    var licKey = key + "-" + key1;
                    licProductMapping.Add(new LicenseKeyProductMapping() { LicenseKey = licKey, ProductId = pro.Id });
                }
            }
            return licProductMapping;
        }
    }
}
