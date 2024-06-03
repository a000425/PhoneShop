using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MP.Dtos;
using MP.Models;
using MP.Repository;
using MP.Services;
using System.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Routing.Tree;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParadiseController : ControllerBase
    {
        private readonly ProductService _service;
        public ParadiseController(ProductService service){
            _service = service;
        }
        #region 取得商品一覽
        [HttpGet]
        public IActionResult GetAllProduct([FromQuery]int sortway,[FromQuery]int nowPage)
        {
            var result = _service.GetProduct(sortway,nowPage);
            var response = new{ Status = 200, Message = result };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 熱銷商品
        [HttpGet("Hot")]
        public IActionResult GetHotProduct([FromQuery]int sortway){
            var result = _service.GetHotProduct(sortway);
            var response = new{ Status = 200, Message = result };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 取得品牌商品一覽與排序
        [HttpGet("{Brand}")]
        public IActionResult GetProductByBrand(string Brand,[FromQuery]int sortway,[FromQuery]int nowPage){
            var result = _service.GetProductByBrand(Brand,sortway,nowPage);
            var response = new{ Status = 200, Message = result };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 取得商品一覽(價格)
        [HttpGet("GetProduct")]
        public IActionResult GetProductByPrice([FromQuery]int MaxPrice,[FromQuery]int MinPrice,[FromQuery]int sortway,[FromQuery]int nowPage){
            var result = _service.GetProductByPrice(MaxPrice,MinPrice,sortway,nowPage);
            var response = new{ Status = 200, Message = result };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 商品資訊
        [HttpGet("ProductInfo")]
        public IActionResult GetProduct([FromQuery]int ItemId){
            var result = _service.GetProductById(ItemId);
            if(result != null && result.Any()){
                var response = new{ Status = 200, Message = result};
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse,"application/json");
            }   
            else{
                var response = new{ Status = 400, Messae = "查無商品"};
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse,"application/json");
            }
        }
        #endregion
        [HttpGet("GetItemId")]
        #region 商品詳細頁 > 規格
        public IActionResult GetId([FromBody]Format format){
            var result = _service.GetId(format.Color,format.Space,format.ItemId);
            var response = new{Status = 200, Message =  result.FormatId};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region QA顯示
        [HttpGet("QA/{ItemId}")]
        public IActionResult GetProductQA(int ItemId)
        {
            var result = _service.GetQAById(ItemId);
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "查無商品" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region QA問
        [Authorize]
        [HttpPost("{ItemId}")]
        public IActionResult ProductQA(int ItemId, [FromForm] string content)
        {
            var result = _service.ProductQA(ItemId, HttpContext.User.Identity.Name, content);
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "查無商品" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 推薦系統(相似商品)
        [Authorize]
        [HttpGet("Similar")]
        public IActionResult SimilarProducts([FromQuery]ItemDto itemDto){
            var result = _service.SimilarProducts(itemDto);
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "錯誤" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
        #region 推薦系統(其他熱銷)
        /*[Authorize]
        [HttpGet("Other")]
        public IActionResult OtherProducts([FromBody]ItemDto itemDto){
            var result = _service.OtherProducts(itemDto);
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "錯誤" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }*/
        #endregion
        #region 推薦系統(其他人也購買)
        [Authorize]
        [HttpGet("Otherbuy")]
        public IActionResult Otherbuy([FromQuery]ItemDto itemDto){
            var result = _service.Otherbuy(itemDto);
            if (result != null && result.Any())
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messae = "錯誤" };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
        }
        #endregion
    }
}
