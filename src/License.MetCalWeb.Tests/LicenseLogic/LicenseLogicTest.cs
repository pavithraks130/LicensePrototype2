using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.Core.DBContext;
using License.Core.Manager;
using License.DataModel;
using License.Logic.DataLogic;
using Microsoft.AspNet.Identity.EntityFramework;
namespace License.MetCalWeb.Tests.LicenseLogic
{
    [TestClass]
    public class LicenseLogicTest
    {
        License.Logic.DataLogic.LicenseLogic licLogic = null;
        License.Logic.DataLogic.UserLogic userLogic = null;
        UserSubscriptionLogic useSubLogic = null;

        AppUserManager UserManager { get; set; }
        AppRoleManager RoleManager { get; set; }
        User user { get; set; }
        UserSubscription sub { get; set; }
        public LicenseLogicTest()
        {
            InitializerClass.Initialize();
           
            licLogic = new Logic.DataLogic.LicenseLogic();
            userLogic = new UserLogic();
            useSubLogic = new UserSubscriptionLogic();
           
            user = userLogic.GetUserByEmail("pavithra.shivarudrappa@fluke.com");
            sub = useSubLogic.GetSubscription(user.UserId).First();

        }

        [TestMethod]
        public void CreateLicenseData()
        {
            try
            {
                List<DataModel.LicenseData> LicenseDataList = new List<LicenseData>();
                LicenseData data1 = new LicenseData();
                data1.LicenseKey = Guid.NewGuid().ToString();
                data1.ProductId = 1;
                data1.UserSubscriptionId = sub.Id;
                data1.AdminUserId = user.UserId;
                LicenseDataList.Add(data1);
                LicenseData data2 = new LicenseData();
                data2.LicenseKey = Guid.NewGuid().ToString();
                data2.ProductId = 1;
                data2.UserSubscriptionId = sub.Id;
                data2.AdminUserId = user.UserId;
                LicenseDataList.Add(data2);
                LicenseData data3 = new LicenseData();
                data3.LicenseKey = Guid.NewGuid().ToString();
                data3.ProductId = 1;
                data3.UserSubscriptionId = sub.Id;
                data3.AdminUserId = user.UserId;
                LicenseDataList.Add(data3);
                licLogic.CreateLicenseData(LicenseDataList);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void GetLicenseBySubscription()
        {
            try
            {
                var licList = licLogic.GetLicenseList(sub.Id);
                Assert.IsTrue(licList.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void GetUnassignedLicense()
        {
            try
            {
                licLogic.GetUnassignedLicense(sub.Id, 1);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
