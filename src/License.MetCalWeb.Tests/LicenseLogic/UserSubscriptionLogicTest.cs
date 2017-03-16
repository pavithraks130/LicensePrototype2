using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.Core.DBContext;
using License.Logic.ServiceLogic;
using License.Core.Manager;
using License.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace License.MetCalWeb.Tests.LicenseLogic
{
    [TestClass]
    public class UserSubscriptionLogicTest
    {
        UserSubscriptionLogic useSubLogic = null;
        UserLogic userLogic = null;
        AppUserManager UserManager { get; set; }

        AppRoleManager RoleManager { get; set; }

        public UserSubscriptionLogicTest()
        {
            InitializerClass.Initialize();
            var dbContext = ApplicationDbContext.Create();
            var userStore = new UserStore<Core.Model.AppUser>(dbContext);
            var roleStore = new RoleStore<Core.Model.Role>(dbContext);
            UserManager = new AppUserManager(userStore);
            RoleManager = new AppRoleManager(roleStore);
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;
            useSubLogic.UserManager = UserManager;
            useSubLogic.RoleManager = RoleManager;
        }

        [TestMethod]
        public void GetSubscriptionList()
        {
            User adminUser = userLogic.GetUserByEmail("apsarj@gmail.com");
            var subList = useSubLogic.GetSubscription(adminUser.UserId);
            Assert.IsTrue(subList.Count > 0);
        }

        [TestMethod]
        public void CreateSubscription()
        {
            User adminUser = userLogic.GetUserByEmail("apsarj@gmail.com");
            UserSubscription sub = new UserSubscription();
            sub.Quantity = 2;
            sub.SubscriptionDate = DateTime.Now;
            sub.SubscriptionId = 1;
            sub.UserId = adminUser.UserId;
            useSubLogic.CreateSubscription(sub);
        }
    }
}
