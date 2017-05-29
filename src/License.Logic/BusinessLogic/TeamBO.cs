using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Logic.DataLogic;
using License.DataModel;
using License.Logic.Common;
using License.Core.Manager;
namespace License.Logic.BusinessLogic
{
    public class TeamBO
    {
        UserLogic userLogic = null;
        TeamMemberLogic logic = null;
        TeamLogic teamLogic = null;
        TeamLicenseLogic teamLicenseLogic = null;

        public string ErrorMessage { get; set; }

        public AppUserManager UserManager { get; set; }

        public AppRoleManager RoleManager { get; set; }
        public TeamBO()
        {
            userLogic = new UserLogic();
            logic = new TeamMemberLogic();
            teamLogic = new TeamLogic();
            teamLicenseLogic = new TeamLicenseLogic();
        }

        public void Initialize()
        {
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;
        }

        public TeamDetails GetteamDetails(int id)
        {
            TeamLogic teamLogic = new TeamLogic();
            TeamDetails dtls = new TeamDetails();
            var team = teamLogic.GetTeamById(id);
            if (team != null)
            {
                dtls.Team = new Team();
                dtls.Team.AdminId = team.AdminId;
                dtls.Team.AdminUser = team.AdminUser;
                dtls.Team.Id = team.Id;
                dtls.Team.Name = team.Name;
                if (team.TeamMembers.Count > 0)
                {
                    dtls.PendinigUsers =
                        team.TeamMembers.Where(s => s.InviteeStatus == InviteStatus.Pending.ToString()).ToList();
                    dtls.AcceptedUsers =
                        team.TeamMembers.Where(s => s.InviteeStatus == InviteStatus.Accepted.ToString()).ToList();
                    dtls.AcceptedUsers.Add(new TeamMember()
                    {
                        InviteeEmail = team.AdminUser.Email,
                        InviteeStatus = InviteStatus.Accepted.ToString(),
                        InviteeUserId = team.AdminUser.UserId,
                        InviteeUser = team.AdminUser,
                        IsAdmin = true
                    });
                }
                else
                {
                    dtls.AcceptedUsers.Add(new TeamMember()
                    {
                        InviteeEmail = team.AdminUser.Email,
                        InviteeStatus = InviteStatus.Accepted.ToString(),
                        InviteeUserId = team.AdminUser.UserId,
                        InviteeUser = team.AdminUser,
                        IsAdmin = true
                    });
                }
            }
            return dtls;
        }

        public TeamMemberResponse CreateTeamMembereInvite(TeamMember member)
        {
            Initialize();
            var user = userLogic.GetUserByEmail(member.InviteeEmail);
            TeamMemberResponse teamMemResObj = new TeamMemberResponse();
            string password = System.Configuration.ConfigurationManager.AppSettings.Get("TeamMemberDefaultPassword");
            if (user == null)
            {
                Registration reg = new Registration()
                {
                    Email = member.InviteeEmail,
                    Password = password
                };
                var status = userLogic.CreateUser(reg, "TeamMember");
                if (status)
                    user = userLogic.GetUserByEmail(member.InviteeEmail);
                else
                {
                    ErrorMessage = userLogic.ErrorMessage;
                    return null;
                }
            }

            member.InviteeUserId = user.UserId;
            var teamMemObj = logic.CreateInvite(member);
            if (teamMemObj != null)
            {
                teamMemResObj.UserId = user.UserId;
                teamMemResObj.UserName = user.UserName;
                teamMemResObj.Password = password;
                teamMemResObj.TeamMemberId = teamMemObj.Id;
                return teamMemResObj;
            }
            else
                ErrorMessage = logic.ErrorMessage;

            return null;

        }

        public bool AddTeamMembers(List<TeamMember> teamMemberList)
        {
            Initialize();
            User user = null;
            foreach (var mem in teamMemberList)
            {
                if (user == null || user.UserId != mem.InviteeUserId)
                    user = userLogic.GetUserById(mem.InviteeUserId);
                mem.InviteeEmail = user.Email;
                mem.InvitationDate = DateTime.Now.Date;
                logic.CreateInvite(mem);
            }
            return true;
        }


        public bool RemoveTeamMembers(List<TeamMember> teamMemberList)
        {
            bool status = true;
            foreach (var mem in teamMemberList)
            {
                status &= logic.DeleteTeamMember(mem);
                var adminId = teamLogic.GetTeamById(mem.TeamId).AdminId;
                var team = teamLogic.GetTeamsByAdmin(adminId).FirstOrDefault(t => t.IsDefaultTeam);
                var memberlist = logic.GetTeamMemberDetailsByUserId(mem.InviteeUserId);
                if (memberlist.Count == 0)
                    logic.CreateInvite(new TeamMember() { InvitationDate = DateTime.Now.Date, TeamId = team.Id, InviteeEmail = mem.InviteeEmail, InviteeUserId = mem.InviteeUserId, InviteeStatus = License.Logic.Common.InviteStatus.Accepted.ToString() });
            }
            return status;
        }
        /// <summary>
        /// It will returns the list of products based on selected team.
        /// </summary>
        /// <param name="teamId">selected team id.</param>
        /// <returns>List of products</returns>
        public List<Product> GetTeamLicenseProductByTeamId(int teamId)
        {
            SubscriptionBO subscriptionBO = new SubscriptionBO();
            List<Product> productList = new List<Product>();
            List<int> productIdList = new List<int>();
            var teamLicenseIdList = teamLicenseLogic.GetTeamLicense(teamId).Select(tl => tl.LicenseId).ToList();
            if (teamLicenseIdList.Count > 0)
            {
                var licenseData = teamLicenseLogic.GetLicenseData();
                //Get distinct Product Id List, by license id 
                foreach (var liceId in teamLicenseIdList)
                {
                    int licenseProductId = licenseData.Where(x => x.Id == liceId).Select(p => p.ProductId).FirstOrDefault();
                    //check whether the product id already exist in list
                    if (productIdList.Count == 0 || !productIdList.Contains(licenseProductId))
                    {
                        productIdList.Add(licenseProductId);
                    }
                }

                for (int index = 0; index < productIdList.Count; index++)
                {
                    string productCode = "Pro_" + productIdList[index].ToString();
                    var product = subscriptionBO.GetProductFromJsonFile(productCode);
                    if (product != null)
                    {
                        productList.Add(product);
                    }
                }
            }
            return productList;
        }

        /// <summary>
        /// Deleting listed products from selected team.
        /// </summary>
        /// <param name="productIdToDelete">product id list for delete from team selected team</param>
        /// <param name="teamId">selected team team id</param>
        /// <returns></returns>
        public bool DeleteTeamLicense(List<int> productIdToDelete, int teamId)
        {
            List<int> collectionOfprodIdToDelete = new List<int>();//It contains same product id multiple times.

            List<Core.Model.LicenseData> licenseDataList = new List<Core.Model.LicenseData>();
            //by using teamid select all License Id from teamlicense table
            var teamLicenseIdList = teamLicenseLogic.GetTeamLicense(teamId).Select(tl => tl.LicenseId).ToList();
            //by using license id find the product id from license data table
            var licenseData = teamLicenseLogic.GetLicenseData();
            foreach (var liceId in teamLicenseIdList)
            {
                var liceData = licenseData.Where(x => x.Id == liceId).FirstOrDefault();
                collectionOfprodIdToDelete.Add(liceData.ProductId);
                licenseDataList.Add(liceData);
            }
            //select only delete request product id from  prodIdList list
            var distinctSelectedTeamProductId = collectionOfprodIdToDelete.Distinct();
            var outerJoinproductId = distinctSelectedTeamProductId.Except(productIdToDelete).ToList();
            foreach (int id in outerJoinproductId)
            {
                collectionOfprodIdToDelete.RemoveAll(x=>x.Equals(id));
                licenseDataList.RemoveAll(p => p.ProductId.Equals(id));
            }
            foreach (var ld in licenseDataList)
            {
                //Making IsMapped true from LicenseData table
                teamLicenseLogic.UpdateLicenseDataTableByProductId(ld.Id);
                //Deleting license from TeamLicense table.
                teamLicenseLogic.RemoveLicenseByLicenseId(ld.Id);
            }
            return true;
        }
    }
}
