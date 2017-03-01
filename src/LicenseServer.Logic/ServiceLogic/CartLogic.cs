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
            Core.Model.CartItem cartItem = AutoMapper.Mapper.Map<CartItem, Core.Model.CartItem>(item);
            cartItem = Work.CartItemLicenseRepository.Create(cartItem);
            Work.ProductRepository.Save();
            return cartItem.Id > 0;
        }

        public bool DeleteCartItem(int id)
        {
            return Work.CartItemLicenseRepository.Delete(id);
        }
    }
}
