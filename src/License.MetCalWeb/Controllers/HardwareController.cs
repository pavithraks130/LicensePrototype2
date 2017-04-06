using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Models;
using System.Net.Http;
using License.MetCalWeb.Common;
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    public class HardwareController : BaseController
    {
        public ActionResult HardwareContainer()
        {
            HardwareModel model = LoadHardware();
            return View(model);
        }

        private HardwareModel LoadHardware()
        {
            var hm = new HardwareModel();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var resposne = client.GetAsync("api/asset/GetAll").Result;
            if (resposne.IsSuccessStatusCode)
            {
                var jsonData = resposne.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    hm.Assets = JsonConvert.DeserializeObject<List<TeamAsset>>(jsonData);
            }
            return hm;
        }

        public ActionResult EditHardware(int id)
        {
            TeamAsset asset = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var resposne = client.GetAsync("api/asset/GetAssetById/" + id.ToString()).Result;
            if (resposne.IsSuccessStatusCode)
            {
                var jsonData = resposne.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    asset = JsonConvert.DeserializeObject<TeamAsset>(jsonData);
            }
            return PartialView(asset);
        }
        public ActionResult AssetConfiguration(int id, string actionType)
        {
            switch (actionType)
            {
                case "Remove":
                    HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                    var resposne = client.DeleteAsync("api/asset/DeleteAsset/" + id.ToString()).Result;
                    if (resposne.IsSuccessStatusCode)
                    {
                        return RedirectToAction("HardwareContainer");
                    }
                    break;
            }
            return RedirectToAction("HardwareContainer");
        }

        public ActionResult AddHardware()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditHardware(TeamAsset assetModel)
        {
            TeamAsset asset = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var resposne = client.PostAsJsonAsync("api/asset/CreateAsset",assetModel).Result;
            if (resposne.IsSuccessStatusCode)
            {
                var jsonData = resposne.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    asset = JsonConvert.DeserializeObject<TeamAsset>(jsonData);
            }
            return RedirectToAction("HardwareContainer");
        }
    }
}
