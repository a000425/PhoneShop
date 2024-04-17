using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MP.Dtos;
using MP.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MP.Repository
{
    public class BackRepository
    {
        private readonly PhoneContext _phoneContext;
        public BackRepository(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }
        public bool AddItem(ItemDto itemDto){
            try{
                var exist = _phoneContext.Item.Any(x => x.ItemName==itemDto.ItemName);
                if (!exist){
                var Item = new Item {
                ItemName = itemDto.ItemName,
                Instruction = itemDto.Instruction,
                IsAvailable = false,
                CreateTime = DateTime.Now
            };
            _phoneContext.Item.Add(Item);
            _phoneContext.SaveChanges();
            }}
            catch{
                return false;
            }
            return true;
        }
        public void AddFormat(ItemDto itemDto){
            try{
                itemDto.ItemId = (from a in _phoneContext.Item
                          where a.ItemName == itemDto.ItemName
                          select a.ItemId).FirstOrDefault();
                var format = new Models.Format {
                Brand = itemDto.Brand,
                Color = itemDto.Color,
                ItemPrice = itemDto.ItemPrice,
                Store = itemDto.Store,
                Space = itemDto.Space,
                ItemId = itemDto.ItemId
                };
                _phoneContext.Format.Add(format);
                _phoneContext.SaveChanges();
            }
            catch(Exception e){
                throw new Exception(e.ToString());
            }
        }
        public void AddImg(ItemDto itemDto){
            try{
                var img = new Models.Img {
                    FormatId = (from a in _phoneContext.Format
                                where a.ItemId == itemDto.ItemId && a.Space == itemDto.Space && a.Color == itemDto.Color
                                select a.FormatId).FirstOrDefault(),
                    ItemImg = itemDto.ItemImg
                };
                _phoneContext.Img.Add(img);
                _phoneContext.SaveChanges();
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
        }

        public string UPItem(int ItemId){
            try{
                var item = (from a in _phoneContext.Item
                            where a.ItemId == ItemId
                            select a).SingleOrDefault();
                item.IsAvailable = true;
                _phoneContext.Update(item);
                _phoneContext.SaveChanges();
            }
            catch{
                return "上架失敗";
            }
            if(!CheckStore(ItemId))
                return "上架失敗";
            return "上架成功";
        }
            public string DownItem(int ItemId){
            try{
                var item = (from a in _phoneContext.Item
                            where a.ItemId == ItemId
                            select a).SingleOrDefault();
                item.IsAvailable = false;
                _phoneContext.Update(item);
                _phoneContext.SaveChanges();
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return "下架成功";
            
        }
        public bool CheckStore(int ItemId){
            var item = (from a in _phoneContext.Format
                        where a.ItemId == ItemId
                        select a.Store).Sum();
            if(item > 0){
                return true;
            }
            return false;
            
        }
        #region 取得未回覆留言
        public IEnumerable<BackQAUnreplyDto> GetQaUnreply()
        {
            IEnumerable<BackQAUnreplyDto> result;
            try
            {
                result = (from q in _phoneContext.QA
                          join i in _phoneContext.Item on q.ItemId equals i.ItemId
                          where q.ReplyTime == null
                          select new BackQAUnreplyDto
                          {
                              Id = q.Id,
                              ItemName = i.ItemName,
                              Account = q.Account,
                              Content = q.Content,
                              CreateTime = q.CreateTime
                          });
                          
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return result;
        }
        #endregion
        #region 取得已回覆留言
        public IEnumerable<BackQAReplyDto> GetQaReply()
        {
            IEnumerable<BackQAReplyDto> result;
            try
            {
                 result = (from q in _phoneContext.QA
                          join i in _phoneContext.Item on q.ItemId equals i.ItemId
                          where q.ReplyTime != null 
                          select new BackQAReplyDto
                          {
                              ItemName = i.ItemName,
                              Account = q.Account,
                              Content = q.Content,
                              CreateTime = q.CreateTime,
                              Reply = q.Reply,
                              ReplyTime = q.ReplyTime
                          });
                          
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return result;
        }
        #endregion
        #region 回覆QA
        public string ReplyQa(int QAId, string Reply)
        {
            var Q = _phoneContext.QA.SingleOrDefault(q => q.Id == QAId);
            if (Q != null)
            {
                if (Q.ReplyTime == null)
                {
                    Q.Reply = Reply;
                    Q.ReplyTime = DateTime.Now;
                    _phoneContext.SaveChanges();
                    return "回覆成功";
                }
                else 
                {
                    return "此問題已回覆";
                }
            }
            else
            {
                return "查無此問題";
            }
            
        }
        #endregion
        #region 取得所有未出貨訂單
        public IEnumerable<BackOrderShowDto> getOrderUnsend()
        {   string orderstatus = "未出貨";
            IEnumerable<BackOrderShowDto> order = getOrder(orderstatus);
            return order;
        }
        #endregion
        #region 取得所有已出貨訂單
        public IEnumerable<BackOrderShowDto> getOrderSent()
        {   string orderstatus = "已出貨";
            IEnumerable<BackOrderShowDto> order = getOrder(orderstatus);
            return order;
        }
        #endregion
        #region 取得所有已完成訂單
        public IEnumerable<BackOrderShowDto> getOrderFinish()
        {   string orderstatus = "已完成";
            IEnumerable<BackOrderShowDto> order = getOrder(orderstatus);
            return order;
        }
        #endregion
        #region 抓所有訂單
        public IEnumerable<BackOrderShowDto> getOrder(string orderstatus)
        {   
            IEnumerable<BackOrderShowDto> order;
            try
            {
                order = (from o in _phoneContext.Order
                join oi in _phoneContext.OrderItem on o.OrderId equals oi.OrderId
                where o.OrderStatus == orderstatus
                select new BackOrderShowDto
                {
                     OrderId = o.OrderId,
                     OrderTime = o.OrderTime,
                     Account = o.Account,
                     TotalPrice = o.TotalPrice,
                     Address = o.Address,
                     OrderStatus = o.OrderStatus,
                     Items = (from i in _phoneContext.OrderItem
                              join it in _phoneContext.Item on i.ItemId equals it.ItemId
                              join f in _phoneContext.Format on i.FormatId equals f.FormatId
                              where i.OrderId == o.OrderId
                              select new BackOrderItemShowDto
                              {
                                  ItemName = it.ItemName,
                                  ItemFormat = f.Space + "-" + f.Color,
                                  ItemPrice = f.ItemPrice,
                                  ItemNum = i.ItemNum
                              }).ToList()
                 }).GroupBy(o => o.OrderId).Select(g => g.First());
                          
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return order;
        }
        #endregion
        #region 訂單狀態更改為已出貨
        public string OrderStatusSent(int orderId)
        {
            var order =  _phoneContext.Order.SingleOrDefault(o => o.OrderId == orderId);
            if(order!=null)
            {
                if(order.OrderStatus == "未出貨")
                {
                    order.OrderStatus = "已出貨";
                    _phoneContext.SaveChanges();
                    return("訂單已出貨");
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
