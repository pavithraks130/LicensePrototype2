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
    public class UserSubscriptionLogicTest
    {
        UserSubscriptionLogic logic = null;
        UserLogic userLogic = null;
        SubscriptionTypeLogic subTypelogic = null;
        User usr = null;

        public UserSubscriptionLogicTest()
        {
            InitializerClass.Initialize();
            logic = new UserSubscriptionLogic();
            userLogic = new UserLogic();
            subTypelogic = new SubscriptionTypeLogic();
            usr = userLogic.GetUserByEmail("veereshrdrpp@gmail.com");

        }

        [TestMethod]
        public void GetUserSubscription()
        {
            if (usr != null)
            {
                string userId = usr.UserId;
                var data = logic.GetUserSubscription(userId);
                Assert.IsTrue(data != null && data.Count > 0);
            }
            else
            {
                Assert.Fail("Specified User not EXist in the application");
            }
        }

        [TestMethod]
        public void CreateUserSubscription()
        {
            CartLogic cartlogic = new CartLogic();
            if (usr != null)
            {
                var itemList = cartlogic.GetCartItems(usr.UserId);
                if (itemList.Count > 0)
                {
                    int i = 0;
                    foreach (var cartItem in itemList)
                    {
                        string userId = usr.UserId;
                        UserSubscription subs = new UserSubscription();
                        subs.SubscriptionTypeId = cartItem.SubscriptionTypeId;                      
                        subs.Quantity = cartItem.Quantity;
                        subs.UserId = userId;
                        subs.SubscriptionDate = DateTime.Now;
                        var data = logic.CreateUserSubscription(subs, usr.OrganizationId);
                        i++;
                    }
                    Assert.IsTrue(i > 0);
                }
                else
                {
                    Assert.Fail("Specified User not EXist in the application");
                }
            }
            else
            {
                Assert.Fail("Specified User not EXist in the application");
            }
        }


    }
}
