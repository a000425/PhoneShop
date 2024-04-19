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
        #region 加入購物車
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
        #endregion
        #region 顯示購物車內的商品資訊
        public IEnumerable<CartDto> GetAllCartList(string userAccount)
        {
            var shoppingCartItems = (from cart in _phoneContext.Cart
                             join item in _phoneContext.Item on cart.ItemId equals item.ItemId
                             join format in _phoneContext.Format on cart.FormatId equals format.FormatId
                             where cart.Account == userAccount
                             select new CartDto
                             {
                                 ItemName = item.ItemName,
                                 Color = format.Color,
                                 Space = format.Space,
                                 ItemNum = cart.ItemNum,
                                 ItemPrice = format.ItemPrice
                             }).ToList();

            return shoppingCartItems;
        }
        #endregion
        #region 刪除單筆商品
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
        #endregion
        #region 下訂單
        public string getOrder(string account,string address){
            try{
                var Cart = _repository.GetCarts(account);
                if(Cart.Any())
                {
                    if(_repository.AddOrder(Cart,account,address)){
                    _repository.AddOrderItem(Cart);
                    _repository.DeleteCart(account);
                    }
                }else
                {
                    return "購物車內無商品";
                }
                
            }catch(Exception e){
                throw new Exception(e.ToString());
            }
            
            return "下單成功";
        }
        #endregion
        #region 顯示訂單資訊
        public IEnumerable<OrderItemDto> GetOrderItem(string account){
            var getOrderItem = _repository.GetOrderItem(account);
            return getOrderItem;
        }
        #endregion
        #region 訂單查詢
        public IEnumerable<OrderInfoDto> OrderDetail(string account,int id)
        {
            var detail = _repository.OrderDetail(account,id);
            return detail;
        }
        #endregion
        #region 更改訂單狀態至已完成
        public string OrderFinish(int orderId, string account)
        {
            var result = _repository.OrderStatusFinish(orderId,account);
            return result;
        }
        #endregion
        #region 個人資料
        public ProfileDto Profile(string account)
        {
            var profile = _repository.Profile(account);
            return profile;
        }
        #endregion
    }
}