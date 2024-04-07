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
        public IEnumerable<ProductDto> Get()
        {
            var result = _service.GetProduct();
            return result;
        }
        #endregion
        #region 商品詳細頁
        [HttpPost]
        public IEnumerable<ItemDto> Item(string color,string space){
            
            var ItemDto = _service.Item(color,space);
            // var result = ItemDto.Select(itemDto=>JsonConvert.SerializeObject(itemDto));
            return ItemDto;
        }
        #endregion

    }
}
