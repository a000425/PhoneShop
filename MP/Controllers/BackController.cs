using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Models;
using MP.Services;
using MP.Dtos;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]
    public class BackController : ControllerBase
    {
        private readonly BackService _backService;
        public BackController(BackService backService){
            _backService = backService;
        }
        #region 新增商品
        [HttpPost("AddProduct")]
        public IActionResult AddProduct(ItemDto itemDto){
            var result = _backService.AddProduct(itemDto);
            var response = new { Status = 200, Message = result };
            var jsongoodResponse = JsonConvert.SerializeObject(response);
            return Content(jsongoodResponse, "application/json");
        }
        #endregion
        #region 上架
        [HttpPost("UP")]
        public IActionResult UPItem([FromBody]int ItemId){
            var result = _backService.UPItem(ItemId);
            if(result == "上架成功"){
                var response = new { Status = 200, message = result};
                var jsongoodResponse = JsonConvert.SerializeObject(response);
                return Content(jsongoodResponse, "application/json");
            }
            else{
                var response = new { Status = 400, message = result};
                var jsongoodResponse = JsonConvert.SerializeObject(response);
                return Content(jsongoodResponse, "application/json");                
            }
        }
        #endregion
        #region 下架
        [HttpPost("Down")]
        public IActionResult DownItem([FromBody]int ItemId){
            var result = _backService.DownItem(ItemId);
            if(result == "下架成功"){
                var response = new { Status = 200, message = result};
                var jsongoodResponse = JsonConvert.SerializeObject(response);
                return Content(jsongoodResponse, "application/json");
            }
            else{
                var response = new { Status = 400, message = result};
                var jsongoodResponse = JsonConvert.SerializeObject(response);
                return Content(jsongoodResponse, "application/json");                
            }
        }
        #endregion
        #region 商品管理
        [HttpGet("Items")]
        public IActionResult Items()
        {
            var result = _backService.Items();
            var response = new { Status = 400, message = result};
            var jsongoodResponse = JsonConvert.SerializeObject(response);
            return Content(jsongoodResponse, "application/json");
        }
        #endregion
        #region 顯示所有未回覆QA
        [HttpGet("QA/Unreply")]
        public IActionResult GetQAUnreply([FromForm]string search)
        {
            var result = _backService.GetQAUnreply(search);
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "沒有未回覆QA" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 顯示單筆未回覆QA
        [HttpGet("QA/Unreply/{QAId}")]
        public IActionResult GetQAUnreplybyId(int QAId)
        {
            var result = _backService.GetQAUnreplybyId(QAId) ;
            var response = new { Status = 200, Message = result };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse, "application/json");
        }
        #endregion
        #region 顯示所有已回覆QA
        [HttpGet("QA/Reply")]
        public IActionResult GetQAReply([FromForm]string search)
        {
            var result = _backService.GetQAReply(search);
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "沒有已回覆QA" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 回覆問題
        [HttpPost("QA/Reply/{QAId}")]
        public IActionResult ReplyQA(int QAId, [FromForm] string reply)
        {
            var result = _backService.ReplyQA(QAId, reply);
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messaeg = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 未出貨訂單顯示
        [HttpGet("OrderShowUnsend")]
        public IActionResult OrderShowUnsend([FromForm] string search)
        {
            var Order = _backService.GetOrderUnsend(search);
            if(Order != null && Order.Any())
            {
                var response = new { Status = 200, Message = Order };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messaeg = Order };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 已出貨訂單顯示
        [HttpGet("OrderShowSent")]
        public IActionResult OrderShowSent([FromForm]string search)
        {
            var Order = _backService.GetOrderSent(search);
            if(Order != null && Order.Any())
            {
                var response = new { Status = 200, Message = Order };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messaeg = Order };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 已完成訂單顯示
        [HttpGet("OrderShowFinish")]
        public IActionResult OrderShowFinish([FromForm] string search)
        {
            var Order = _backService.GetOrderFinish(search);
            if(Order != null && Order.Any())
            {
                var response = new { Status = 200, Message = Order };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messaeg = Order };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 更改訂單狀態至已出貨
        [HttpPost("OrderSent")]
        public IActionResult OrderSent([FromBody] int OrderId)
        {
            var result = _backService.OrderSent(OrderId);
            var response = new{Status=200,Message= result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 搜尋(商品&問答)
        [HttpGet("SearchProduct")]
        public IActionResult SearchProduct(string Search){
            var result = _backService.SearchProduct(Search);
            var response = new{Status=200,Message = result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 所有商品庫存更改顯示
        [HttpGet("ItemUpdate")]
        public IActionResult ItemUpdate()
        {
            var items = _backService.GetAllItem();
            if(items != null && items.Any())
            {
                var response = new { Status = 200, Message = items };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messaeg = items };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 單筆商品庫存更改顯示
        [HttpGet("ItemUpdate/{FormatId}")]
        public IActionResult ItemUpdate(int FormatId)
        {
            var item = _backService.GetOneItem(FormatId);
            if(item != null)
            {
                var response = new { Status = 200, Message = item };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messaeg = item };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 加入商品庫存與更改價格
        [HttpPost("ItemUpdate/{FormatId}")]
        public IActionResult UpdateItemSaP(int FormatId,[FromBody] ItemDto Item)
        {
            var result = _backService.UpdateItem(FormatId, Item.Store, Item.ItemPrice);
            var response = new{Status=200,Message= result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 搜尋(問答)
        [HttpGet("SearchQA")]
        public IActionResult SearchQA(string Search){
            var result = _backService.SearchQA(Search);
            var response = new{Status=200,Message = result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 商品庫存搜尋
        [HttpGet("ItemSearch")]
        public IActionResult ItemSearch([FromForm]string search)
        {
            var result = _backService.ItemSearch(search);
            var response = new{Status=200,Message= result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
    }
}
