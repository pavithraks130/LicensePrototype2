using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseServer.Logic;
using LicenseServer.DataModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace License.MetCalWeb.Controllers
{
    public class CartController : BaseController
    {

        CartLogic logic = null;

        public CartController()
        {
            logic = new CartLogic();
        }

        public ActionResult CartItem()
        {
            if (LicenseSessionState.Instance.User == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            var obj = logic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);
            ViewData["TotalAmount"] = logic.TotalAmount;
            return View(obj);
        }

        public void Purchase()
        {
            var obj = logic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);
            UserSubscriptionLogic subscriptionlogic = new UserSubscriptionLogic();
            List<UserSubscription> subsList = new List<UserSubscription>();
            foreach (var item in obj)
            {

                UserSubscription usersubs = new UserSubscription();
                usersubs.UserId = LicenseSessionState.Instance.User.ServerUserId;
                usersubs.SubscriptionTypeId = item.SubscriptionTypeId;
                usersubs.SubscriptionDate = DateTime.Now.Date;
                usersubs.Quantity = item.Quantity;
                subsList.Add(usersubs);
            }

            UserSubscriptionList userSubscriptionList = subscriptionlogic.CreateUserSubscription(subsList, LicenseSessionState.Instance.User.ServerUserId);
            UpdateSubscriptionOnpremise(userSubscriptionList);
            foreach (var item in obj)
            {
                item.IsPurchased = true;
                logic.UpdateCartItem(item);
            }
        }


        private void UpdateSubscriptionOnpremise(UserSubscriptionList subs)
        {
            string userId = string.Empty;
            if (LicenseSessionState.Instance.User.ServerUserId != subs.UserId)
            {
                License.Logic.ServiceLogic.UserLogic userLogic = new License.Logic.ServiceLogic.UserLogic();
                userLogic.UserManager = Request.GetOwinContext().GetUserManager<License.Core.Manager.AppUserManager>();
            }
            else
                userId = LicenseSessionState.Instance.User.UserId;
            foreach (var subDtls in subs.SubscriptionList)
            {
                License.Model.Subscription subsModel = new Model.Subscription();
                subsModel.Id = subDtls.SubscriptionType.Id;
                subsModel.SubscriptionName = subDtls.SubscriptionType.Name;

                License.Logic.ServiceLogic.SusbscriptionLogic subLogic = new Logic.ServiceLogic.SusbscriptionLogic();
                subLogic.CreateSubscription(subsModel);

                License.Model.UserSubscription userSubscription = new Model.UserSubscription();
                userSubscription.SubscriptionDate = subDtls.SubscriptionDate;
                userSubscription.SubscriptionId = subDtls.SubscriptionTypeId;
                userSubscription.UserId = userId;

                License.Logic.ServiceLogic.UserSubscriptionLogic userSubscriptionLogic = new Logic.ServiceLogic.UserSubscriptionLogic();
                int userSubscriptionId = userSubscriptionLogic.CreateSubscription(userSubscription);

                List<License.Model.Product> productList = new List<License.Model.Product>();
                List<License.Model.ProductSubscriptionMapping> mappingList = new List<Model.ProductSubscriptionMapping>();
                foreach (var pro in subDtls.Products)
                {
                    License.Model.Product prod = new License.Model.Product();
                    prod.Id = pro.Id;
                    prod.Name = pro.Name;
                    prod.Description = pro.Description;
                    prod.ProductCode = pro.ProductCode;
                    productList.Add(prod);

                    License.Model.ProductSubscriptionMapping mapping = new Model.ProductSubscriptionMapping();
                    mapping.ProductId = pro.Id;
                    mapping.SubscriptionId = subDtls.SubscriptionTypeId;
                    mappingList.Add(mapping);
                }

                License.Logic.ServiceLogic.ProductLogic productLogic = new Logic.ServiceLogic.ProductLogic();
                productLogic.CreateProduct(productList);

                License.Logic.ServiceLogic.ProductSubscriptionMappingLogic prodSubsMapLogic = new Logic.ServiceLogic.ProductSubscriptionMappingLogic();
                prodSubsMapLogic.Create(mappingList);

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
                licenseLogic.Save();
            }

            //License.Model.UserSubscription subscription = new Model.UserSubscription();
            //subscription.UserId = LicenseSessionState.Instance.User.UserId;
            //subscription.SubscriptionDate = subs.SubscriptionDate;
            //subscription.SubscriptionId = subs.SubscriptionTypeId;
            //License.Logic.ServiceLogic.UserSubscriptionLogic logic = new Logic.ServiceLogic.UserSubscriptionLogic();
            //int subscriptionId = logic.CreateSubscription(subscription);
            //License.Logic.ServiceLogic.LicenseLogic licLogic = new Logic.ServiceLogic.LicenseLogic();
            //if (subscriptionId > 0)
            //{

            //    foreach (var str in subs.LicenseKeys)
            //    {
            //        License.Model.LicenseData data = new Model.LicenseData();
            //        data.AdminUserId = LicenseSessionState.Instance.User.UserId;
            //        data.LicenseKey = str.LicenseKey;
            //        data.UserSubscriptionId = subscriptionId;
            //        licLogic.CreateLicenseData(data);
            //    }
            //    if (subs.LicenseKeys.Count > 0)
            //        licLogic.Save();
            //}
        }
        public ActionResult RemoveItem(int id)
        {
            logic.DeleteCartItem(id);
            return RedirectToAction("CartItem", "Cart");
        }


        public ActionResult PaymentGateway()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DoPayment()
        {
            Purchase();
            return View();
        }

    }
}