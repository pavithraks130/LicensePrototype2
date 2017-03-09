using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace License.MetCalWeb.Tests.LicenseServer.Logic
{
    [TestClass]
    public class UserLogicTest
    {
        UserLogic logic;
        public UserLogicTest()
        {
            logic = new UserLogic();
        }

        [TestMethod]
        public void CreateUser()
        {
            Registration reg = new Registration();
            reg.FirstName = "veeresh";
            reg.LastName = "S";
            reg.OrganizationName = "sidssol";
            reg.Email = "veereshrdrpp@gmail.com";
            reg.Password = "Test@1234";
            reg.PhoneNumber = "1234567890";
            string id = logic.CreateUser(reg);
            Assert.IsTrue(String.IsNullOrEmpty(id));
        }

        [TestMethod]
        public void AuthenticateUser()
        {
            var data = logic.AutheticateUser("veereshrdrpp@gmmail.com", "Test@1234");
            Assert.IsNotNull(data);
        }
        
    }
}
