using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class UserTokenController : BaseController
    {
      
        public UserTokenController()
        {
           
        }
        // GET: UserToken
        public async Task<ActionResult> Index()
        {
            List<UserToken> tokenList = new List<UserToken>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.GetAsync("api/usertoken/All");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                tokenList = JsonConvert.DeserializeObject<List<UserToken>>(data);
            }
            return View(tokenList);
        }

        public ActionResult CreateToken()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateToken(UserToken token)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                var response = await client.PostAsJsonAsync<UserToken>("api/usertoken/create", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var tokenObj = JsonConvert.DeserializeObject<UserToken>(data);
                    string subject = string.Empty;
                    string body = string.Empty;

                    subject = "Admin Invite to Fluke Calibration";

                    body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/RegistrationToken.html"));
                    body = body.Replace("{{UserToken}}", tokenObj.Token);
                    body = body.Replace("{{RegisterUrl}}", String.Concat(Request.Url.ToString().Replace(Request.Url.AbsolutePath, ""), Url.Action("Register", "Account")));
                    Common.EmailService emailService = new Common.EmailService();
                    emailService.SendEmail(token.Email, subject, body);
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("", response.ReasonPhrase);               
            }
            return View();
        }
    }
}