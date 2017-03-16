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
    public class CartLogicTest
    {
        CartLogic cartLogic = null;
        UserLogic logic = null;
        SubscriptionTypeLogic subTypeLogic = null;
        User usr = null;
        public CartLogicTest()
        {
            cartLogic = new CartLogic();
            logic = new UserLogic();
            subTypeLogic = new SubscriptionTypeLogic();
            InitializerClass.Initialize();
            usr = logic.GetUserByEmail("veereshrdrpp@gmail.com");
        }
        [TestMethod]
        public void GetCartDetails()
        {
            var obj = cartLogic.GetCartItems(usr.UserId);
            Assert.IsTrue(obj.Count > 0);
        }

        [TestMethod]
        public void AddCartItem()
        {
            var sub = subTypeLogic.GetSubscriptionType();
            if (sub.Count > 0)
            {
                var obj = sub.FirstOrDefault(s => s.Name == "Sub1");
                var subscriptioinTypeId = obj.Id;
                var status = cartLogic.CreateCartItem(new CartItem() { Quantity = 2, SubscriptionTypeId = subscriptioinTypeId, UserId = usr.UserId, DateCreated = DateTime.Now.Date, Price = 2 * obj.Price });
                Assert.IsTrue(status);
            }
            else
            {
                Assert.Fail("Subscription Type does not exist");
            }
        }

        [TestMethod]
        public void DeleteCartItem()
        {
            var obj = cartLogic.GetCartItems(usr.UserId);
            if (obj.Count > 0)
            {
                var data = obj.FirstOrDefault();
                var status = cartLogic.DeleteCartItem(data.Id);
                Assert.IsTrue(status);
            }
        }
    }
}
