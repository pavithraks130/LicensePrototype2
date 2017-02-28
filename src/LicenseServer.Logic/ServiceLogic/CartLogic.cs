using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class CartLogic : BaseLogic
    {
        public List<CartItem> GetCartItems(string userId)
        {
            List<CartItem> cartItems = new List<CartItem>();
            var cartItemObj = Work.CartItemLicenseRepository.GetData(src => src.UserId == userId);
            foreach (var obj in cartItemObj)
                cartItems.Add(AutoMapper.Mapper.Map<Core.Model.CartItem, CartItem>(obj));
            return cartItems;
        }

        public bool CreateCartItem(CartItem item)
        {
            var cartitem = AutoMapper.Mapper.Map<CartItem, LicenseServer.Core.Model.CartItem>(item);
            cartitem = Work.CartItemLicenseRepository.Create(cartitem);
            return cartitem.Id > 0;
        }

        public bool DeleteCartItem(int id)
        {
            return Work.CartItemLicenseRepository.Delete(id);
        }
    }
}
