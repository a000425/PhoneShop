using System.Security.AccessControl;
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
        #region 新增商品
        public bool AddItem(ItemDto itemDto){
            try{
                var exist = _phoneContext.Item.Any(x => x.ItemName==itemDto.ItemName);
                if (!exist){
                var Item = new Item {
                ItemName = itemDto.ItemName,
                Instruction = itemDto.Instruction,
                IsAvailable = true,
                CreateTime = DateTime.Now,
                UPTime = DateTime.Now
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
                var formatId = (from a in _phoneContext.Format
                                where a.ItemId == itemDto.ItemId && a.Space == itemDto.Space && a.Color == itemDto.Color
                                select a.FormatId).FirstOrDefault();

                if (itemDto.ItemImg != null)
                {
                    foreach (var itemImg in itemDto.ItemImg)
                    {
                        var img = new Models.Img {
                            FormatId = formatId, 
                            ItemImg = itemImg
                        };
                        _phoneContext.Img.Add(img);
                    }
                    _phoneContext.SaveChanges();
                }
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
        }
        #endregion
        #region 上架
        public string UPItem(int ItemId){
            try{
                var item = (from a in _phoneContext.Item
                            where a.ItemId == ItemId
                            select a).SingleOrDefault();
                item.IsAvailable = true;
                item.UPTime = DateTime.Now;
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
        #endregion
        #region 下架
        public string DownItem(int ItemId){
            try{
                var item = (from a in _phoneContext.Item
                            where a.ItemId == ItemId
                            select a).SingleOrDefault();
                item.IsAvailable = false;
                item.DownTime = DateTime.Now;
                _phoneContext.Update(item);
                _phoneContext.SaveChanges();
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return "下架成功";
            
        }
        #endregion
        #region 商品管理
        public IEnumerable<Item> Items()
        {
            var result = (from i in _phoneContext.Item
                          select new Item{
                            ItemId = i.ItemId,
                            ItemName = i.ItemName,
                            UPTime = i.UPTime,
                            DownTime = i.DownTime,
                            IsAvailable = i.IsAvailable
                          }).ToList();
            return result;
        }
        #endregion
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
        public IEnumerable<BackQADto> GetQaUnreply()
        {
            IEnumerable<BackQADto> result;
            try
            {
                result = (from q in _phoneContext.QA
                        join i in _phoneContext.Item on q.ItemId equals i.ItemId
                        where q.ReplyTime == null
                        select new BackQADto
                        {
                            Id = q.Id,
                            ItemId = q.ItemId,
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
        #region 取得單筆未回覆
        public BackQADto GetQAUnreplybyId(int QAId)
        {
            var result = (from Q in _phoneContext.QA
                         join item in _phoneContext.Item on Q.ItemId equals item.ItemId
                          where Q.Id==QAId
                          select new BackQADto{
                            ItemId = Q.Id,
                            Account = Q.Account,
                            ItemName = item.ItemName,
                            Content = Q.Content,
                            CreateTime = Q.CreateTime
                          }).SingleOrDefault();
            return result;
        }
        #endregion
        #region 取得已回覆留言
        public IEnumerable<BackQADto> GetQaReply()
        {
            IEnumerable<BackQADto> result;
            try
            {
            result = (from q in _phoneContext.QA
                    join i in _phoneContext.Item on q.ItemId equals i.ItemId
                    where q.ReplyTime != null 
                    select new BackQADto
                    {
                        ItemId = i.ItemId,
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
                    Q.Id = QAId;
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
        {   
            IEnumerable<BackOrderShowDto> order;
            string orderstatus = "未出貨";
            order = getOrder(orderstatus);
            return order;
        }
        #endregion
        #region 取得所有已出貨訂單
        public IEnumerable<BackOrderShowDto> getOrderSent()
        {   
            IEnumerable<BackOrderShowDto> order;
            string orderstatus = "已出貨";
            order = getOrder(orderstatus);
            return order;
        }
        #endregion
        #region 取得所有已完成訂單
        public IEnumerable<BackOrderShowDto> getOrderFinish()
        {   
            IEnumerable<BackOrderShowDto> order;
            string orderstatus = "已完成";
            order = getOrder(orderstatus);
            return order;
        }
        #endregion
        #region 抓所有訂單-無搜尋值
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
                     Discount = o.Discount,
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
                                  ItemPrice = i.ItemPrice,
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
        #region 抓所有訂單-有搜尋值
        public IEnumerable<BackOrderShowDto> getOrder(string orderstatus,string search)
        {   
            IEnumerable<BackOrderShowDto> order;
            try
            {
                order = (from o in _phoneContext.Order
                join oi in _phoneContext.OrderItem on o.OrderId equals oi.OrderId
                where o.OrderStatus == orderstatus && (o.Account.Contains(search) 
                || _phoneContext.OrderItem.Any(oi => oi.OrderId==o.OrderId 
                && _phoneContext.Item.Any(item => item.ItemId==oi.ItemId && item.ItemName.Contains(search))))
                select new BackOrderShowDto
                {
                     OrderId = o.OrderId,
                     OrderTime = o.OrderTime,
                     Account = o.Account,
                     TotalPrice = o.TotalPrice,
                     Discount = o.Discount,
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
        #region 搜尋(商品)
        public IEnumerable<Item> SearchProduct(string search){
            var result = (from i in _phoneContext.Item
                          join f in _phoneContext.Format on i.ItemId equals f.ItemId
                          where i.ItemName.Contains(search)
                          select new Item{
                            ItemId = i.ItemId,
                            ItemName = i.ItemName,
                            CreateTime = i.CreateTime
                          }).ToList();
            return result;
        }
        #endregion
        #region 搜尋(QA)
        public IEnumerable<BackQADto> SearchQA(string search){
            var result  = (from i in _phoneContext.Item
                          join QA in _phoneContext.QA on i.ItemId equals QA.ItemId
                          where i.ItemName.Contains(search) || QA.Account.Contains(search)||QA.Content.Contains(search)||QA.Reply.Contains(search)
                          select new BackQADto{
                            ItemId = i.ItemId,
                            ItemName = i.ItemName,
                            Account = QA.Account,
                            Content = QA.Content,
                            CreateTime = QA.CreateTime,
                            Reply = QA.Reply,
                            ReplyTime = QA.ReplyTime
                          });
            return result;
        }
        #endregion
        #region 取得所有Item與Format
        public IEnumerable<BackItemStoreDto> getAllItem()
        {   
            IEnumerable<BackItemStoreDto> Items;
            try
            {
                
                Items = (from i in _phoneContext.Item
                select new BackItemStoreDto
                {
                     ItemName = i.ItemName,
                     CreateTime = i.CreateTime,
                     Format = (from f in _phoneContext.Format
                               where f.ItemId == i.ItemId
                               group f by f.Space into groupedFormats
                              select new BackItemFormatStoreDto
                              {
                                  Space = groupedFormats.Key,
                                  info = groupedFormats.Select(fi => new BackItemFormatStoreDto
                                          {
                                            Color = fi.Color,
                                            Store = fi.Store,
                                            ItemPrice = fi.ItemPrice,
                                            FormatId = fi.FormatId
                                          }).ToList()
                              }).ToList()
                 }).GroupBy(i => i.ItemName).Select(g => g.First());
                
                          
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return Items;
        }
        #endregion
        #region 取得一筆Item與Format
        public ItemDto getOneItem(int FormatId)
        {   
            ItemDto Item;
            try
            {
                
                Item = (from i in _phoneContext.Item
                        join f in _phoneContext.Format on i.ItemId equals f.ItemId
                        where f.FormatId == FormatId
                        select new ItemDto
                        {
                            Brand = f.Brand,
                            ItemName = i.ItemName,
                            Space = f.Space,
                            Color = f.Color,
                            ItemPrice = f.ItemPrice,
                            Store = f.Store
                                 

                        }).FirstOrDefault();
                
                          
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return Item;
        }
        #endregion
        #region 更新一商品庫存與單價
        public string updateItem(int FormatId,int Store,int Price)
        {
            var format = _phoneContext.Format.SingleOrDefault(f => f.FormatId == FormatId);
            //int storecheck = format.Store+Store;
            if(format!=null)
            {
                if(Store>=0 && Price>0)
                {
                    format.Store = Store;
                    format.ItemPrice = Price;
                    _phoneContext.SaveChanges();
                    return("商品更新成功");

                }else if(Store>=0 && Price<=0)
                {
                    format.Store = Store;
                    _phoneContext.SaveChanges();
                    return("商品庫存更新成功");
                }else if (Store<0 && Price>0)
                {
                    format.ItemPrice = Price;
                    _phoneContext.SaveChanges();
                    return("商品單價更新成功");
                }else
                {
                    return("輸入商品單價或數量有誤");
                }
                
            }
            else
            {
                return("商品FormatId異常，找不到此規格");
            }
        }
        #endregion
        #region 商品庫存搜尋
        public IEnumerable<BackItemStoreDto> ItemSearch(string search)
        {
            IEnumerable<BackItemStoreDto> Items;
            try
            {
            Items = (from i in _phoneContext.Item
             where i.ItemName.Contains(search)
             select new BackItemStoreDto
             {
                 ItemName = i.ItemName,
                 CreateTime = i.CreateTime,
                 Format = (from f in _phoneContext.Format
                               where f.ItemId == i.ItemId
                               group f by f.Space into groupedFormats
                              select new BackItemFormatStoreDto
                              {
                                  Space = groupedFormats.Key,
                                  info = groupedFormats.Select(fi => new BackItemFormatStoreDto
                                          {
                                            Color = fi.Color,
                                            Store = fi.Store,
                                            ItemPrice = fi.ItemPrice,
                                            FormatId = fi.FormatId
                                          }).ToList()
                              }).ToList()
                 }).ToList();
            }
            catch(Exception e)
            {
                throw new Exception(e.ToString());
            }
            return Items;
        }
        #endregion
        #region 取得所有會員
        public IEnumerable<Account> getAllAccount()
        {
            IEnumerable<Account> result;
            try
            {
                
                result= (from a in _phoneContext.Account
                        where a.IsAdmin ==false
                        select new Account
                        {
                            Account1 = a.Account1,
                            Name = a.Name,
                            Email = a.Email,
                            Cellphone = a.Cellphone,
                            MemberKind = a.MemberKind,
                            MemberTime = a.MemberTime,
                            CanUse = a.CanUse
                        }).ToList();
                
                          
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return result;
        }
        #endregion
        #region 變更會員等級
        public bool changeAccountLevel(int level,string account)
        {
            
            try
            {
                var member = _phoneContext.Account.SingleOrDefault(a => a.IsAdmin == false && a.Account1 == account);
                if(member == null)
                {
                    return false;
                }else
                {
                    if(level == 0)
                    {
                        member.MemberKind = null;
                        member.MemberTime = null;
                        _phoneContext.SaveChanges();
                    }else if(level == 1)
                    {
                        member.MemberKind = "銀級會員";
                        member.MemberTime = DateTime.Now;
                        _phoneContext.SaveChanges();
                    }else if(level == 2)
                    {
                        member.MemberKind = "金級會員";
                        member.MemberTime = DateTime.Now;
                        _phoneContext.SaveChanges();
                    }else
                    {
                        return false;
                    }
                }
                
                          
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return true;
        }
        #endregion
        #region 停權會員
        public bool unUse(string account){
            try{
                var member = _phoneContext.Account.SingleOrDefault(a => a.IsAdmin == false && a.Account1 == account);
                member.CanUse = false;
                
                _phoneContext.Update(member);
                _phoneContext.SaveChanges();
                if(member == null)
                    return false;
            }
            catch{
                return false;
            }
            
            return true;
        }
        #endregion
        #region 解除停權會員
        public bool canUse(string account){
            try{
                var member = _phoneContext.Account.SingleOrDefault(a => a.IsAdmin == false && a.Account1 == account);
                member.CanUse = true;
                _phoneContext.Update(member);
                _phoneContext.SaveChanges();
                if(member == null)
                    return false;
            }
            catch{
                return false;
            }
            
            return true;
        }
        #endregion

        
        #region 取得圖表所需資訊(年初至今月每月銷售)
        public ChartDataDto getAllMonthsell()
        {
            int IsMonth = DateTime.Now.Month;
            int IsYear = DateTime.Now.Year;
           var data = new ChartDataDto();
           data.Labels = Enumerable.Range(1, IsMonth).Select(i => i.ToString()+"月").ToArray();
           try
            {
                data.Datasets = new[]
                {
                    new ChartDatasetDto
                    {
                        Label = "月銷售額",
                        Data = new int[IsMonth],
                        BorderWidth = 1
                    }
                };

                for(int i = 1 ;i<=IsMonth;i++)
                {
                    var sell = (from o in _phoneContext.Order
                                where o.OrderTime.Year == IsYear && o.OrderTime.Month == i
                                select new Order
                                {
                                    TotalPrice = o.TotalPrice
                                }).ToList();
                    int MonthSell  = sell.Sum(o => o.TotalPrice);
                    data.Datasets[0].Data[i-1] = MonthSell;
                }
                
                
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return data;
        }
        #endregion

       #region 取得圖表資訊(今月各品牌銷售量圓餅圖)
        public ChartDataDto getAllBrandMonthNum()
        {
            int IsMonth = DateTime.Now.Month;
            int IsYear = DateTime.Now.Year;
            var data = new ChartDataDto();
        
            try
            {
                var sell = (from o in _phoneContext.Order
                            join oi in _phoneContext.OrderItem on o.OrderId equals oi.OrderId
                            join f in _phoneContext.Format on oi.FormatId equals f.FormatId
                            join i in _phoneContext.Item on f.ItemId equals i.ItemId
                            where o.OrderTime.Year == IsYear && o.OrderTime.Month == IsMonth
                            select new OrderInfoDto
                            {
                                    Brand = f.Brand+" "+i.ItemName+"/"+f.Space,
                                    ItemNum = oi.ItemNum
                            }).ToList();
                List<string> Brand = new List<string>();
                
                foreach(var order in sell)
                {
                    if(!Brand.Contains(order.Brand))
                    {
                        Brand.Add(order.Brand);
                    }
                }
                int[] num = new int[Brand.Count];
                data.Labels = Brand.ToArray();
                data.Datasets = new[]
                {
                    new ChartDatasetDto
                    {
                        Label = "月銷售量",
                        Data = new int[Brand.Count],
                        BorderWidth = 1
                    }
                };
                foreach(var order in sell)
                {
                    num[Brand.IndexOf(order.Brand)]+=order.ItemNum;
                }
                data.Datasets[0].Data= num;
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return data;
        }
        #endregion
        #region 取得圖表資訊(今年品牌商品銷售量圓餅圖)
        public ChartDataDto getBrandYearNum(string brand)
        {
           var data = new ChartDataDto();
           try
            {
               var sell = (from o in _phoneContext.Order
                            join oi in _phoneContext.OrderItem on o.OrderId equals oi.OrderId
                            join f in _phoneContext.Format on oi.FormatId equals f.FormatId
                            join i in _phoneContext.Item on f.ItemId equals i.ItemId
                            where o.OrderTime.Year == DateTime.Now.Year && f.Brand == brand
                            select new OrderInfoDto
                            {
                                    Brand = f.Brand+" "+i.ItemName+"/"+f.Space,
                                    ItemNum = oi.ItemNum
                            }).ToList();
                List<string> Brand = new List<string>();
                
                foreach(var order in sell)
                {
                    if(!Brand.Contains(order.Brand))
                    {
                        Brand.Add(order.Brand);
                    }
                }
                int[] num = new int[Brand.Count];
                data.Labels = Brand.ToArray();
                data.Datasets = new[]
                {
                    new ChartDatasetDto
                    {
                        Label = "年總銷售量",
                        Data = new int[Brand.Count],
                        BorderWidth = 1
                    }
                };
                foreach(var order in sell)
                {
                    num[Brand.IndexOf(order.Brand)]+=order.ItemNum;
                }
                data.Datasets[0].Data= num;
   
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return data;
        }
        #endregion

        #region 取得圖表資訊(年初至今月兩品牌銷售比較)
        public ChartDataDto compareTwoBrand(string brand1,string brand2)
        {
            int IsMonth = DateTime.Now.Month;
            int IsYear = DateTime.Now.Year;
           var data = new ChartDataDto();
           data.Labels = Enumerable.Range(1, IsMonth).Select(i => i.ToString()+"月").ToArray();
           try
            {
                if(brand2!=null)
                {
                    data.Datasets = new[]
                    {
                        new ChartDatasetDto
                        {
                            Label = brand1+"銷售量",
                            Data = new int[IsMonth],
                            BorderWidth = 1
                        },
                        new ChartDatasetDto
                        {
                            Label = brand2+"銷售量",
                            Data = new int[IsMonth],
                            BorderWidth = 1
                        }
                    };
                }else
                {
                    data.Datasets = new[]
                    {
                        new ChartDatasetDto
                        {
                            Label = brand1+"銷售量",
                            Data = new int[IsMonth],
                            BorderWidth = 1
                        }
                    };
                }
                

                for(int x = 1 ;x<=IsMonth;x++)
                {
                    var sell = (from o in _phoneContext.Order
                            join oi in _phoneContext.OrderItem on o.OrderId equals oi.OrderId
                            join f in _phoneContext.Format on oi.FormatId equals f.FormatId
                            join i in _phoneContext.Item on f.ItemId equals i.ItemId
                            where o.OrderTime.Year == DateTime.Now.Year &&o.OrderTime.Month == x &&f.Brand == brand1
                            select new OrderInfoDto
                            {
                                    Brand = f.Brand,
                                    ItemNum = oi.ItemNum
                            }).ToList();
                    data.Datasets[0].Data[x-1] = sell.Sum(o => o.ItemNum);
                    if(brand2!=null)
                    {
                        var sell2 = (from o in _phoneContext.Order
                            join oi in _phoneContext.OrderItem on o.OrderId equals oi.OrderId
                            join f in _phoneContext.Format on oi.FormatId equals f.FormatId
                            join i in _phoneContext.Item on f.ItemId equals i.ItemId
                            where o.OrderTime.Year == DateTime.Now.Year && o.OrderTime.Month == x && f.Brand == brand2
                            select new OrderInfoDto
                            {
                                    Brand = f.Brand,
                                    ItemNum = oi.ItemNum
                            }).ToList();
                        data.Datasets[1].Data[x-1] = sell2.Sum(oa => oa.ItemNum);
                    }
                }
                
                
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return data;
        }
        #endregion
        
        #region 取得圖表所需資訊(年初至今月每月會員數量)
        public ChartDataDto getAllMonthMenber()
        {
            int IsMonth = DateTime.Now.Month;
            int IsYear = DateTime.Now.Year;
           var data = new ChartDataDto();
           data.Labels = Enumerable.Range(1, IsMonth).Select(i => i.ToString()+"月").ToArray();
           try
            {
                data.Datasets = new[]
                {
                    new ChartDatasetDto
                    {
                        Label = "月註冊量",
                        Data = new int[IsMonth],
                        BorderWidth = 1
                    }
                };

                for(int i = 1 ;i<=IsMonth;i++)
                {
                    var member =  _phoneContext.Account
                                .Where(a=>a.RegisterTime.HasValue && a.RegisterTime.Value.Year == IsYear && a.RegisterTime.Value.Month == i)
                                .Count();
                    //int MonthSell  = sell.Sum(o => o.TotalPrice);
                    data.Datasets[0].Data[i-1] = member;
                }
                
                
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return data;
        }
        #endregion
    }
}
