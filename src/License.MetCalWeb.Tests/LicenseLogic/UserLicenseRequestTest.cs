using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.Model;
using License.Logic.ServiceLogic;


namespace License.MetCalWeb.Tests.LicenseLogic
{
    [TestClass]
    public class UserLicenseRequestTest
    {
        UserSubscriptionLogic useSubLogic = null;
        UserLogic userLogic = null;
        UserLicenseRequestLogic reqLogic = null;
        public UserLicenseRequestTest()
        {
            InitializerClass.Initialize();
            useSubLogic = new UserSubscriptionLogic();
            userLogic = new UserLogic();
            reqLogic = new UserLicenseRequestLogic();
        }
        [TestMethod]
        public void CreateUserLicenseRequest()
        {
            UserLicenseRequest req = new UserLicenseRequest();
            User adminUser = userLogic.GetUserByEmail("pavithra.shivarudrappa@fluke.com");
            User teamMember = userLogic.GetUserByEmail("pavithraks2006@yahoo.com");
            var subList = useSubLogic.GetSubscription(adminUser.UserId);
            if (subList.Count > 0)
            {
                req.Requested_UserId = teamMember.UserId;
                req.UserSubscriptionId = subList[0].Id;
                req.ProductId = 1;
                req.RequestedDate = DateTime.Now;
                List<UserLicenseRequest> reqList = new List<UserLicenseRequest>();
                reqList.Add(req);
                UserLicenseRequestLogic reqLogic = new UserLicenseRequestLogic();
                reqLogic.Create(reqList);
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("No Subscriptions are purechased");
            }
        }

        [TestMethod]
        public void GetUserLicenseRequest()
        {
            User teamMember = userLogic.GetUserByEmail("pavithraks2006@yahoo.com");
            var data = reqLogic.GetLicenseRequest(teamMember.UserId);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void UpdateUserLicenseRequest()
        {
            UserLicenseRequest req = new UserLicenseRequest();
            User teamMember = userLogic.GetUserByEmail("pavithraks2006@yahoo.com");
            var data = reqLogic.GetLicenseRequest(teamMember.UserId);
            if (data.Count > 0)
            {
                var dt = data[0];
                dt.IsApproved = true;
                dt.ApprovedBy = "pavithra.shivavrudrappa@fluke.com";
                List<UserLicenseRequest> reqList = new List<UserLicenseRequest>();
                reqList.Add(dt);
                UserLicenseRequestLogic reqLogic = new UserLicenseRequestLogic();
                reqLogic.Update(reqList);
            }
            else
            {
                Assert.Fail("No License Request Exist");
            }
        }
    }
}
