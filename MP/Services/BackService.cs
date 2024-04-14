using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MP.Models;
using MP.Dtos;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using MP.Repository;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace MP.Services
{
    public class BackService
    {
        private readonly BackRepository _repository;
        private readonly PhoneContext _phoneContext;
        public BackService(PhoneContext phoneContext,BackRepository repository)
        {
            _phoneContext = phoneContext;
            _repository = repository;
        }

        public string AddProduct(ItemDto itemDto){
            if(_repository.AddItem(itemDto)){
                _repository.AddFormat(itemDto);
                _repository.AddImg(itemDto);
                return "新增成功";
            }
            return "新增失敗";
        }

        #region 取得所有QA未回覆
        public IEnumerable<BackQAUnreplyDto> GetQAUnreply()
        {
            var result = (from q in _phoneContext.QA
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

            return result;

        }
        #endregion
        #region 取得所有QA已回覆
        public IEnumerable<BackQAReplyDto> GetQAReply()
        {
            var result = (from q in _phoneContext.QA
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

            return result;

        }
        #endregion
        #region 回覆QA
        public string ReplyQA(int QAId, string Reply)
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