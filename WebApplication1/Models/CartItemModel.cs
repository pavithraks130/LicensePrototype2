using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class CartItemModel
    {
        public Model.Model.CartItem ModelCartItem;
        public CartItemModel()
        {
            ModelCartItem = new Model.Model.CartItem();

        }

        public int Id
        {
            get { return ModelCartItem.Id; }
        }

        public int ProductId
        {
            get { return ModelCartItem.ProductId; }
            set { ModelCartItem.ProductId = value; }
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