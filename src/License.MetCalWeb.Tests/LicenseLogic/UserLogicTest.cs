using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.Logic.ServiceLogic;
using License.Core.Manager;
using Microsoft.AspNet.Identity.EntityFramework;
using License.Core.Model;
using License.Core.DBContext;

namespace License.MetCalWeb.Tests.LicenseLogic
{
    [TestClass]
    public class UserLogicTest
    {
        UserLogic logic = null;

        Moq.Mock<AppUserManager> manager;
        Moq.Mock<AppRoleManager> rolemanager;

        public UserLogicTest()
        {
            InitializerClass.Initialize();
            logic = new UserLogic();
           
        }

        [TestMethod]
        public void GetUser()
        {
            var users = logic.GetUsers();
            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]        
        public void CrearUser()
        {
            Model.Registration reg = new Model.Registration();
            reg.FirstName = "veeresh";
            reg.LastName = "S";
            reg.OrganizationName = "fluke";
            reg.Email = "pavithra.shivarudrappa@fluke.com";
            reg.Password = "Test@1234";
            reg.PhoneNumber = "1234567890";
            var licserUserLogic = new LicenseServer.Logic.UserLogic();
            var obj = licserUserLogic.GetUserByEmail(reg.Email);
            if (obj != null)
            {
                reg.ServerUserId = obj.UserId;
                var result = logic.CreateUser(reg);
                Assert.IsTrue(result);
            }
            else
                Assert.Fail("Register user is not exist in License Server. WHich tracks the admin users");

        }


    }
}
