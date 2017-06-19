using LicenseServer.DataModel;
using LicenseServer.Logic.DataLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Centralized.WebAPI.Controllers
{
    /// <summary>
    /// Controller for Notification Related Operations
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Notification")]
    public class NotificationController:BaseController
    {
        NotificationLogic notificationLogic = null;
        public NotificationController()
        {
            notificationLogic = new NotificationLogic();
        }


        /// <summary>
        /// POST Method. Create Notification Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateNotification(Notification item)
        {
            var obj = notificationLogic.CreateNotificationItem(item);
            if (obj != null && obj.Id > 0)
                return Request.CreateResponse(HttpStatusCode.Created, obj);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Failed to create Notifcation object due to internal service error");
        }

    }
}