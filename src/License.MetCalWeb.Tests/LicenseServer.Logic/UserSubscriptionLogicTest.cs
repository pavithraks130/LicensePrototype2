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
    public class UserSubscriptionLogicTest
    {
        UserSubscriptionLogic logic = null;


        public UserSubscriptionLogicTest()
        {
            logic = new UserSubscriptionLogic();
            InitializerClass.Initialize();
        }

        [TestMethod]
        public void GetUserSubscription()
        {
            string userId = null;
            var data = logic.GetUserSubscription(userId);
            Assert.IsTrue(data != null && data.Count > 0);
        }

        [TestMethod]
        public void CreateUserSubscription()
        {            
            string userId = string.Empty;
            UserSubscription subs = new UserSubscription();
            subs.SubscriptionTypeId = 1;
            subs.Quantity = 4;
            subs.UserId = userId;
            subs.SubscriptionDate = DateTime.Now;
            var data = logic.CreateUserSubscription(subs);
            Assert.IsTrue(data.id > 0);
        }

        
    }
}
