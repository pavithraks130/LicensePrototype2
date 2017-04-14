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
    [Authorize]
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
            var response = client.GetAsync("api/asset/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    hm.Assets = JsonConvert.DeserializeObject<List<TeamAsset>>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return hm;
        }

        public ActionResult EditHardware(int id)
        {
            TeamAsset asset = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/asset/GetAssetById/" + id.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    asset = JsonConvert.DeserializeObject<TeamAsset>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHardware(TeamAsset asset)
        {
            if (ModelState.IsValid)
            {

                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                var response = client.PutAsJsonAsync("api/asset/UpdateAsset/" + asset.Id, asset).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("HardwareContainer");
                else
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
                }
            }
            return View(asset);
        }
        public ActionResult AssetConfiguration(int id, string actionType)
        {
            switch (actionType)
            {
                case "Remove":
                    HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                    var response = client.DeleteAsync("api/asset/DeleteAsset/" + id.ToString()).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("HardwareContainer");
                    }
                    else
                    {
                        var jsonData = response.Content.ReadAsStringAsync().Result;
                        var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                        ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
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
            var response = client.PostAsJsonAsync("api/asset/CreateAsset", assetModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    asset = JsonConvert.DeserializeObject<TeamAsset>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return RedirectToAction("HardwareContainer");
        }
    }
}
