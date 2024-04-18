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
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;
        private readonly MemberService _memberService;
        public CartController(CartService service, MemberService memberService,IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _memberService = memberService;
        }
        #region 加入購物車
        [HttpPost]
        public IActionResult AddCart(Cart cart)
        {
            var result = _service.AddCart(HttpContext.User.Identity.Name,cart.ItemId,cart.ItemNum,cart.FormatId);
            var response = new { Status = 200, Message = result };
            var jsongoodResponse = JsonConvert.SerializeObject(response); // 序列化為 JSON 格式的字符串
            return Content(jsongoodResponse, "application/json");
        }
        #endregion
        #region 顯示購物車內的商品資訊
        [HttpGet]
        public IEnumerable<CartDto> Get()
        {
            var result = _service.GetAllCartList(HttpContext.User.Identity.Name);
            return result;
        }
        #endregion
        #region 刪除單筆商品
        [HttpDelete]
        public IActionResult DeleteItemFromCart([FromBody]Cart cart)
        {
            if(_service.DeleteCart(cart.Id,HttpContext.User.Identity.Name)){
                var response = new{Status=200,Message="已刪除"};
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse,"application/json");
            }else
            {
                var response = new{Status=400,Message= "查無項目"};
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse,"application/json");
            }
        } 
        #endregion
        #region 下訂單
        [HttpPost("Order")]
        public IActionResult getOrder([FromBody]string address){
            var OrderResult = _service.getOrder(HttpContext.User.Identity.Name,address);
            var response = new{Status=200,Message= OrderResult};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 顯示訂單資訊
        [HttpGet("GetOrderItem")]
        public IEnumerable<OrderItemDto> getOrderItem()
        {
            var getOrderItem = _service.GetOrderItem(HttpContext.User.Identity.Name);
            return getOrderItem;
        }
        #endregion
        #region 訂單查詢
        [HttpGet("GetOrderItemId")]
        public IEnumerable<OrderInfoDto> OrderDetail([FromBody]Order order)
        {
            var detail = _service.OrderDetail(HttpContext.User.Identity.Name,order.OrderId);
            return detail;
        }
        #endregion
        #region 更改訂單狀態至已完成
        [HttpPost("OrderFinish")]
        public IActionResult OrderFinish([FromBody] int OrderId)
        {
            var result = _service.OrderFinish(OrderId,HttpContext.User.Identity.Name);
            var response = new{Status=200,Message= result};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion


        #region 個人資料
        [HttpGet("Profile")]
        public ProfileDto Profile()
        {
            var profile = _service.Profile(HttpContext.User.Identity.Name);
            return profile;
        }
        #endregion
    }
}