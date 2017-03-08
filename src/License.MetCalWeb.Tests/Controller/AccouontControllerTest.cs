using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.MetCalWeb;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class AccouontControllerTest
    {
        [TestMethod]
        public void LoginTest()
        {
            Controllers.AccountController accCtrl = new Controllers.AccountController();
            Models.LoginViewModel model = new Models.LoginViewModel();
            model.Email = "apsarj@gmail.com";
            model.Password = "Test@1234";
            var obj = accCtrl.LogIn(model) as ViewResult;
            Assert.AreEqual("Home", obj.ViewName);
        }
    }
}
