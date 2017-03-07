using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb;
using System.Web.Mvc;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void RegistrationVerification()
        {
            Controllers.AccountController accCtrl = new Controllers.AccountController();
            Models.LoginViewModel viewModel = new Models.LoginViewModel();
            viewModel.Email = "apsarj@gmail.com";
            viewModel.Password = "Test@1234";
            var obj = accCtrl.LogIn(viewModel) as ViewResult;
            Assert.AreEqual("Home", obj.MasterName);
        }

    }
}
