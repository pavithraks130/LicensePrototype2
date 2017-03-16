using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace License.MetCalWeb.Tests.LicenseServerLogic
{
    [TestClass]
    public class UserLogicTest
    {
        UserLogic logic;
        public UserLogicTest()
        {
            logic = new UserLogic();
            InitializerClass.Initialize();
        }

        [TestMethod]
        public void CreateLicenseServerUser()
        {
            Registration reg = new Registration();
            reg.FirstName = "veeresh";
            reg.LastName = "S";
            reg.OrganizationName = "sidssol";
            reg.Email = "veereshrdrpp@gmail.com";
            reg.Password = "Test@1234";
            reg.PhoneNumber = "1234567890";
            string id = logic.CreateUser(reg);
            Assert.IsTrue(!String.IsNullOrEmpty(id));
        }

        [TestMethod]
        public void CreateUserByTokenVerify()
        {

            UserTokenLogic tokenlogic = new UserTokenLogic();
            var list = tokenlogic.GetUsertokenList();
            UserToken token = list.FirstOrDefault(u => u.Email == "pavithra.shivarudrappa@fluke.com");
            bool status = tokenlogic.VerifyUserToken(token);
            if (status)
            {
                Registration reg = new Registration();
                reg.FirstName = "pavithra";
                reg.LastName = "S";
                reg.OrganizationName = "Fluke";
                reg.Email = "pavithra.shivarudrappa@fluke.com";
                reg.Password = "Test@1234";
                reg.PhoneNumber = "1234567890";

                string id = logic.CreateUser(reg);
                Assert.IsTrue(!String.IsNullOrEmpty(id));
            }
            else
                Assert.Fail("Invalid Token");
        }

        [TestMethod]
        public void AuthenticateUser()
        {
            var data = logic.AuthenticateUser("veereshrdrpp@gmail.com", "Test@1234");
            Assert.IsNotNull(data);
        }

    }
}
