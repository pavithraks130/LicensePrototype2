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
    public class SubscriptionBO
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
                // To remove the Products duplication in  both subscription and indivdual file.             
                foreach (var pro in sub.Products)
                    SaveProductToJson(pro);
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
            if (Common.CommonFileIO.IsFileExist(proDtl.ProductCode + ".txt"))
                Common.CommonFileIO.DeleteFile(proDtl.ProductCode + ".txt");
            var data = JsonConvert.SerializeObject(proDtl);
            Common.CommonFileIO.SaveDatatoFile(data, proDtl.ProductCode + ".txt");
        }

        public Product GetProductFromJsonFile(string productCode)
        {
            var fileName = productCode + ".txt";
            if (Common.CommonFileIO.IsFileExist(fileName))
            {
                var prodJsondata = Common.CommonFileIO.GetJsonDataFromFile(fileName);
                var obj = JsonConvert.DeserializeObject<Product>(prodJsondata);
                return obj;
            }
            else
                return null;
        }

        public List<SubscriptionType> GetSubscriptionFromFile()
        {
            List<SubscriptionType> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("SubscriptionDetails.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("SubscriptionDetails.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<SubscriptionType>>(existingData);

                foreach (var sub in subscriptionList)
                {
                    if (sub.Products == null)
                        sub.Products = new List<Product>();
                    foreach (var obj in sub.ProductIdList)
                    {
                        var pro = GetProductFromJsonFile(obj.ProductCode.ToString());
                        sub.Products.Add(pro);
                    }
                }
            }
            else
                subscriptionList = new List<SubscriptionType>();
            Common.CommonFileIO.DeleteTempFolder();
            return subscriptionList;

        }


    }
}
