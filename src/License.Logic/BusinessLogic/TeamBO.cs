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
        ProductLicenseLogic licLogic = null;
        UserLicenseLogic userLicLogic = null;

        public string ErrorMessage { get; set; }

        public AppUserManager UserManager { get; set; }

        public AppRoleManager RoleManager { get; set; }
        public TeamBO()
        {
            userLogic = new UserLogic();
            logic = new TeamMemberLogic();
            teamLogic = new TeamLogic();
            teamLicenseLogic = new TeamLicenseLogic();
            userLicLogic = new UserLicenseLogic();
            licLogic = new ProductLicenseLogic();
        }

        public void Initialize()
        {
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;
        }
        /// Based on Team Id get Team Details
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

        /// Create Team Member Invitation
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

        /// Remove Team Member  from team 
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
            var productIdList = GetProductByTeamId(teamId);
            for (int index = 0; index < productIdList.Count; index++)
            {
                var product = subscriptionBO.GetProductFromJsonFile(productIdList[index]);
                if (product != null)
                {
                    productList.Add(product);
                }
            }
            return productList;
        }
        
        /// Deleting listed products from selected team.
        public bool DeleteTeamLicense(TeamLicenseDataMapping teamObj)
        {
            var proceedDelete = true;
            List<int> collectionOfprodIdToDelete = new List<int>();//It contains same product id multiple times.
            foreach (var team in teamObj.TeamList)
            {
                // Checks if any team Member is logged In if Yes the the error response will be sent.
                var userList = logic.GetTeamMembers(team.Id).Where(tm => tm.InviteeUser.IsActive == true).ToList();
                if (userList != null && userList.Count > 0)
                {
                    ErrorMessage = "Won't be able to revoke Licence until team Members log out";
                    proceedDelete = proceedDelete && false;
                }
                // If the no user has logged in and if any team License is assigned to admin for the team which is being deleted then
                // mapped Team license will be deleted for the admin
                if (proceedDelete)
                {
                    userLicLogic.RevokeTeamLicenseFromUser(team.AdminUser.UserId, team.Id);
                    // Team License will be deleted here and license status will be updated in the Product License DB.
                    var teamLicList = teamLicenseLogic.GetTeamLicense(team.Id);
                    foreach (var pro in teamObj.LicenseDataList)
                    {
                        var teamLicListByPro = teamLicList.Where(tl => tl.ProductId == pro.ProductId).ToList();
                        teamLicListByPro.ForEach(teamlic => { teamLicenseLogic.RemoveTeamLicenseById(teamlic.Id, teamlic.LicenseId); });
                    }
                }
                else
                    break;
            }
            return proceedDelete;

        }

        //Gets List of Product Based on TeamId
        public List<int> GetProductByTeamId(int teamId)
        {
            List<int> productIdList = new List<int>();
            var teamLicenseIdList = teamLicenseLogic.GetTeamLicense(teamId).Select(tl => tl.LicenseId).ToList();
            if (teamLicenseIdList.Count > 0)
            {
                var licenseData = licLogic.GetLicenseData();
                productIdList = licenseData.Where(x => teamLicenseIdList.Contains(x.Id)).Select(p => p.ProductId).Distinct().ToList();
                ////Get distinct Product Id List, by license id 
                //foreach (var liceId in teamLicenseIdList)
                //{
                //    int licenseProductId = licenseData.Where(x => x.Id == liceId).Select(p => p.ProductId).FirstOrDefault();
                //    //check whether the product id already exist in list
                //    if (productIdList.Count == 0 || !productIdList.Contains(licenseProductId))
                //        productIdList.Add(licenseProductId);
                //}
            }
            return productIdList;
        }

        /// Get all the Team product by UserId or AdminId if isAdmin set to true
        public List<int> GetTeamProductByUserId(string userId, bool isAdmin)
        {
            List<Team> teamList = null;
            // Get team list based on the user id or admin Id
            if (isAdmin)
                teamList = teamLogic.GetTeamsByAdmin(userId);
            else
                teamList = teamLogic.GetTeamsByUser(userId);
            // Listing all the Products based on the team Id and returns the list of Product ids
            List<int> productIds = new List<int>();
            foreach (var team in teamList)
            {
                var proIds = GetProductByTeamId(team.Id);
                productIds.AddRange(proIds);
            }

            return productIds.Distinct().ToList();
        }

        /// Update Concurrent user Count, If the product License is already mapped then based on the count changes the 
        /// Product license will added or removed for the team for the products which are mapped.
        public TeamConcurrentUserResponse UpdateConcurrentUsers(Team team)
        {
            TeamConcurrentUserResponse concurentUserResponse = new TeamConcurrentUserResponse();
            concurentUserResponse.TeamId = team.Id;
            // Get Team Based on the Team Id
            var dbTeamObj = teamLogic.GetTeamById(team.Id);
            // Get Team License List based on the Team >id
            var teamLicList = teamLicenseLogic.GetTeamLicense(team.Id);

            // specifying concurrent user for first time 
            if (dbTeamObj.ConcurrentUserCount == 0)
                teamLogic.UpdateConcurrentUser(team);

            //If dbConcurrent user is greater than the changed valuee
            else if (dbTeamObj.ConcurrentUserCount > team.ConcurrentUserCount)
            {
                //Get List of the Product id from teamLicList Object
                var proList = teamLicList.Select(t => t.ProductId).Distinct();

                //Looping license List based on the product Id 
                foreach (var pro in proList)
                {
                    // Get team license list based on he Product from teamLicList Object
                    var licList = teamLicList.Where(t => t.ProductId == pro).ToList();

                    //Deleting the team License  which is more than the concurrent user count in the Team license table
                    for (int k = team.ConcurrentUserCount; k < licList.Count; k++)
                    {
                        var deleteLic = licList[k];
                        if (deleteLic.IsMapped)
                            userLicLogic.RevokeTeamLicenseFromUser(deleteLic.Id);
                        licLogic.UpdateLicenseStatus(deleteLic.LicenseId, false);
                        teamLicenseLogic.RemoveTeamLicenseById(deleteLic.Id, deleteLic.LicenseId);
                    }
                }
                //Update the concurrent user to Team table
                teamLogic.UpdateConcurrentUser(team);
            }

            //If dbConcurrent user is lesser than the changed value
            else if (dbTeamObj.ConcurrentUserCount < team.ConcurrentUserCount)
            {
                // Querying no of license required 
                var requiredLicense = team.ConcurrentUserCount - dbTeamObj.ConcurrentUserCount;
                // Getting list of Product whose license is already mapped to the team
                var proIds = teamLicList.Select(l => l.ProductId).Distinct();

                // Verifying the required number of license is available for the products which is mapped already to the team
                bool isLicenseAvailable = true;
                foreach (var pro in proIds)
                {
                    var licAvailableCount = licLogic.GetAvailableLicenseCountByProduct(pro);
                    isLicenseAvailable &= licAvailableCount >= requiredLicense;
                }

                // If not available then the error Response will be sent else the  required license will be  added to the team License.
                if (!isLicenseAvailable)
                {
                    concurentUserResponse.ErrorMessage = "Not much license Exist";
                    concurentUserResponse.UserUpdateStatus = false;
                    concurentUserResponse.OldUserCount = dbTeamObj.ConcurrentUserCount;
                    return concurentUserResponse;
                }
                else
                {
                    // Creation of Team license  based on the number of Concurrent User Count
                    TeamLicenseDataMapping licMappingObj = new TeamLicenseDataMapping()
                    {
                        LicenseDataList = proIds.Select(p => new ProductLicense() { ProductId = p }).ToList(),
                        TeamList = new List<Team>() { new Team() { Id = team.Id, ConcurrentUserCount = requiredLicense } }
                    };
                    teamLicenseLogic.CreateTeamLicense(licMappingObj);
                }

                teamLogic.UpdateConcurrentUser(team);
            }
            concurentUserResponse.UserUpdateStatus = true;
            return concurentUserResponse;
        }

        /// Delete Team , along with the team delete if any license is mapped then remove the team License.
        public bool DeleteTeam(int teamId)
        {
            try
            {
                // Getting the Team License list for team
                var teamLicList = teamLicenseLogic.GetTeamLicense(teamId);
                // Checking the is teama license is mapped if its mapped to user then remove the Team License from User and
                // then delete the Team License
                if (teamLicList != null && teamLicList.Count > 0)
                    teamLicList.ForEach(teamlic =>
                    {
                        if (teamlic.IsMapped) userLicLogic.RevokeTeamLicenseFromUser(teamlic.Id);
                        teamLicenseLogic.RemoveTeamLicenseById(teamlic.Id, teamlic.LicenseId);
                    });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            bool status = false;
            if (String.IsNullOrEmpty(ErrorMessage))
                status = teamLogic.DeleteTeam(teamId);
            if (!status)
                ErrorMessage = ErrorMessage + " " + teamLogic.ErrorMessage;
            return status;
        }
    }
}