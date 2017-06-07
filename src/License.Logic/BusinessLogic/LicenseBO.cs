using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using License.Logic.DataLogic;
using License.Core.Manager;

namespace License.Logic.BusinessLogic
{
    /// <summary>
    /// History :
    ///     Created By :
    ///     Created Date :
    ///     Purpose : 1. FUnctionality or Business logic for the User License, Subscription will be performed here.
    /// </summary>
    public class LicenseBO
    {
        UserLicenseRequestLogic userLicenseRequestLogic = null;
        UserLicenseLogic licLogic = null;
        UserLogic userLogic = null;
        TeamLogic _teamLogic = null;
        public AppUserManager UserManager { get; set; }
        public AppRoleManager RoleManager { get; set; }
        public string ErrorMessage { get; set; }

        public LicenseBO()
        {
            userLicenseRequestLogic = new UserLicenseRequestLogic();
            licLogic = new UserLicenseLogic();
            userLogic = new UserLogic();
            _teamLogic = new TeamLogic();
        }

        /// Updating the license Request Status
        public void ApproveOrRejectLicense(List<UserLicenseRequest> licReqList)
        {
            List<UserLicenseRequest> licenseRequestList = new List<UserLicenseRequest>();
            List<UserLicense> userLicenseList = new List<UserLicense>();
            foreach (var licReq in licReqList)
            {
                // If Request is approved then creating the User License record and updating DB
                if (licReq.IsApproved)
                {
                    UserLicense lic = new UserLicense()
                    {
                        UserId = licReq.Requested_UserId,
                        TeamId = licReq.TeamId,
                        License = new ProductLicense()
                    };
                    lic.License.ProductId = licReq.ProductId;
                    lic.License.UserSubscriptionId = licReq.UserSubscriptionId;
                    userLicenseList.Add(lic);
                }
            }
            userLicenseRequestLogic.Update(licReqList);
            ErrorMessage = userLicenseRequestLogic.ErrorMessage;
            if (userLicenseList.Count > 0 && String.IsNullOrEmpty(ErrorMessage))
            {
                licLogic.CreataeUserLicense(userLicenseList);
                ErrorMessage = licLogic.ErrorMessage;
            }
        }

        /// Get User License with features based on the user id
        public UserLicenseDetails GetUserLicenseSubscriptionDetails(FetchUserSubscription model)
        {
            UserLicenseDetails licDetails = new UserLicenseDetails();
            var licenseMapModelList = new List<Subscription>();
            UserLicenseLogic logic = new UserLicenseLogic();
            SubscriptionBO proSubLogic = new SubscriptionBO();
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;

            licDetails.User = userLogic.GetUserById(model.UserId);


            List<UserLicense> data = null;
            if (model.TeamId == 0)
                data = logic.GetUserLicenseByUserId(model.UserId);
            else
                data = logic.GetUserLicenseByUserIdTeamId(model.UserId, model.TeamId);

            var dataList = proSubLogic.GetSubscriptionFromFile();

            if (data.Count > 0)
            {
                var subsIdList = data.Select(l => l.License.Subscription.SubscriptionId);
                var subscriptionList = dataList.Where(s => subsIdList.Contains(s.Id)).ToList();
                DateTime licExpireData = DateTime.MinValue;
                foreach (var subs in subscriptionList)
                {
                    var userLicLicst = data.Where(ul => ul.License.Subscription.SubscriptionId == subs.Id).ToList();
                    var proList = userLicLicst.Select(u => u.License.ProductId).ToList();
                    Subscription mapModel = new Subscription()
                    {
                        Name = subs.Name,
                        UserSubscriptionId = data.FirstOrDefault(us => us.License.Subscription.SubscriptionId == subs.Id).License.UserSubscriptionId
                    };
                    mapModel.Products = new List<Product>();
                    foreach (var pro in subs.Products.Where(p => proList.Contains(p.Id)))
                    {
                        var objLic = userLicLicst.FirstOrDefault(f => f.License.ProductId == pro.Id);
                        if (objLic != null)
                        {
                            string licenseKeydata = String.Empty;
                            licenseKeydata = objLic.License.LicenseKey;
                            var splitData = licenseKeydata.Split(new char[] { '-' });
                            var datakey = splitData[0];
                            var decryptObj = LicenseKey.LicenseKeyGen.CryptoEngine.Decrypt(datakey, true);
                            var licdataList = decryptObj.Split(new char[] { '^' });
                            licExpireData = Convert.ToDateTime(licdataList[1]);
                        }
                        Product prod = new Product()
                        {
                            Id = pro.Id,
                            Name = pro.Name,
                            ExpireDate = licExpireData
                        };
                        if (model.IsFeatureRequired)
                        {
                            foreach (var fet in pro.Features)
                            {
                                var feature = new Feature()
                                {
                                    Id = fet.Id,
                                    Name = fet.Name,
                                    Description = fet.Description,
                                    Version = fet.Version
                                };
                                prod.Features.Add(feature);
                            }
                        }
                        mapModel.Products.Add(prod);
                    }
                    licenseMapModelList.Add(mapModel);
                }
            }
            licDetails.SubscriptionDetails = licenseMapModelList;
            return licDetails;
        }

        // Get Team License based on the Team ID.
        public TeamLicenseDetails GetTeamLicenseSubscriptionDetails(string teamId)
        {
            TeamLicenseDetails licDetails = new TeamLicenseDetails();
            var licenseMapModelList = new List<Subscription>();
            TeamLicenseLogic teamLicenseLogic = new TeamLicenseLogic();
            SubscriptionBO proSubLogic = new SubscriptionBO();
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;

            List<TeamLicense> teamLicenseList = teamLicenseLogic.GetTeamLicense(Convert.ToInt32(teamId));

            var subscriptionTypeList = proSubLogic.GetSubscriptionFromFile();

            if (teamLicenseList.Count > 0)
            {
                var subsIdList = teamLicenseList.Select(l => l.License.Subscription.SubscriptionId);
                var subscriptionList = subscriptionTypeList.Where(s => subsIdList.Contains(s.Id)).ToList();
                DateTime licExpireData = DateTime.MinValue;
                foreach (var subs in subscriptionList)
                {
                    var teamLicList = teamLicenseList.Where(ul => ul.License.Subscription.SubscriptionId == subs.Id).ToList();
                    var proList = teamLicList.Select(u => u.License.ProductId).ToList();
                    Subscription mapModel = new Subscription()
                    {
                        Name = subs.Name,
                        UserSubscriptionId = teamLicenseList.FirstOrDefault(us => us.License.Subscription.SubscriptionId == subs.Id).License.UserSubscriptionId
                    };
                    foreach (var pro in subs.Products.Where(p => proList.Contains(p.Id)))
                    {
                        var objLic = teamLicList.FirstOrDefault(f => f.License.ProductId == pro.Id);
                        if (objLic != null)
                        {
                            string licenseKeydata = String.Empty;
                            licenseKeydata = objLic.License.LicenseKey;
                            var splitData = licenseKeydata.Split(new char[] { '-' });
                            var datakey = splitData[0];
                            var decryptObj = LicenseKey.LicenseKeyGen.CryptoEngine.Decrypt(datakey, true);
                            var licdataList = decryptObj.Split(new char[] { '^' });
                            licExpireData = Convert.ToDateTime(licdataList[1]);
                        }
                        Product prod = new Product()
                        {
                            Id = pro.Id,
                            Name = pro.Name,
                            ExpireDate = licExpireData
                        };
                        foreach (var fet in pro.Features)
                        {
                            var feature = new Feature()
                            {
                                Id = fet.Id,
                                Name = fet.Name,
                                Description = fet.Description,
                                Version = fet.Version
                            };
                            prod.Features.Add(feature);
                        }
                        mapModel.Products.Add(prod);
                    }
                    licenseMapModelList.Add(mapModel);
                }
            }
            licDetails.SubscriptionDetails = licenseMapModelList;
            return licDetails;
        }

        // Validating the user Login for the concurrent user.
        public bool ValidateConcurrentUser(int teamId, string userId)
        {
            _teamLogic.UserManager = UserManager;
            bool status = _teamLogic.AllowTeamMemberLogin(teamId, userId);
            return status;
        }

        // Assign team License to the Concurrent user whop has logged In.
        public void UpdateTeamLicenseToUser(int teamId, string userId)
        {
            licLogic.AssignTeamLicenseToUser(teamId, userId);
        }
    }
}
