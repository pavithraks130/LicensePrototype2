using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace License.MetCalWeb.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/Notification/GetAllNotification/").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var notificationList = JsonConvert.DeserializeObject<List<Notification>>(jsondata);
                return View(notificationList);
            }
            else
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                ModelState.AddModelError("", errorResponse.Message);
            }
            client.Dispose();
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
            // TODO: Add insert logic here

            if (notification != null)
            {
                //Service call to save the data in Centralized DB
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                var response = client.PostAsJsonAsync("api/Notification/Create", notification).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Create");
                else
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                    ModelState.AddModelError("", errorResponse.Message);
                }
            }

            return RedirectToAction("Create");

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
                // TODO: Add update logic here

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
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
