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
        #region 訂單
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
        #endregion
        #region 訂單項目
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
        #endregion
        #region 清空購物車
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
                            Address = order.Address
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
    }
}
