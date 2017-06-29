using License.MetCalWeb.Common;
using License.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using License.ServiceInvoke;

namespace License.MetCalWeb.Controllers
{
    public class NotificationController : Controller
    {
        private APIInvoke _invoke = null;
        public NotificationController()
        {
            _invoke = new APIInvoke();
        }
        // GET: Notification
        public ActionResult Index()
        {
            List<Notification> notifications = new List<Notification>();
            WebAPIRequest<List<Notification>> request = new WebAPIRequest<List<Notification>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.All,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Notification,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<List<Notification>, List<Notification>>(request);
            if (response.Status)
            { 
                notifications = response.ResponseData;
                return View(notifications);
            }
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.GetAsync("api/Notification/GetAllNotification").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsondata = response.Content.ReadAsStringAsync().Result;
            //    var notificationList = JsonConvert.DeserializeObject<List<Notification>>(jsondata);
            //    return View(notificationList);
            //}
            //else
            //{
            //    var jsondata = response.Content.ReadAsStringAsync().Result;
            //    var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
            //    ModelState.AddModelError("", errorResponse.Message);
            //}
            //client.Dispose();
            return View();

        }

        // GET: Notification/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Notification/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notification/Create
        [HttpPost]
        public ActionResult Create(Notification notification)
        {
            if (ModelState.IsValid)
            {
                WebAPIRequest<Notification> request = new WebAPIRequest<Notification>()
                {
                    AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                    Functionality = Functionality.Create,
                    InvokeMethod = Method.POST,
                    ServiceModule = Modules.Notification,
                    ServiceType = ServiceType.CentralizeWebApi,
                    ModelObject = notification

                };
                var response = _invoke.InvokeService< Notification, Notification>(request);
                if (response.Status)
                    return RedirectToAction("Create");
                else
                    ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
                ////Service call to save the data in Centralized DB
                //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                //var response = client.PostAsJsonAsync("api/Notification/Create", notification).Result;
                //if (response.IsSuccessStatusCode)
                //    return RedirectToAction("Create");
                //else
                //{
                //    var jsondata = response.Content.ReadAsStringAsync().Result;
                //    var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                //    ModelState.AddModelError("", errorResponse.Message);
                //}
            }

            return View(notification);

        }

        // GET: Notification/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Notification/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Notification/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Notification/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
