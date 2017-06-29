using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using License.Models;
using License.MetCalDesktop.Common;
using License.ServiceInvoke;

namespace License.MetCalDesktop.businessLogic
{
    public class DashboardLogic
    {
        private string featurefileName = "Products.txt";
        private FileIO _fileIO = null;

        public DashboardLogic()
        {
            _fileIO = new FileIO();
        }
        public List<Product> LoadFeatureOffline()
        {
            var productList = _fileIO.GetDataFromFile<List<Product>>(featurefileName);
            return productList;
            //if (FileIO.IsFileExist(featurefileName))
            //{
            //    var featuresList = FileIO.GetJsonDataFromFile(featurefileName);
            //    var licenseDetails = JsonConvert.DeserializeObject<List<Product>>(featuresList);
            //    return licenseDetails;
            //}
            //return null;
        }



    }
}
