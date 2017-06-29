using License.MetCalWeb.Common;
using License.ServiceInvoke;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using License.Models;

namespace License.MetCalWeb.Controllers
{
    public class VISMADataController : Controller
    {

        private APIInvoke _invoke = null;
        private object jsonData;

        public VISMADataController()
        {
            _invoke = new APIInvoke();
        }

        /// <summary>
        /// Display User interaction View 
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// Used to upload VISMA data into database
        /// </summary>
        /// <param name="link"></param>
        /// <param name="delimter"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(string link, string delimter)
        {
            URLData data = new URLData();
            data.Delimiter = delimter;
            data.Url = link;
            TempData["msg"] = "";

            WebAPIRequest<URLData> request = new WebAPIRequest<URLData>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.UploadFile,
                InvokeMethod = Method.POST,
                ServiceModule = Modules.VISMAData,
                ModelObject = data,
                ServiceType = ServiceType.OnPremiseWebApi
            };

            var response = _invoke.InvokeService<URLData, String>(request);
            if (response.Status)
                TempData["msg"] = "Successfully uploaded file";
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.PostAsJsonAsync("api/VISMAData/UploadFile", data).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    TempData["msg"] = "Successfully uploaded file";
            //    return View();
            //}
            //else
            //{
            //    TempData["msg"] = "";
            //    var jsondata = response.Content.ReadAsStringAsync().Result;
            //    var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
            //    ModelState.AddModelError("", errorResponse.Message);
            //}
            return View();
        }
    }
}

