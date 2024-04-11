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

        [HttpPost]
        public IActionResult AddCart(Cart cart)
        {
            var result = _service.AddCart(HttpContext.User.Identity.Name,cart.ItemId,cart.ItemNum,cart.FormatId);
            var response = new { Status = 200, Message = result };
            var jsongoodResponse = JsonConvert.SerializeObject(response); // 序列化為 JSON 格式的字符串
            return Content(jsongoodResponse, "application/json");
        }
        [HttpGet]
        public IEnumerable<CartDto> Get()
        {
            var result = _service.GetAllCartList(HttpContext.User.Identity.Name);
            return result;
        }

        [HttpDelete]
        public IActionResult DeleteItemFromCart(int id)
        {
            if(_service.DeleteCart(id,HttpContext.User.Identity.Name)){
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
        #region 下訂單
        [HttpPost("Order")]
        public IActionResult getOrder([FromForm]CartDto cartDto, [FromForm] string address){
            var OrderResult = _service.getOrder(cartDto,HttpContext.User.Identity.Name,address);
            var response = new{Status=200,Message= OrderResult};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
    }
}
