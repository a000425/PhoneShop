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

        #region 修改單筆購物車內商品數量
        public string updateCart(int cartId, string account, int itemnum)
        {
            try
            {
                var cart = (from c in _phoneContext.Cart
                            where c.Account == account && c.Id == cartId
                            select c).FirstOrDefault();
                if(cart != null)
                {
                    var format = (from f in _phoneContext.Format
                               where f.FormatId == cart.FormatId
                               select f).FirstOrDefault();
                    if(format.Store >= itemnum)
                    {
                        if(itemnum >=1)
                        {
                            cart.ItemNum = itemnum;
                            _phoneContext.SaveChanges();
                        }else
                        {
                            return "購物車商品數量不得等於0或小於0";
                        }
                        
                    }else
                    {
                        return "超過庫存數量";
                    }
                    
                    
                }else
                {
                    return "無此購物車項目";
                }
            }
            catch (Exception e){
                throw new Exception(e.ToString());
                return "修改出現錯誤";
            }
            return "修改成功";
        }
        #endregion
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
            double discount = 0;
            decimal totalPrice = 0;
            try
            {
                var member = (from a in _phoneContext.Account where a.Account1 == account select a.MemberKind).FirstOrDefault();
                if(member == "銀級會員")
                {
                    discount = 0.1;
                }else if(member == "金級會員")
                {
                    discount = 0.15;
                }
                foreach (var num in carts)
                {
                    var price = (from cart in _phoneContext.Cart
                                join i in _phoneContext.Item on cart.ItemId equals i.ItemId
                                join format in _phoneContext.Format on cart.FormatId equals format.FormatId
                                where cart.Id == num.Id
                                select format.ItemPrice).FirstOrDefault();

                    var itemnum = (from c in _phoneContext.Cart where c.Id == num.Id select c.ItemNum).FirstOrDefault();
                    var formatnum = (from f in _phoneContext.Format where f.FormatId == num.FormatId select f.Store).FirstOrDefault();
                    

                    if(itemnum<=formatnum)
                    {
                        totalPrice += price * itemnum;
                    }else
                    {
                        return false;
                    }
                     

                }
                int alldis = (int)((int)(totalPrice)*discount);
                totalPrice = (int)(totalPrice-alldis);
                var order = new Order
                {
                    Account = account,
                    TotalPrice = (int)totalPrice, 
                    Discount = alldis,
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
                        ItemNum = (from c in _phoneContext.Cart where c.Id == item.Id select c.ItemNum).FirstOrDefault(),
                        ItemPrice = (from a in _phoneContext.Cart
                                     join b in _phoneContext.Format on a.FormatId equals b.FormatId
                                     where a.Id == item.Id
                                     select b.ItemPrice).FirstOrDefault()
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
                
                var member = (from a in _phoneContext.Account where a.Account1 == account select a.MemberKind).FirstOrDefault();
                double discount = 0;
                if(member == "銀級會員")
                {
                    discount = 0.1;
                }else if(member == "金級會員")
                {
                    discount = 0.15;
                }
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
                            Discount = (int)(discount*format.ItemPrice)
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
                            Email = a.Email,
                            MemberKind = a.MemberKind,
                            MemberTime = a.MemberTime
                           }).Distinct().FirstOrDefault();
            return profile;
        }
        #endregion
        #region 訂單狀態更改為已完成
        public string OrderStatusFinish(int orderId, string account)
        {
            var order =  _phoneContext.Order.SingleOrDefault(o => o.OrderId == orderId && o.Account == account);
            var member =  _phoneContext.Account.SingleOrDefault(a => a.Account1 == account);
            if(order!=null)
            {
                if(order.OrderStatus == "已出貨")
                {
                    order.OrderStatus = "已完成";
                    _phoneContext.SaveChanges();
                    if(member.MemberKind == null || member.MemberKind == "銀級會員")
                    {
                        if(MemberUpgrade(account,member.MemberKind))
                        {
                            return("訂單已完成，達成升級會員標準");
                        }
                    }
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
        #region 會員升級
        public bool MemberUpgrade(string account,string MemberKind)
        {
            var member =  _phoneContext.Account.SingleOrDefault(a => a.Account1 == account);
            DateTime OneYear = DateTime.Now.AddYears(-1);
            var orderyear = _phoneContext.Order.Where(o => o.Account == account && o.OrderTime >= OneYear);
            int allbuy = 0;
            foreach(var item in orderyear)
            {
                allbuy+=item.TotalPrice;
            }
            if(MemberKind == null)
            {
                if(allbuy>=50000)
                {
                    member.MemberKind = "銀級會員";
                    member.MemberTime = DateTime.Now;
                    _phoneContext.SaveChanges();
                    return true;
                }
            }else if(MemberKind == "銀級會員")
            {
                if(allbuy>=100000)
                {
                    member.MemberKind = "金級會員";
                    member.MemberTime = DateTime.Now;
                    _phoneContext.SaveChanges();
                    return true;
                }
            }
            return false;
            
                
            
        }
        #endregion
    }
}
