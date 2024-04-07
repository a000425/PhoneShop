using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MP.Models;
using MP.Dtos;


namespace MP.Services
{
    public class CartService
    {
        private readonly PhoneContext _phoneContext;
        public CartService(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }

        public string AddCart(string user,int id,int num)
        {
            var item = _phoneContext.Item.SingleOrDefault(i => i.ItemId == id);
            if (item != null) 
            {
                var format = _phoneContext.Format.SingleOrDefault(f => f.FormatId == item.FormatId);
                if (format != null)
                {
                    if (num <= format.Store)
                    {
                        Cart cart = new Cart 
                        {
                            Account = user,
                            ItemId = id,
                            ItemNum = num,
                            AddTime = DateTime.Now
                        };
                        _phoneContext.Cart.Add(cart);
                        _phoneContext.SaveChanges();
                        return "加入成功";
                    }
                    else 
                    {
                        return "商品剩餘數量不足";
                    }
                }
                else 
                {
                    return "無法加入購物車2";
                }
            }
            else 
            {
                return "無法加入購物車1";
            }
           
        }

        public IEnumerable<CartDto> GetAllCartList(string userAccount)
        {
            var shoppingCartItems = (from cart in _phoneContext.Cart
                             join item in _phoneContext.Item on cart.ItemId equals item.ItemId
                             join format in _phoneContext.Format on item.FormatId equals format.FormatId
                             where cart.Account == userAccount
                             select new CartDto
                             {
                                 ItemName = item.ItemName,
                                 Color = format.Color,
                                 Space = format.Space,
                                 ItemNum = cart.ItemNum,
                                 ItemPrice = item.ItemPrice
                             }).ToList();

            return shoppingCartItems;
        }
        public bool DeleteCart(int id, string userAccount)
        {
            var cart = (from a in _phoneContext.Cart where a.Id == id && a.Account ==userAccount select a).FirstOrDefault();
            if(cart!=null)
            {
                _phoneContext.Cart.Remove(cart);
                _phoneContext.SaveChanges();
                return true;
            }else
            {
                return false;
            }
        }
    }
}
