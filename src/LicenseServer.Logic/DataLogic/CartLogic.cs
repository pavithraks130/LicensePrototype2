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
        public double TotalAmount { get; set; }


        public List<CartItem> GetCartItems(string userId)
        {
            TotalAmount = 0;
            List<CartItem> cartItems = new List<CartItem>();
            var cartItemObj = Work.CartItemLicenseRepository.GetData(src => src.UserId == userId && src.IsPurchased == false);
            foreach (var obj in cartItemObj)
            {
                TotalAmount += (obj.Quantity * obj.Price);
                cartItems.Add(AutoMapper.Mapper.Map<Core.Model.CartItem, CartItem>(obj));
            }
            return cartItems;
        }

        public bool CreateCartItem(CartItem item)
        {
            Core.Model.CartItem cartItem = AutoMapper.Mapper.Map<CartItem, Core.Model.CartItem>(item);
            var obj = Work.SubscriptionRepository.GetById(item.SubscriptionTypeId);
            cartItem.Price = obj.Price;
            cartItem = Work.CartItemLicenseRepository.Create(cartItem);
            Work.CartItemLicenseRepository.Save();
            return cartItem.Id > 0;
        }

        public bool UpdateCartItem(CartItem item)
        {
            Core.Model.CartItem cartItem = Work.CartItemLicenseRepository.GetById(item.Id);
            cartItem.IsPurchased = item.IsPurchased;
            cartItem = Work.CartItemLicenseRepository.Update(cartItem);
            Work.CartItemLicenseRepository.Save();
            return cartItem.Id > 0;
        }

        public CartItem GetCartItemById(int id)
        {
            var obj = Work.CartItemLicenseRepository.GetById(id);
            return AutoMapper.Mapper.Map<Core.Model.CartItem, CartItem>(obj);
        }

        public bool DeleteCartItem(int id)
        {
            var obj = Work.CartItemLicenseRepository.Delete(id);
            Work.CartItemLicenseRepository.Save();
            return obj;
        }

       
    }
}
