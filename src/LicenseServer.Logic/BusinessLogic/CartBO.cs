using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;
using LicenseServer.Core.Manager;

namespace LicenseServer.Logic.BusinessLogic
{
    public class CartBO
    {
        public string ErrorMessage { get; set; }
        public LicUserManager UserManager { get; set; }
        public LicRoleManager RoleManager { get; set; }
        public PurchaseOrder OfflinePayment(string userId)
        {
            CartLogic cartLogic = new CartLogic();
            PurchaseOrderLogic orderLogic = new PurchaseOrderLogic();
            PurchaseOrder order = new PurchaseOrder();
            try
            {
                var items = cartLogic.GetCartItems(userId);
                if (items.Count == 0) return null;

                order.UserId = userId;
                order.CreatedDate = DateTime.Now.Date;
                order.PurchaseOrderNo = orderLogic.CreatePONumber();
                var obj = orderLogic.CreatePurchaseOrder(order);

                if (obj != null)
                {
                    List<PurchaseOrderItem> poItemList = new List<PurchaseOrderItem>();
                    foreach (var item in items)
                    {
                        PurchaseOrderItem poItem = new PurchaseOrderItem();
                        poItem.Quantity = item.Quantity;
                        poItem.SubscriptionId = item.SubscriptionTypeId;
                        poItemList.Add(poItem);
                    }
                    if (poItemList.Count > 0)
                    {
                        POItemLogic itemLogic = new POItemLogic();
                        itemLogic.CreateItem(poItemList, obj.Id);
                    }
                    foreach(var item in items)
                    {
                        item.IsPurchased = true;
                        cartLogic.UpdateCartItem(item);
                    }
                    
                }
                return obj;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return null;
        }

        public SubscriptionList OnlinePayment(string userId)
        {
            List<UserSubscription> subsList = new List<UserSubscription>();
            CartLogic cartLogic = new CartLogic();
            cartLogic.UserManager = UserManager;
            cartLogic.RoleManager = RoleManager;
            UserSubscriptionLogic userSubLogic = new UserSubscriptionLogic();
            userSubLogic.UserManager = UserManager;
            userSubLogic.RoleManager = RoleManager;
            var cartItems = cartLogic.GetCartItems(userId);
            if (cartItems.Count > 0)
            {
                foreach (CartItem item in cartItems)
                {
                    UserSubscription usersubs = new UserSubscription();
                    usersubs.UserId = userId;
                    usersubs.SubscriptionTypeId = item.SubscriptionTypeId;
                    usersubs.SubscriptionDate = DateTime.Now.Date;
                    usersubs.Quantity = item.Quantity;
                    subsList.Add(usersubs);
                }
                var dataList = userSubLogic.CreateUserSubscriptionList(subsList, userId);
                foreach (var item in cartItems)
                {
                    item.IsPurchased = true;
                    cartLogic.UpdateCartItem(item);
                }
                return dataList;
            }
            else
            {
                ErrorMessage = cartLogic.ErrorMessage;
                return null;
            }


        }

        public SubscriptionList SyncPurchaseOrder(string userId)
        {

            UserSubscriptionLogic userSubLogic = new UserSubscriptionLogic();
            List<UserSubscription> subsList = new List<UserSubscription>();
            PurchaseOrderLogic logic = new PurchaseOrderLogic();
            SubscriptionList userSsubList = new SubscriptionList();

            userSubLogic.UserManager = UserManager;
            userSubLogic.RoleManager = RoleManager;

            userSsubList.UserId = userId;
            var poList = logic.GetPOToBeSynchedByUser(userId);
            foreach (var poItem in poList)
            {
                foreach (var item in poItem.OrderItems)
                {
                    LicenseServer.DataModel.UserSubscription usersubs = new LicenseServer.DataModel.UserSubscription();
                    usersubs.UserId = userId;
                    usersubs.SubscriptionTypeId = item.SubscriptionId;
                    usersubs.SubscriptionDate = DateTime.Now.Date;
                    usersubs.Quantity = item.Quantity;
                    subsList.Add(usersubs);
                }
                var dataList = userSubLogic.CreateUserSubscriptionList(subsList, userId);
                poItem.IsSynched = true;
                logic.UpdatePurchaseOrder(poItem.Id, poItem);
                userSsubList.Subscriptions.AddRange(dataList.Subscriptions);
            }
            return userSsubList;
        }
    }
}
