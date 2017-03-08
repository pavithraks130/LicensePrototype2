using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LicenseServer.Logic;
namespace License.MetCalWeb.Tests.LicenseServer.Logic
{
    [TestClass]
    public class SubscriptionLogicTest
    {
        SubscriptionTypeLogic logic = new SubscriptionTypeLogic();
        [TestMethod]
        public void GetSubscriptions()
        {
            InitializerClass.Initialize();
            var typeList = logic.GetSubscriptionType();
            Assert.IsNotNull(typeList);
        }
    }
}
