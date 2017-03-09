using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;
using System.IO;
using Newtonsoft.Json;

namespace License.Logic.ServiceLogic
{
    public class ProductSubscriptionLogic
    {
        public void SaveToFile(Subscription subs)
        {
            List<Subscription> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("productSubscription.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("productSubscription.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<Subscription>>(existingData);
            }
            else
                subscriptionList = new List<Subscription>();
            if (!subscriptionList.Any(s => s.Id == subs.Id))
            {
                subscriptionList.Add(subs);
                var data = JsonConvert.SerializeObject(subscriptionList);
                Common.CommonFileIO.SaveDatatoFile(data, "productSubscription.txt");
            }
        }

        public List<Subscription> GetSubscriptionFromFile()
        {
            List<Subscription> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("productSubscription.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("productSubscription.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<Subscription>>(existingData);
            }
            else
                subscriptionList = new List<Subscription>();
            return subscriptionList;

        }
    }
}
