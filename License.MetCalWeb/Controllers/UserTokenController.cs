using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace License.MetCalWeb.Controllers
{
    public class UserTokenController : BaseController
    {
        UserTokenLogic tokenLogic = null;

        public UserTokenController()
        {
            tokenLogic = new UserTokenLogic();
        }
        // GET: UserToken
        public ActionResult Index()
        {
            var tokenList = tokenLogic.GetUsertokenList();
            return View(tokenList);
        }

        public ActionResult CreateToken()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateToken(UserToken token)
        {
            if (ModelState.IsValid)
            {
                LicenseKey.GenerateKey keyGen = new LicenseKey.GenerateKey();
                keyGen.LicenseTemplate = "xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";
                keyGen.UseBase10 = false;
                keyGen.UseBytes = false;
                keyGen.CreateKey();
                token.Token = keyGen.GetLicenseKey();
                tokenLogic.CreateUserToken(token);
                string subject = string.Empty;
                string body = string.Empty;

                subject = "Admin Invite to Fluke Calibration";

                body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/RegistrationToken.html"));
                body = body.Replace("{{UserToken}}", token.Token);
                body = body.Replace("{{RegisterUrl}}", String.Concat(Request.Url.ToString().Replace(Request.Url.AbsolutePath, ""), Url.Action("Register", "Account")));
                Common.EmailService emailService = new Common.EmailService();

                emailService.SendEmail(token.Email, subject, body);

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}