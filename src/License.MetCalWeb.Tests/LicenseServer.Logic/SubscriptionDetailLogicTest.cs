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
    public class SubscriptionDetailLogicTest
    {
        SubscriptionDetailLogic detailLogic;
        public SubscriptionDetailLogicTest()
        {
            detailLogic = new SubscriptionDetailLogic();
            InitializerClass.Initialize();
        }


        [TestMethod]
        public void GetSubscriptionDetail()
        {
            int subscriptionId = 1;
            var list = detailLogic.GetSubscriptionDetails(subscriptionId);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void CreateSubscriptionDetail()
        {
            int proId = 1;
            int subscriptioinTypeId = 1;
            SubscriptionDetails detail = new SubscriptionDetails();
            detail.ProductId = proId;
            detail.SubscriptionTypeId = subscriptioinTypeId;
            detail.Quantity = 4;
            List<SubscriptionDetails> details = new List<SubscriptionDetails>();
            details.Add(detail);
            var status = detailLogic.CreateSubscriptionDetails(details);
            Assert.IsTrue(status);
        }
    }
}
