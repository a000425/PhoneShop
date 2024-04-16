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
        #region 上架
        public string UPItem(int ItemId){
            var result = _repository.UPItem(ItemId);
            return result;
        }
        #endregion
        #region 下架
        public string DownItem(int ItemId){
            var result = _repository.DownItem(ItemId);
            return result;
        }
        #endregion
        #region 取得所有QA未回覆
        public IEnumerable<BackQAUnreplyDto> GetQAUnreply()
        {
            var result = _repository.GetQaUnreply();
            return result;

        }
        #endregion
        #region 取得所有QA已回覆
        public IEnumerable<BackQAReplyDto> GetQAReply()
        {
            var result = _repository.GetQaReply();
            return result;
        }
        #endregion
        #region 回覆QA
        public string ReplyQA(int QAId, string Reply)
        {
            string result = _repository.ReplyQa(QAId,Reply);
            return result;
        }
        #endregion
    }
}