using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
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

namespace License.MetCalWeb.Controllers
{
    public class VISMADataController : Controller
    {
        private object jsonData;

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(string link,string delimter)
        {
            URLData data = new URLData();
            data.Delimiter = delimter;
            data.Url = link;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //dynamic obj = new { urllink = link };
            //string jsonData = JsonConvert.SerializeObject(obj);
            var response = client.PostAsJsonAsync("api/VISMAData/UploadFile", data).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["msg"] = "Successfully uploaded file";
                return View();
            }
            else
            {
                TempData["msg"] = "";
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                ModelState.AddModelError("", errorResponse.Message);
            }
            return View();
        }
    }
}

