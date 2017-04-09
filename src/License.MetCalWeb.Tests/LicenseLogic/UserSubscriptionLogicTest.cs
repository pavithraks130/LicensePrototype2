using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.Core.DBContext;
using License.Logic.DataLogic;
using License.Core.Manager;
using License.DataModel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace License.MetCalWeb.Tests.LicenseLogic
{
    [TestClass]
    public class UserSubscriptionLogicTest
    {
        UserSubscriptionLogic useSubLogic = null;
        UserLogic userLogic = null;

        public UserSubscriptionLogicTest()
        {
            InitializerClass.Initialize();
            useSubLogic = new UserSubscriptionLogic();
            userLogic = new UserLogic();
        }

        [TestMethod]
        public void GetSubscriptionList()
        {
            User adminUser = userLogic.GetUserByEmail("pavithra.shivarudrappa@fluke.com");
            var subList = useSubLogic.GetSubscription(adminUser.UserId);
            Assert.IsTrue(subList.Count > 0);
        }

        [TestMethod]
        public void CreateSubscription()
        {
            User adminUser = userLogic.GetUserByEmail("pavithra.shivarudrappa@fluke.com");
            UserSubscription sub = new UserSubscription();
            sub.Quantity = 2;
            sub.SubscriptionDate = DateTime.Now;
            sub.SubscriptionId = 1;
            sub.UserId = adminUser.UserId;
            useSubLogic.CreateSubscription(sub);
        }
    }
}
