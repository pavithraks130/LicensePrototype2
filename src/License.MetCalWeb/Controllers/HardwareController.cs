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
    /// <summary>
    /// cOntroller for handling the Team/Hardware assets.
    /// </summary>
    [Authorize]
    [SessionExpire]
    public class HardwareController : BaseController
    {
        /// <summary>
        /// Get Action to list the All the Assets.
        /// </summary>
        /// <returns></returns>
        public ActionResult HardwareContainer()
        {
            HardwareDetails model = LoadHardware();
            return View(model);
        }

        /// <summary>
        /// Service call to  get the List of assets which are created
        /// </summary>
        /// <returns></returns>
        private HardwareDetails LoadHardware()
        {
            var hm = new HardwareDetails();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
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

        /// <summary>
        /// Get Method to display the Edit view with the existing data for the selected asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditHardware(int id)
        {
            TeamAsset asset = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
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

        /// <summary>
        /// Post action to Updating the modified data to the asset for the selected Asset Id.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHardware(TeamAsset asset)
        {
            if (ModelState.IsValid)
            {

                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
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
            var _message = string.Join(Environment.NewLine, ModelState.Values
                                      .SelectMany(x => x.Errors)
                                      .Select(x => x.ErrorMessage));
            return Json(new { success = false, message = _message });
        }

        /// <summary>
        /// Get actopn to perform action based on the action type for the selectd asset id. This is used as the Generic function which can used for performing anay action
        /// for the selected asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public ActionResult AssetConfiguration(int id, string actionType)
        {
            switch (actionType)
            {
                case "Remove":
                    HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
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

        /// <summary>
        /// Get action to display the View to create new Asset
        /// </summary>
        /// <returns></returns>
        public ActionResult AddHardware()
        {
            return View();
        }

        /// <summary>
        /// Post Action to create the new asset record to DB by making a service call. The return data will be JSOn result to this action where the repose can handled in the Script
        ///  and display error if any error exist.
        /// </summary>
        /// <param name="assetModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddHardware(TeamAsset assetModel)
        {
            if (ModelState.IsValid)
            {
                TeamAsset asset = null;
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                var response = client.PostAsJsonAsync("api/asset/CreateAsset", assetModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    if (!String.IsNullOrEmpty(jsonData))
                        asset = JsonConvert.DeserializeObject<TeamAsset>(jsonData);
                    return RedirectToAction("HardwareContainer");
                }
                else
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
                }
            }
            var _message = string.Join(Environment.NewLine, ModelState.Values
                                      .SelectMany(x => x.Errors)
                                      .Select(x => x.ErrorMessage));
            return Json(new { success = false, message = _message });

        }
    }
}
