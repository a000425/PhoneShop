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
        public IActionResult GetAllProduct()
        {
            var result = _service.GetProduct();
            var response = new{ Status = 200, Message = result };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 商品資訊
        [HttpGet("{ItemId}")]
        public IActionResult GetProduct(int ItemId){
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

    }
}
