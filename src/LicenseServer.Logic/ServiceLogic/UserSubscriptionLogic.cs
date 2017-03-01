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
            {
                subscriptions.Add(AutoMapper.Mapper.Map<LicenseServer.Core.Model.UserSubscription, DataModel.UserSubscription>(obj));
            }

            return subscriptions;
        }

        public UserSubscription CreateUserSubscription(UserSubscription subscription)
        {
            UserSubscription sub = null;
            Core.Model.UserSubscription subs = AutoMapper.Mapper.Map<DataModel.UserSubscription, Core.Model.UserSubscription>(subscription);
            var obj = Work.UserSubscriptionRepository.Create(subs);
            Work.UserSubscriptionRepository.Save();
            if (obj != null)
            {
                sub = AutoMapper.Mapper.Map<Core.Model.UserSubscription, UserSubscription>(obj);
                sub.LicenseKeys = new List<string>();
                GenerateLicenseKey(sub);
            }
            return sub;
        }

        public void GenerateLicenseKey(UserSubscription subs)
        {
            SubscriptionDetailLogic detailLogic = new SubscriptionDetailLogic();
            ProductLogic produLogic = new ProductLogic();
            SubscriptionTypeLogic logic = new SubscriptionTypeLogic();
            SubscriptionType type = logic.GetById(subs.SubscriptionTypeId);
            List<SubscriptionDetails> details = detailLogic.GetSubscriptionDetails(subs.SubscriptionTypeId);
            int qty = subs.Qty;
            List<string> chuksum = new List<string>();

            foreach (var data in details)
            {
                var pro = produLogic.GetProductById(data.ProductId);
                var count = data.Quantity;
                string keyData = pro.ProductCode + "/" + (data.Quantity * subs.Qty).ToString() + "/" + DateTime.Now.AddDays(type.ActiveDays).Date.ToString() + "/" + type.Id;
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
                subs.LicenseKeys.Add(licKey);
            }

        }
    }


}
