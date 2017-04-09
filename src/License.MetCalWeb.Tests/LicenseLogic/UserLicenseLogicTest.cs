using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.Logic.DataLogic;
using License.DataModel;
using License.Core.DBContext;
using License.Core.Manager;
using Microsoft.AspNet.Identity.EntityFramework;
namespace License.MetCalWeb.Tests.LicenseLogic
{
    [TestClass]
    public class UserLicenseLogicTest
    {
        UserLicenseLogic userLicLogic = null;
        License.Logic.DataLogic.LicenseLogic licLogic = null;
        UserLogic userLogic = null;
        UserSubscriptionLogic subLogic = null;

        List<LicenseData> LicenseList = null;
        AppUserManager UserManager { get; set; }

        AppRoleManager RoleManager { get; set; }

        User AdminUser { get; set; }

        User TeamMember { get; set; }

        UserSubscription Sub { get; set; }

        public UserLicenseLogicTest()
        {
            InitializerClass.Initialize();

            userLicLogic = new UserLicenseLogic();
            userLogic = new UserLogic();
            licLogic = new Logic.ServiceLogic.LicenseLogic();
            userLicLogic = new UserLicenseLogic();
            subLogic = new UserSubscriptionLogic();
            AdminUser = userLogic.GetUserByEmail("pavithra.shivarudrappa@fluke.com");
            TeamMember = userLogic.GetUserByEmail("pavithraks2006@yahoo.com");
            Sub = subLogic.GetSubscription(AdminUser.UserId).First();
            LicenseList = licLogic.GetLicenseList(Sub.Id);
        }

        [TestMethod]
        public void CreateUserLicense()
        {
            try
            {
                List<UserLicense> userLicList = new List<UserLicense>();
                UserLicense lic = new UserLicense();
                lic.UserId = TeamMember.UserId;
                lic.LicenseId = LicenseList[0].Id;
                lic.License = licLogic.GetLicenseById(lic.LicenseId);
                userLicList.Add(lic);
                UserLicense lic1 = new UserLicense();
                lic1.UserId = TeamMember.UserId;
                lic1.LicenseId = LicenseList[1].Id;
                lic1.License = licLogic.GetLicenseById(lic1.LicenseId);
                userLicList.Add(lic1);
                userLicLogic.CreateMultiUserLicense(userLicList, new List<string>() { TeamMember.UserId });

                userLicList = new List<UserLicense>();
                UserLicense lic2 = new UserLicense();
                lic2.UserId = AdminUser.UserId;
                lic2.LicenseId = LicenseList[2].Id;
                lic2.License = licLogic.GetLicenseById(lic2.LicenseId);
                userLicList.Add(lic2);
                userLicLogic.CreateMultiUserLicense(userLicList, new List<string>() { AdminUser.UserId });
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void RevokeuserLicense()
        {
            try
            {
                var userLicList = userLicLogic.GetUserLicense(TeamMember.UserId);
                var obj = userLicList.First();
                userLicLogic.RevokeUserLicense(new List<UserLicense> { obj }, TeamMember.UserId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod]
        public void GetUserLicenseDetails()
        {
            try
            {
                userLicLogic.GetUserLicense(TeamMember.UserId);
                userLicLogic.GetUserLicense(AdminUser.UserId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

    }
}
