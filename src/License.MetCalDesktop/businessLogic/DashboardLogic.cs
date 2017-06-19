using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using License.MetCalDesktop.Model;
using License.MetCalDesktop.Common;


namespace License.MetCalDesktop.businessLogic
{
    public class DashboardLogic
    {
        private string featurefileName = "Products.txt";

        public List<Product> LoadFeatureOffline()
        {
            if (FileIO.IsFileExist(featurefileName))
            {
                var featuresList = FileIO.GetJsonDataFromFile(featurefileName);
                var licenseDetails = JsonConvert.DeserializeObject<List<Product>>(featuresList);
                return licenseDetails;
            }
            return null;
        }

       

    }
}
