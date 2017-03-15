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
    public class SubscriptionDetailLogicTest
    {
        SubscriptionDetailLogic detailLogic;
        SubscriptionTypeLogic logic;
        ProductLogic proLogic = null;
        public SubscriptionDetailLogicTest()
        {
            detailLogic = new SubscriptionDetailLogic();
            logic = new SubscriptionTypeLogic();
            proLogic = new ProductLogic();
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
            var proList = proLogic.GetProducts();
            var sub = logic.GetSubscriptionType();
            if (proList.Count > 0 && sub.Count > 0)
            {
                var proId = proList.FirstOrDefault(p => p.ProductCode.ToUpper() == "PRO-01").Id;
                var subscriptioinTypeId = sub.FirstOrDefault(s => s.Name == "Sub1").Id;
                SubscriptionDetails detail = new SubscriptionDetails();
                detail.ProductId = proId;
                detail.SubscriptionTypeId = subscriptioinTypeId;
                detail.Quantity = 4;
                List<SubscriptionDetails> details = new List<SubscriptionDetails>();
                details.Add(detail);
                var status = detailLogic.CreateSubscriptionDetails(details);
                Assert.IsTrue(status);
            }
            else
            {
                Assert.Fail("No Subscription and Product Details created");
            }
        }
    }
}
