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
        public List<Cart> GetCarts(string account){
                var Cart = (from a in _phoneContext.Cart
                            where a.Account == account
                            select new Cart{
                                Id = a.Id,
                                ItemId = a.ItemId,
                                ItemNum = a.ItemNum,
                                AddTime = a.AddTime,
                                FormatId = a.FormatId
                            }).ToList(); 
                return Cart;          
        }
        public bool AddOrder(List<Cart>carts,string account,string address){
           
            decimal totalPrice = 0; // 用于存储总价格
            try
            {
                foreach (var num in carts)
                {
                    var price = (from cart in _phoneContext.Cart
                                join i in _phoneContext.Item on cart.ItemId equals i.ItemId
                                join format in _phoneContext.Format on cart.FormatId equals format.FormatId
                                where cart.Id == num.Id
                                select format.ItemPrice).FirstOrDefault();

                    var itemnum = (from c in _phoneContext.Cart where c.Id == num.Id select c.ItemNum).FirstOrDefault();
                    totalPrice += price * itemnum; // 将价格添加到总价格中
                }

                var order = new Order
                {
                    Account = account,
                    TotalPrice = (int)totalPrice, // 将总价格赋值给订单的 TotalPrice 属性
                    OrderTime = DateTime.Now,
                    OrderStatus = "未出貨",
                    Address = address
                };

                _phoneContext.Order.Add(order);
                _phoneContext.SaveChanges();
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return true;
        }
        public void AddOrderItem(List<Cart> carts){
            try{
                foreach(var item in carts){
                    var orderItem = new OrderItem{
                        OrderId = _phoneContext.Order
                                    .OrderByDescending(o=>o.OrderTime)
                                    .Select(o=>o.OrderId)
                                    .FirstOrDefault(),
                        ItemId = (from c in _phoneContext.Cart 
                                    where c.Id == item.Id 
                                    select c.ItemId).FirstOrDefault(),
                        FormatId =  (from c in _phoneContext.Cart 
                                    where c.Id == item.Id 
                                    select c.FormatId).FirstOrDefault(),
                        ItemNum = (from c in _phoneContext.Cart where c.Id == item.Id select c.ItemNum).FirstOrDefault()
                    };
                    _phoneContext.OrderItem.Add(orderItem);
                    var format = _phoneContext.Format.SingleOrDefault(f => f.FormatId == orderItem.FormatId);
                    if(format != null)
                    {
                        format.Store = format.Store - orderItem.ItemNum;
                        
                    }

                }
                _phoneContext.SaveChanges();
            }
            catch(Exception ex){
                throw new Exception(ex.ToString());
            }
        }
        public void DeleteCart(string account){
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
