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
    public class LicenseBO
    {
        UserLicenseRequestLogic userLicenseRequestLogic = null;
        UserLicenseLogic licLogic = null;
        UserLogic userLogic = null;

        public AppUserManager UserManager { get; set; }
        public AppRoleManager RoleManager { get; set; }
        public string ErrorMessage { get; set; }

        public LicenseBO()
        {
            userLicenseRequestLogic = new UserLicenseRequestLogic();
            licLogic = new UserLicenseLogic();
            userLogic = new UserLogic();
        }


        public void ApproveOrRejectLicense(List<UserLicenseRequest> licReqList)
        {
            List<UserLicenseRequest> licenseRequestList = new List<UserLicenseRequest>();
            List<UserLicense> userLicenseList = new List<UserLicense>();
            foreach (var licReq in licReqList)
            {
                var userlicReq = userLicenseRequestLogic.GetById(licReq.Id);
                userlicReq.Comment = licReq.Comment;
                userlicReq.ApprovedBy = licReq.ApprovedBy;
                userlicReq.IsApproved = licReq.IsApproved;
                userlicReq.IsRejected = licReq.IsRejected;
                licenseRequestList.Add(userlicReq);
                if (userlicReq.IsApproved)
                {
                    UserLicense lic = new UserLicense();
                    lic.UserId = userlicReq.Requested_UserId;
                    lic.License = new LicenseData();
                    lic.License.ProductId = userlicReq.ProductId;
                    lic.License.UserSubscriptionId = userlicReq.UserSubscriptionId;
                    userLicenseList.Add(lic);
                }
            }
            userLicenseRequestLogic.Update(licenseRequestList);
            ErrorMessage = userLicenseRequestLogic.ErrorMessage;
            if (userLicenseList.Count > 0 && String.IsNullOrEmpty(ErrorMessage))
            {
                licLogic.CreataeUserLicense(userLicenseList);
                ErrorMessage = licLogic.ErrorMessage;
            }
        }

        public UserLicenseDetails GetUserLicenseSubscriptionDetails(FetchUserSubscription model)
        {
            UserLicenseDetails licDetails = new UserLicenseDetails();
            var licenseMapModelList = new List<SubscriptionDetails>();
            UserLicenseLogic logic = new UserLicenseLogic();
            ProductSubscriptionLogic proSubLogic = new ProductSubscriptionLogic();
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;

            licDetails.User = userLogic.GetUserById(model.UserId);

            List<UserLicense> data = null;
            if (model.TeamId == 0)
                data = logic.GetUserLicense(model.UserId);
            else
                data = logic.GetUserLicense(model.UserId, model.TeamId);

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
                    SubscriptionDetails mapModel = new SubscriptionDetails();
                    mapModel.Name = subs.Name;
                    mapModel.UserSubscriptionId = data.FirstOrDefault(us => us.License.Subscription.SubscriptionId == subs.Id).License.UserSubscriptionId;
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
                        ProductDetails prod = new ProductDetails()
                        {
                            Id = pro.Id,
                            Name = pro.Name,
                            ExpireDate = licExpireData
                        };
                        if (model.IsFeatureRequired)
                        {
                            foreach (var fet in pro.AssociatedFeatures)
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
    }
}
