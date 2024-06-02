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
        #region 查看所有會員資訊
        [HttpGet("GetAllAccountInfo")]
        public IActionResult GetAllAccountInfo()
        {
            var result = _backService.GetAllAccount();
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "出現問題" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 變更會員等級
        [HttpPost("ChangeAccountLevel")]
        public IActionResult ChangeAccountLevel([FromQuery]int level, Account account)
        {
            bool result = _backService.ChangeAccountLevel(level,account.Account1);
            if (result)
            {
                var response = new { Status = 200, Message = "變更成功" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "出現問題" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 會員停權
        [HttpPost("UnUse")]
        public IActionResult UnUse([FromBody]string Account){
            
            if(_backService.UnUse(Account)){
                var response = new { Status = 200, message = "停權成功"};
                var jsongoodResponse = JsonConvert.SerializeObject(response);
                return Content(jsongoodResponse, "application/json");
            }
            else{
                var response = new { Status = 400, message = "停權失敗"};
                var jsongoodResponse = JsonConvert.SerializeObject(response);
                return Content(jsongoodResponse, "application/json");                
            }
        }
        #endregion
        #region 會員解除停權
        [HttpPost("CanUse")]
        public IActionResult CanUse([FromBody]string Account){
            
            if(_backService.CanUse(Account)){
                var response = new { Status = 200, message = "解除成功"};
                var jsongoodResponse = JsonConvert.SerializeObject(response);
                return Content(jsongoodResponse, "application/json");
            }
            else{
                var response = new { Status = 400, message = "解除失敗"};
                var jsongoodResponse = JsonConvert.SerializeObject(response);
                return Content(jsongoodResponse, "application/json");                
            }
        }
        #endregion
        #region 新增商品
        [HttpPost("AddProduct")]
        public IActionResult AddProduct([FromBody]ItemDto itemDto){
            var result = _backService.AddProduct(itemDto);
            var response = new { Status = 200, Message = result };
            var jsongoodResponse = JsonConvert.SerializeObject(response);
            return Content(jsongoodResponse, "application/json");
        }
        #endregion
        #region 上傳圖檔
        [HttpPost("uploadimg")]
        public async Task<IActionResult> UploadImages(List<IFormFile> images)
        {
            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var filePath = Path.Combine("C:/Users/User/Desktop/frontstage/mobile_paradise-front-end/Index/image", image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
            }

            return Ok();
        }
        #endregion
        #region 上架
        [HttpPost("UP")]
        public IActionResult UPItem([FromQuery]int ItemId){
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
        public IActionResult DownItem([FromQuery]int ItemId){
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
            var response = new { Status = 200, message = result};
            var jsongoodResponse = JsonConvert.SerializeObject(response);
            return Content(jsongoodResponse, "application/json");
        }
        #endregion
        #region 顯示所有未回覆QA
        [HttpGet("QA/Unreply")]
        public IActionResult GetQAUnreply()
        {
            var result = _backService.GetQAUnreply();
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
        public IActionResult GetQAReply()
        {
            var result = _backService.GetQAReply();
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
        public IActionResult ReplyQA(int QAId, [FromBody] string reply)
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
        public IActionResult OrderShowUnsend()
        {
            var Order = _backService.GetOrderUnsend();
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
        public IActionResult OrderShowSent()
        {
            var Order = _backService.GetOrderSent();
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
        public IActionResult OrderShowFinish()
        {
            var Order = _backService.GetOrderFinish();
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
        #region 搜尋(商品)
        [HttpGet("SearchProduct")]
        public IActionResult SearchProduct([FromQuery]string Search){
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
        #region 更改商品庫存與價格
        [HttpPost("ItemUpdate")]
        public IActionResult UpdateItemSaP([FromBody] ItemDto Item)
        {
            var result = _backService.UpdateItem(Item.FormatId, Item.Store, Item.ItemPrice);
            var response = new{Status=200,Message= result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 搜尋(問答)
        [HttpGet("SearchQA")]
        public IActionResult SearchQA([FromQuery]string Search){
            var result = _backService.SearchQA(Search);
            var response = new{Status=200,Message = result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 商品庫存搜尋
        [HttpGet("ItemSearch")]
        public IActionResult ItemSearch([FromQuery]string search)
        {
            var result = _backService.ItemSearch(search);
            var response = new{Status=200,Message= result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion

        #region 取得圖表資訊(年初至今月銷售額折線圖)
        [HttpGet("GetAllMonthsell")]
        public IActionResult GetAllMonthsell()
        {
            var result = _backService.GetAllMonthsell();
            if(result != null)
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
        #region 取得圖表資訊(年初至今月兩品牌銷售比較)
        [HttpGet("CompareTwoBrand")]
        public IActionResult CompareTwoBrand([FromQuery] string brand1, string brand2)
        {
            var result = _backService.CompareTwoBrand(brand1,brand2);
            if(result != null)
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
        #region 取得圖表資訊(今月各品牌銷售量圓餅圖)
        [HttpGet("GetAllBrandMonthNum")]
        public IActionResult GetAllBrandMonthNum()
        {
            var result = _backService.GetAllBrandMonthNum();
            if(result != null)
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

        #region 取得圖表資訊(今年品牌商品銷售量圓餅圖)
        [HttpGet("GetBrandYearNum")]
        public IActionResult GetBrandYearNum([FromQuery] string Brand)
        {
            var result = _backService.GetBrandYearNum(Brand);
            if(result != null)
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
        #region 取得圖表資訊(年初至今月會員數成長圖)
        [HttpGet("GetAllMonthMenber")]
        public IActionResult GetAllMonthMenber()
        {
            var result = _backService.GetAllMonthMenber();
            if(result != null)
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
    }
}
