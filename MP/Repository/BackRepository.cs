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
                var Item = new Item {
                ItemName = itemDto.ItemName,
                Instruction = itemDto.Instruction,
                IsAvailable = true,
                CreateTime = DateTime.Now
            };
            _phoneContext.Item.Add(Item);
            _phoneContext.SaveChanges();
            }
            catch{
                return false;
            }
            return true;
        }
        public void AddFormat(ItemDto itemDto){
            try{
                itemDto.ItemId = (from a in _phoneContext.Item
                          where a.ItemName == itemDto.ItemName
                          select a.ItemId).SingleOrDefault();
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
                                select a.FormatId).SingleOrDefault(),
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
    }
}
