using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using System.IO;
using Newtonsoft.Json;

namespace License.Logic.DataLogic
{
    public class ProductSubscriptionLogic
    {

        public void SaveToFile(List<SubscriptionType> subscriptions)
        {

            List<SubscriptionType> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("SubscriptionDetails.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("SubscriptionDetails.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<SubscriptionType>>(existingData);
            }
            else
                subscriptionList = new List<SubscriptionType>();
            bool isDataModified = false;
            foreach (var sub in subscriptions)
            {
                if (!subscriptionList.Any(s => s.Id == sub.Id))
                {
                    isDataModified = true;
                    subscriptionList.Add(sub);
                }
            }
            if (isDataModified)
            {
                var data = JsonConvert.SerializeObject(subscriptionList);
                Common.CommonFileIO.SaveDatatoFile(data, "productSubscription.txt");
            }
        }

        public List<SubscriptionType> GetSubscriptionFromFile()
        {
            List<SubscriptionType> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("SubscriptionDetails.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("SubscriptionDetails.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<SubscriptionType>>(existingData);
            }
            else
                subscriptionList = new List<SubscriptionType>();
            return subscriptionList;

        }

        
    }
}
