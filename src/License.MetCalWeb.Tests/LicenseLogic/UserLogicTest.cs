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
            var dbContext = ApplicationDbContext.Create();
            UserStore<AppUser> userStore = new UserStore<AppUser>(dbContext);
            RoleStore<Role> roleStore = new RoleStore<Role>(dbContext);
            manager = new Moq.Mock<AppUserManager>(userStore);
            rolemanager = new Moq.Mock<AppRoleManager>(roleStore);
            logic.UserManager = manager.Object;
            logic.RoleManager = rolemanager.Object;
        }

        [TestMethod]
        public void GetUser()
        {
            var users = logic.GetUsers();
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void CrearUser()
        {
            Model.Model.Registration reg = new Model.Model.Registration();
            reg.FirstName = "veeresh";
            reg.LastName = "S";
            reg.OrganizationName = "sidssol";
            reg.Email = "veereshrdrpp@gmail.com";
            reg.Password = "Test@1234";
            reg.PhoneNumber = "1234567890";
            var result = logic.CreateUser(reg);
            Assert.AreEqual("true", result.Succeeded);

        }
    }
}
