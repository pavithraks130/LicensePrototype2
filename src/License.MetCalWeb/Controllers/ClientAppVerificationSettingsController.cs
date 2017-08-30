using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.ServiceInvoke;
using License.Models;

namespace License.MetCalWeb.Controllers
{
    [Authorize(Roles = "Fluke Admin")]
    public class ClientAppVerificationSettingsController : BaseController
    {

        private APIInvoke invoke;

        public ClientAppVerificationSettingsController()
        {
            invoke = new APIInvoke();
        }

        // GET: ClientAppVerificationSettings
        public ActionResult Index()
        {
            List<ClientAppVerificationSettings> settings = new List<ClientAppVerificationSettings>();

            WebAPIRequest<List<ClientAppVerificationSettings>> request = new WebAPIRequest<List<ClientAppVerificationSettings>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.All,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.ClientAppVerificationSettings,
                ServiceType = ServiceType.CentralizeWebApi
            };

            var response = invoke.InvokeService<List<ClientAppVerificationSettings>, List<ClientAppVerificationSettings>>(request);
            if (response.Status)
                settings = response.ResponseData;
            return View(settings);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(ClientAppVerificationSettings setting)
        {
            if (ModelState.IsValid)
            {
                WebAPIResponse<ClientAppVerificationSettings> response = invoke.InvokeService<ClientAppVerificationSettings, ClientAppVerificationSettings>(new WebAPIRequest<ClientAppVerificationSettings>()
                {
                    AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                    Functionality = Functionality.Create,
                    InvokeMethod = Method.POST,
                    ModelObject = setting,
                    ServiceModule = Modules.ClientAppVerificationSettings,
                    ServiceType = ServiceType.CentralizeWebApi
                });
                if (!response.Status)
                {
                    string errorMessage = response.Error.Message;
                    return Json(new { success = false, message = errorMessage });
                }
                return Json(new { success = true, message = "" });
            }
            var _message = string.Join(Environment.NewLine, ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
            return Json(new { success = false, message = _message });
        }

        [AllowAnonymous]
        public ActionResult Sync()
        {
            var response = invoke.InvokeService<List<ClientAppVerificationSettings>, List<ClientAppVerificationSettings>>(new WebAPIRequest<List<ClientAppVerificationSettings>>()
            {
                Functionality = Functionality.All,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.ClientAppVerificationSettings,
                ServiceType = ServiceType.CentralizeWebApi
            });
            if (response.Status)
            {
                var settingsList = response.ResponseData;
                response = invoke.InvokeService<List<ClientAppVerificationSettings>, List<ClientAppVerificationSettings>>(new WebAPIRequest<List<ClientAppVerificationSettings>>()
                {
                    Functionality = Functionality.Sync,
                    InvokeMethod = Method.POST,
                    ServiceModule = Modules.ClientAppVerificationSettings,
                    ServiceType = ServiceType.OnPremiseWebApi,
                    ModelObject = settingsList
                });
                if (response.Status)
                    ViewData["SyncResponseMessage"] = "Synced Successfully";
                else
                    ViewData["SyncResponseMessage"] = response.Error.Message;
            }
            return View();
        }


    }
}