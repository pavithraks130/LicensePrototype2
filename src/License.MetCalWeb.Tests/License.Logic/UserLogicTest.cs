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
namespace License.MetCalWeb.Tests.License.Logic
{
    [TestClass]
    public class UserLogicTest
    {
        UserLogic logic = new UserLogic();
        [TestMethod]
        public void GetUser()
        {
            InitializerClass.Initialize();
            UserStore<AppUser> userStore = new UserStore<AppUser>(ApplicationDbContext.Create());
            var manager = new Moq.Mock<AppUserManager>(userStore);
            logic.UserManager = manager.Object;
            var users =  logic.GetUsers();
            Assert.IsNotNull(users);
        }
    }
}
