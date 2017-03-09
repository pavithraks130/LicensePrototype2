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
        UserLogic userLogic = null;
        SubscriptionTypeLogic subTypelogic = null;
        User usr = null;

        public UserSubscriptionLogicTest()
        {
            logic = new UserSubscriptionLogic();
            userLogic = new UserLogic();
            subTypelogic = new SubscriptionTypeLogic();
            usr = userLogic.GetUserByEmail("veereshrdrpp@gmail.ccom");
            InitializerClass.Initialize();
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
            if (usr != null)
            {
                var sub = subTypelogic.GetSubscriptionType();
                if (sub.Count > 0)
                {
                    var subscriptioinTypeId = sub.FirstOrDefault(s => s.Name == "Sub1").Id;
                    string userId = usr.UserId;
                    UserSubscription subs = new UserSubscription();
                    subs.SubscriptionTypeId = subscriptioinTypeId;
                    subs.Quantity = 4;
                    subs.UserId = userId;
                    subs.SubscriptionDate = DateTime.Now;
                    var data = logic.CreateUserSubscription(subs,usr.OrganizationId);
                    Assert.IsTrue(data != null);
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
