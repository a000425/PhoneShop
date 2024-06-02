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
        #region 商品管理
        public IEnumerable<Item> Items()
        {
            var result = _repository.Items();
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
        #region 顯示單筆未回覆QA
        public BackQADto GetQAUnreplybyId(int QAId)
        {
            var result = _repository.GetQAUnreplybyId(QAId);
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
        #region 取得所有Item與Format
        public IEnumerable<BackItemStoreDto> GetAllItem()
        {
            var Items = _repository.getAllItem();
            return Items;
        }
        #endregion
        #region 取得一筆Item與Format
        public ItemDto GetOneItem(int FormatId)
        {
            var Item = _repository.getOneItem(FormatId);
            return Item;
        }
        #endregion
        #region 更新商品庫存與價格
        public string UpdateItem(int FormatId, int Store, int Price)
        {
            string result = _repository.updateItem(FormatId,Store,Price);
            return result;
        }
        #endregion
        #region 商品庫存搜尋
        public IEnumerable<BackItemStoreDto> ItemSearch(string search)
        {
            var result = _repository.ItemSearch(search);
            return result;
        }
        #endregion
        #region 取得所有會員
        public IEnumerable<Account> GetAllAccount()
        {
            var result = _repository.getAllAccount();
            return result;
        }
        #endregion
        #region 變更會員等級
        public bool ChangeAccountLevel(int level, string account)
        {
            bool result = _repository.changeAccountLevel(level,account);
            return result;
        }
        #endregion
        #region 取得圖表所需資訊(年初至今月每月銷售)
        public ChartDataDto GetAllMonthsell()
        {
            ChartDataDto chartData = _repository.getAllMonthsell();
            return chartData;
        }
        #endregion
        #region 取得圖表資訊(今月各品牌銷售量圓餅圖)
        public ChartDataDto GetAllBrandMonthNum()
        {
            ChartDataDto chartData = _repository.getAllBrandMonthNum();
            return chartData;
        }
        #endregion

        #region 取得圖表資訊(今年品牌商品銷售量圓餅圖)
        public ChartDataDto GetBrandYearNum(string Brand)
        {
            ChartDataDto chartData = _repository.getBrandYearNum(Brand);
            return chartData;
        }
        #endregion
        #region 取得圖表資訊(年初至今月兩品牌銷售比較)
        public ChartDataDto CompareTwoBrand(string brand1,string brand2)
        {
            ChartDataDto chartData = _repository.compareTwoBrand(brand1,brand2);
            return chartData;
        }
        #endregion
        
        #region 取得圖表所需資訊(年初至今月每月會員數量)
        public ChartDataDto GetAllMonthMenber()
        {
            ChartDataDto chartData = _repository.getAllMonthMenber();
            return chartData;
        }
        #endregion
    }
}