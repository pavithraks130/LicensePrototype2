using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using System.IO;
using Newtonsoft.Json;

namespace License.Logic.BusinessLogic
{
    /// <summary>
    /// History:
    ///     Created By : 
    ///     Created date :
    ///     Purpose: 1. Logic for reading the data from Json file.
    ///              2. Saving the Subscription and product to the Json file.
    /// </summary>
    public class SubscriptionBO
    {

        public void SaveToFile(List<Subscription> subscriptions)
        {

            List<Subscription> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("SubscriptionDetails.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("SubscriptionDetails.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<Subscription>>(existingData);
            }
            else
                subscriptionList = new List<Subscription>();
            bool isDataModified = false;
            foreach (var sub in subscriptions)
            {
                // To remove the Products duplication in  both subscription and indivdual file.             
                foreach (var pro in sub.Products)
                {
                    pro.IsLocal = true;
                    SaveProductToJson(pro);
                }
                sub.Products = null;

                if (!subscriptionList.Any(s => s.Id == sub.Id))
                {
                    isDataModified = true;
                    subscriptionList.Add(sub);
                }
            }
            if (isDataModified)
            {
                var data = JsonConvert.SerializeObject(subscriptionList);
                Common.CommonFileIO.SaveDatatoFile(data, "SubscriptionDetails.txt");
                Common.CommonFileIO.DeleteTempFolder();
            }
        }

        public void SaveProductToJson(Product proDtl)
        {
            if (Common.CommonFileIO.IsFileExist(proDtl.Id + ".txt"))
                Common.CommonFileIO.DeleteFile(proDtl.Id + ".txt");
            var data = JsonConvert.SerializeObject(proDtl);
            Common.CommonFileIO.SaveDatatoFile(data, proDtl.Id + ".txt");
        }

        public Product GetProductFromJsonFile(int proId)
        {
            var fileName = proId + ".txt";
            if (Common.CommonFileIO.IsFileExist(fileName))
            {
                var prodJsondata = Common.CommonFileIO.GetJsonDataFromFile(fileName);
                var obj = JsonConvert.DeserializeObject<Product>(prodJsondata);
                return obj;
            }
            else
                return null;
        }

        public List<Subscription> GetSubscriptionFromFile()
        {
            List<Subscription> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("SubscriptionDetails.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("SubscriptionDetails.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<Subscription>>(existingData);

                foreach (var sub in subscriptionList)
                {
                    if (sub.Products == null)
                        sub.Products = new List<Product>();
                    foreach (var obj in sub.ProductIdList)
                    {
                        var pro = GetProductFromJsonFile(Convert.ToInt32(obj.Id));
                        sub.Products.Add(pro);
                    }
                }
            }
            else
                subscriptionList = new List<Subscription>();
            Common.CommonFileIO.DeleteTempFolder();
            return subscriptionList;

        }


    }
}
