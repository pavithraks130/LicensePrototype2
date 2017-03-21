using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LicenseServer.DataModel;

namespace License.MetCalWeb.Models
{
    public class CartItemModel
    {
        public CartItem ModelCartItem;
        public CartItemModel()
        {
            ModelCartItem = new CartItem();

        }

        public int Id
        {
            get { return ModelCartItem.Id; }
        }

        public int SubscriptionTypeId
        {
            get { return ModelCartItem.SubscriptionTypeId; }
            set { ModelCartItem.SubscriptionTypeId = value; }
        }

        public DateTime DateCreated
        {
            get { return ModelCartItem.DateCreated; }
            set { ModelCartItem.DateCreated = value; }
        }

        public int Quantity
        {
            get { return ModelCartItem.Quantity; }
            set { ModelCartItem.Quantity = value; }
        }
    }
}