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
        public IEnumerable<BackQADto> GetQAUnreply()
        {
            var result = _repository.GetQaUnreply();
            return result;

        }
        #endregion
        #region 取得所有QA已回覆
        public IEnumerable<BackQADto> GetQAReply()
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
        #region 取得未出貨訂單
        public IEnumerable<BackOrderShowDto> GetOrderUnsend()
        {
            var Order = _repository.getOrderUnsend();
            return Order;
        }
        #endregion
        #region 取得已出貨訂單
        public IEnumerable<BackOrderShowDto> GetOrderSent()
        {
            var Order = _repository.getOrderSent();
            return Order;
        }
        #endregion
        #region 取得已完成訂單
        public IEnumerable<BackOrderShowDto> GetOrderFinish()
        {
            var Order = _repository.getOrderFinish();
            return Order;
        }
        #endregion
        #region 更改訂單狀態至已出貨
        public string OrderSent(int orderId)
        {
            var result = _repository.OrderStatusSent(orderId);
            return result;
        }
        #endregion
        #region 搜尋(商品)
        public IEnumerable<Item> SearchProduct(string Search){
            var result = _repository.SearchProduct(Search);
            return result;
        }
        #endregion
        #region 搜尋(QA)
        public IEnumerable<BackQADto> SearchQA(string Search){
            var result = _repository.SearchQA(Search);
            return result;
        }
        #endregion
    }
}