﻿using Microsoft.AspNetCore.Authorization;
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
    public class CartController : ControllerBase
    {
        private readonly CartService _service;
        private readonly MemberService _memberService;
        private string UserAccount;
        public CartController(CartService service, MemberService memberService,IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _memberService = memberService;
            var context = httpContextAccessor.HttpContext;
            UserAccount = _memberService.GetUserAccountFromToken(context);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddCart(Cart cart)
        {
            var result = _service.AddCart(UserAccount,cart.ItemId,cart.ItemNum,cart.FormatId);
            var response = new { Status = 200, Message = result };
            var jsongoodResponse = JsonConvert.SerializeObject(response); // 序列化為 JSON 格式的字符串
            return Content(jsongoodResponse, "application/json");
        }
        [HttpGet]
        [Authorize]
        public IEnumerable<CartDto> Get()
        {
            var result = _service.GetAllCartList(UserAccount);
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteItemFromCart(int id)
        {
            if(_service.DeleteCart(id,UserAccount)){
                var response = new{Status=200,Message="已刪除"};
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse,"application/json");
            }else
            {
                var response = new{Status=200,Message="刪除失敗"};
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse,"application/json");
            }
        } 
    }
}