using Microsoft.EntityFrameworkCore;
using MP.Dtos;
using MP.Models;

namespace MP.Repository
{
    public class CartRepository
    {
        private readonly PhoneContext _phoneContext;
        public CartRepository(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }
        public bool AddOrder(string account,string address){
            try{
                var order = new Order{
                    Account = account,
                    TotalPrice = 0,
                    OrderTime = DateTime.Now,
                    OrderStatus = "未出貨",
                    Address = address
                    };
                _phoneContext.Order.Add(order);
                _phoneContext.SaveChanges();
            }
            catch{
                return false;
            }
            return true;
        }
        public void AddOrderItem(CartDto cartDto){
            try{
                foreach(var item in cartDto.Items){
                    var orderItem = new OrderItem{
                        OrderId = _phoneContext.Order
                                    .OrderByDescending(o=>o.OrderTime)
                                    .Select(o=>o.OrderId)
                                    .FirstOrDefault(),
                        ItemId = (from a in _phoneContext.Item
                                where a.ItemName == cartDto.ItemName
                                select a.ItemId).FirstOrDefault(),
                        FormatId =  (from a in _phoneContext.Item
                                    join f in _phoneContext.Format on a.ItemId equals f.ItemId
                                    where a.ItemName == cartDto.ItemName && f.Color == cartDto.Color && f.Space == cartDto.Space
                                    select f.FormatId).FirstOrDefault(),
                        ItemNum = cartDto.ItemNum
                    };
                    _phoneContext.OrderItem.Add(orderItem);
                }
                _phoneContext.SaveChanges();
            }
            catch(Exception ex){
                throw new Exception(ex.ToString());
            }
        }
        public void DeleteCart(CartDto cartDto,string account){
            try{
                var itemsToDelete = _phoneContext.Cart.Where(c => c.Account == account).ToList();
                _phoneContext.Cart.RemoveRange(itemsToDelete);
                _phoneContext.SaveChanges();
            }
            catch (Exception ex){
                throw new Exception(ex.ToString());
            }
        }
    }
}
