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
    public class SubscriptionLogicTest
    {
        SubscriptionTypeLogic logic = null;

        public SubscriptionLogicTest()
        {
            logic = new SubscriptionTypeLogic();
            InitializerClass.Initialize();
        }

        [TestMethod]
        public void GetSubscriptions()
        {
            var typeList = logic.GetSubscriptionType();
            Assert.IsNotNull(typeList);
        }

        [TestMethod]
        public void CreateSubscription()
        {
            var result = logic.CreateSubscription(new SubscriptionType() { Name = "Sub1", ActiveDays = 340, ImagePath = "P1.png", Price = 1200 });
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void GetSubscriptionById()
        {
            int subscriptionId = 1;
            var obj = logic.GetById(subscriptionId);
            Assert.IsNotNull(obj);
        }
    }
}
