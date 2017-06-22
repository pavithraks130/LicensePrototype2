using License.MetCalWeb.Common;
using License.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using License.ServiceInvoke;

namespace License.MetCalWeb.Controllers
{
    public class FileActionController : Controller
    {
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {

                    string link = "link file";
                    HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                    dynamic obj = new { urllink = link };
                    string jsonData = JsonConvert.SerializeObject(obj);
                    var response = client.PostAsJsonAsync("api/FileAction/UploadFile", jsonData).Result;
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("Create");
                    else
                    {
                        var jsondata = response.Content.ReadAsStringAsync().Result;
                        var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                        ModelState.AddModelError("", errorResponse.Message);
                    }
                    return View();
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please Upload Your file");
            }
            return View();
        }

    }

}
