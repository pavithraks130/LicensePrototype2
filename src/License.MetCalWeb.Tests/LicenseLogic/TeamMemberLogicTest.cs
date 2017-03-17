using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.Logic.ServiceLogic;
using License.Model;
using License.Core.DBContext;
using Microsoft.AspNet.Identity.EntityFramework;
using License.Core.Model;
using License.Core.Manager;

namespace License.MetCalWeb.Tests.LicenseLogic
{
    [TestClass]
    public class TeamMemberLogicTest
    {
        TeamMemberLogic teamLogic = null;
        UserLogic userLogic = null;
        AppUserManager UserManager { get; set; }
        AppRoleManager RoleManager { get; set; }
        public TeamMemberLogicTest()
        {
            InitializerClass.Initialize();
            userLogic = new UserLogic();
            teamLogic = new TeamMemberLogic();
        }

        [TestMethod]
        public void CreateInvite()
        {
            User adminUser = userLogic.GetUserByEmail("apsarj@gmail.com");
            Registration usr = new Registration();
            usr.Email = "pavithraks2006@yahoo.com";
            usr.Password = "Test@1234";
            usr.ManagerId = adminUser.UserId;
            userLogic.CreateUser(usr);
            var obj = userLogic.GetUserByEmail(usr.Email);
            License.Model.TeamMembers member = new License.Model.TeamMembers();
            member.AdminId = adminUser.UserId;
            member.InvitationDate = DateTime.Now;
            member.InviteeStatus = License.Logic.Common.InviteStatus.Pending.ToString();
            member.InviteeUserId = obj.UserId;
            member.InviteeEmail = obj.Email;
            var memberObj = teamLogic.CreateInvite(member);
            Assert.IsTrue(memberObj.Id > 0);
        }
        
        [TestMethod]
        public void UpdateInviteStatus()
        {
            User adminUser = userLogic.GetUserByEmail("apsarj@gmail.com");
            var obj = teamLogic.VerifyUserInvited("pavithraks2006@yahoo.com", adminUser.UserId);
            if (obj != null)
            {
                teamLogic.UpdateInviteStatus(obj.Id, License.Logic.Common.InviteStatus.Accepted.ToString());
                Assert.IsTrue(obj.Id > 0);
            }
            else
            {
                Assert.Fail("No invite exist for the sspecified EMail");
            }
        }

        [TestMethod]
        public void GetUserinvite()
        {
            User adminUser = userLogic.GetUserByEmail("apsarj@gmail.com");
            var dataList = teamLogic.GetUserInviteDetails(adminUser.UserId);
            Assert.IsTrue(dataList.AcceptedInvites.Count > 0 || dataList.PendingInvites.Count > 0);
        }

        [TestMethod]
        public void SetAsAdmin()
        {
            User adminUser = userLogic.GetUserByEmail("apsarj@gmail.com");
            var obj = teamLogic.VerifyUserInvited("pavithraks2006@yahoo.com", adminUser.UserId);
            teamLogic.SetAsAdmin(obj.Id, obj.InviteeUserId, true);
        }

    }
}
