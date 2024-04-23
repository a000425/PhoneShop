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

        public bool AddOrder(List<Cart> carts,string account,string address){
           
            decimal totalPrice = 0;
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
                    totalPrice += price * itemnum; 
                }

                var order = new Order
                {
                    Account = account,
                    TotalPrice = (int)totalPrice, 
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
        #region 清空購物車
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
        #endregion
        #region 訂單資訊
        public IEnumerable<OrderItemDto> GetOrderItem(string account)
        {
            var getOrderItem = (from o in _phoneContext.Order 
                                where o.Account == account 
                                select new OrderItemDto
                                {
                                    OrderId = o.OrderId,
                                    OrderTime = o.OrderTime,
                                    TotalPrice = o.TotalPrice,
                                    OrderStatus = o.OrderStatus
                                }).ToList().OrderByDescending(o => o.OrderId);
            return getOrderItem;
        }
        #endregion
        #region 訂單查詢
        public IEnumerable<OrderInfoDto> OrderDetail(string account,int id)
        {
                var Detail = (from order in _phoneContext.Order
                          join orderitem in _phoneContext.OrderItem
                          on order.OrderId equals orderitem.OrderId
                          join format in _phoneContext.Format
                          on orderitem.FormatId equals format.FormatId 
                          join item in _phoneContext.Item
                          on orderitem.ItemId equals item.ItemId
                          join Account in _phoneContext.Account
                          on order.Account equals Account.Account1
                          where order.OrderId==id && order.Account==account
                          select new OrderInfoDto
                          {
                            Brand = format.Brand,
                            ItemName = item.ItemName,
                            Color = format.Color,
                            Space = format.Space,
                            ItemNum = orderitem.ItemNum,
                            Name = Account.Name,
                            Cellphone = Account.Cellphone,
                            Email = Account.Email,
                            Address = order.Address,
                          }).ToList();
            return Detail;
        }
        #endregion
        #region 個人資料
        public ProfileDto Profile(string account)
        {
            var profile = (from a in _phoneContext.Account
                           where a.Account1==account
                           select new ProfileDto
                           {
                            Account1 = a.Account1,
                            Name = a.Name,
                            Cellphone = a.Cellphone,
                            Email = a.Email
                           }).Distinct().FirstOrDefault();
            return profile;
        }
        #endregion
        #region 訂單狀態更改為已完成
        public string OrderStatusFinish(int orderId, string account)
        {
            var order =  _phoneContext.Order.SingleOrDefault(o => o.OrderId == orderId && o.Account == account);
            if(order!=null)
            {
                if(order.OrderStatus == "已出貨")
                {
                    order.OrderStatus = "已完成";
                    _phoneContext.SaveChanges();
                    return("訂單已完成");
                }else
                {
                    return("訂單狀態異常");
                }
                
            }else
            {
                return("未找到此訂單");
            }
            
        }
        #endregion
    }
}
