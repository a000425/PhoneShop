using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MP.Models;
using MP.Dtos;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using MP.Repository;


namespace MP.Services
{
    public class CartService
    {
        private readonly PhoneContext _phoneContext;
        private readonly CartRepository _repository;
        public CartService(PhoneContext phoneContext,CartRepository repository)
        {
            _phoneContext = phoneContext;
            _repository = repository;
        }

        public string AddCart(string user,int ItemId,int num,int formatId)
        {
            var item = _phoneContext.Item.SingleOrDefault(i => i.ItemId == ItemId);
            if (item != null) 
            {
                var format = _phoneContext.Format.SingleOrDefault(f => f.ItemId == item.ItemId && f.FormatId == formatId);
                if (format != null)
                {
                    if (num <= format.Store)
                    {
                        Cart cart = new Cart 
                        {
                            Account = user,
                            ItemId = ItemId,
                            FormatId = formatId,
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
                    return "查無此規格";
                }
            }
            else 
            {
                return "查無此商品";
            }
           
        }

        public IEnumerable<CartDto> GetAllCartList(string userAccount)
        {
            var shoppingCartItems = (from cart in _phoneContext.Cart
                             join item in _phoneContext.Item on cart.ItemId equals item.ItemId
                             join format in _phoneContext.Format on cart.FormatId equals format.FormatId
                             where cart.Account == userAccount
                             select new CartDto
                             {
                                 cartId = cart.Id,
                                 ItemName = item.ItemName,
                                 Color = format.Color,
                                 Space = format.Space,
                                 ItemNum = cart.ItemNum,
                                 ItemPrice = format.ItemPrice
                             }).ToList();

            return shoppingCartItems;
        }
        public bool DeleteCart(int id, string userAccount)
        {
            var cart = (from a in _phoneContext.Cart where a.ItemId == id && a.Account ==userAccount select a).FirstOrDefault();
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
        #region 下訂單
        public string getOrder(string account,string address){
            try{
                var Cart = _repository.GetCarts(account);
                if(_repository.AddOrder(Cart,account,address)){
                    _repository.AddOrderItem(Cart);
                    _repository.DeleteCart(account);
                }
            }catch(Exception e){
                throw new Exception(e.ToString());
            }
            
            return "下單成功";
        }
        #endregion
    }
}