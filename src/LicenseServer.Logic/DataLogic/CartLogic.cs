using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;

namespace LicenseServer.Logic
{
    public class CartLogic : BaseLogic
    {


        public List<CartItem> GetCartItems(string userId)
        {
            List<CartItem> cartItems = new List<CartItem>();
            var cartItemObj = Work.CartItemLicenseRepository.GetData(src => src.UserId == userId && src.IsPurchased == false);
            foreach (var obj in cartItemObj)
                cartItems.Add(AutoMapper.Mapper.Map<Core.Model.CartItem, CartItem>(obj));
            return cartItems;
        }

        public CartItem CreateCartItem(CartItem item)
        {
            Core.Model.CartItem cartItem = AutoMapper.Mapper.Map<CartItem, Core.Model.CartItem>(item);
            var obj = Work.SubscriptionRepository.GetById(item.SubscriptionId);
            cartItem.Price = obj.Price;
            cartItem = Work.CartItemLicenseRepository.Create(cartItem);
            Work.CartItemLicenseRepository.Save();
            return AutoMapper.Mapper.Map<CartItem>(cartItem);
        }

        public CartItem UpdateCartItem(CartItem item)
        {
            Core.Model.CartItem cartItem = Work.CartItemLicenseRepository.GetById(item.Id);
            cartItem.Quantity = item.Quantity;
            cartItem.IsPurchased = item.IsPurchased;
            cartItem.Price = item.Price;
            cartItem = Work.CartItemLicenseRepository.Update(cartItem);
            Work.CartItemLicenseRepository.Save();
            return AutoMapper.Mapper.Map<CartItem>(cartItem);
        }

        public CartItem GetCartItemById(int id)
        {
            var obj = Work.CartItemLicenseRepository.GetById(id);
            return AutoMapper.Mapper.Map<Core.Model.CartItem, CartItem>(obj);
        }

        public CartItem DeleteCartItem(int id)
        {
            var cartItem = Work.CartItemLicenseRepository.GetById(id);
            cartItem = Work.CartItemLicenseRepository.Delete(cartItem);
            Work.CartItemLicenseRepository.Save();
            return AutoMapper.Mapper.Map<CartItem>(cartItem);
        }
    }
}
