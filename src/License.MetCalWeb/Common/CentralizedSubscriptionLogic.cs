using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Common
{
    public class CentralizedSubscriptionLogic
    {
        public static void UpdateUserSubscription(List<LicenseServer.DataModel.UserSubscription> subsList)
        {
            LicenseServer.Logic.UserSubscriptionLogic subscriptionlogic = new LicenseServer.Logic.UserSubscriptionLogic();
            LicenseServer.DataModel.UserSubscriptionList userSubscriptionList = subscriptionlogic.CreateUserSubscription(subsList, LicenseSessionState.Instance.User.ServerUserId);
            UpdateSubscriptionOnpremise(userSubscriptionList);
        }

        private static void UpdateSubscriptionOnpremise(LicenseServer.DataModel.UserSubscriptionList subs)
        {
            string userId = string.Empty;
            if (LicenseSessionState.Instance.User.ServerUserId != subs.UserId)
            {
                License.Logic.ServiceLogic.UserLogic userLogic = new License.Logic.ServiceLogic.UserLogic();
            }
            else
                userId = LicenseSessionState.Instance.User.UserId;

            foreach (var subDtls in subs.SubscriptionList)
            {
                //Code to save the Subscription Data with Product in to file.
                //Begin
                List<License.Model.Product> productList = new List<License.Model.Product>();
                foreach (var pro in subDtls.Products)
                {
                    License.Model.Product prod = new License.Model.Product();
                    prod.Id = pro.Product.Id;
                    prod.Name = pro.Product.Name;
                    prod.Description = pro.Product.Description;
                    prod.ProductCode = pro.Product.ProductCode;
                    prod.QtyPerSubscription = pro.QtyPerSubscription;
                    prod.Features = new List<Model.Feature>();
                    foreach (var f in pro.Product.AssociatedFeatures)
                    {
                        var feture = new License.Model.Feature();
                        feture.Id = f.Id;
                        feture.Name = f.Name;
                        feture.Description = f.Description;
                        feture.Version = f.Version;
                        prod.Features.Add(feture);
                    }
                    productList.Add(prod);
                }

                License.Model.Subscription subsModel = new Model.Subscription();
                subsModel.Id = subDtls.SubscriptionType.Id;
                subsModel.SubscriptionName = subDtls.SubscriptionType.Name;
                subsModel.Product = productList;

                Logic.ServiceLogic.ProductSubscriptionLogic proSubLogic = new Logic.ServiceLogic.ProductSubscriptionLogic();
                proSubLogic.SaveToFile(subsModel);
                //End

                //Code to save the user Subscription details to Database.
                License.Model.UserSubscription userSubscription = new Model.UserSubscription();
                userSubscription.SubscriptionDate = subDtls.SubscriptionDate;
                userSubscription.SubscriptionId = subDtls.SubscriptionTypeId;
                userSubscription.UserId = userId;
                userSubscription.Quantity = subDtls.OrderdQuantity;

                License.Logic.ServiceLogic.UserSubscriptionLogic userSubscriptionLogic = new Logic.ServiceLogic.UserSubscriptionLogic();
                int userSubscriptionId = userSubscriptionLogic.CreateSubscription(userSubscription);


                List<License.Model.LicenseData> licenseDataList = new List<Model.LicenseData>();
                foreach (var lic in subDtls.LicenseKeyProductMapping)
                {
                    License.Model.LicenseData licenseData = new Model.LicenseData();
                    licenseData.LicenseKey = lic.LicenseKey;
                    licenseData.ProductId = lic.ProductId;
                    licenseData.UserSubscriptionId = userSubscriptionId;
                    licenseDataList.Add(licenseData);
                }
                License.Logic.ServiceLogic.LicenseLogic licenseLogic = new Logic.ServiceLogic.LicenseLogic();
                licenseLogic.CreateLicenseData(licenseDataList);
            }
        }
    }
}