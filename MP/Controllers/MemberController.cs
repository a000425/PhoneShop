﻿using AutoMapper;
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
    public class MemberController : ControllerBase
    {
        //_phoneContext做完要後要從Controller刪除
        private readonly PhoneContext _phoneContext;
        private readonly IMapper _mapper;
        private readonly MemberService _services;
        private readonly MailService _mail;
        public MemberController(PhoneContext phoneContext,IMapper mapper, MemberService services,MailService mail)
        {
            _phoneContext = phoneContext;
            _mapper = mapper;
            _services = services;
            _mail = mail;
        }
        
        #region 註冊
        // POST api/<MPController>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Account newmember)
        {
            if(!_services.CheckAccount(newmember.Account1)){
                await _services.RegisterAsync(newmember);
                string TempMail = System.IO.File.ReadAllText("../MP/MailBody/MailBody.html");
                string ValidateUrl = $"{Request.Scheme}://{Request.Host}/api/Member/EmailValidate?Account={newmember.Account1}&AuthCode={newmember.AuthCode}";
                string mailBody = _mail.GetMailBody(TempMail,newmember.Account1,ValidateUrl);
                _mail.SendMail(mailBody,newmember.Email);
                return Ok("註冊成功");
            }
            else{
                return BadRequest("帳號已註冊");
            }
            
        }
        #endregion
        #region 驗證結果
        [HttpGet("EmailValidate")]
        public async Task<IActionResult> Get([FromQuery]string Account, string AuthCode)
        {
            if (await _services.EmailValidateAsync(Account, AuthCode))
            {
                return Ok("驗證成功");
            }
                else
            {
                return BadRequest("驗證失敗");
            }   
        }
        #endregion
        #region 登入
        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto){
            var result = _services.Login(loginDto.Account1,loginDto.Password);
            if (result == "登入成功")
            {
                string token = _services.GenerateToken(loginDto);
                var cookieOption = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    HttpOnly = true
                };
                Response.Cookies.Append("Token", token, cookieOption);
                var goodresponse = new { status = 200, Message = "已登入" };
                var jsongoodResponse = JsonConvert.SerializeObject(goodresponse); // 序列化為 JSON 格式的字符串
                return Content(jsongoodResponse, "application/json"); // 返回 JSON 格式的響應
            }
            var response = new { status = 400, Message = result };
            var jsonResponse = JsonConvert.SerializeObject(response); // 序列化為 JSON 格式的字符串
            return Content(jsonResponse, "application/json");
        }
        #endregion
        #region 密碼修改
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<string> ChangePassword(ChangePasswordDto changeDto){
            var result =await _services.ChangePassword(User.Identity.Name,changeDto.OldPassword,changeDto.NewPassword);
            return result;
        }
        #endregion
        #region 登出
        [HttpDelete]
        [Authorize]
        public string  logout()
        {
            var cookieOption = new CookieOptions{
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true
            };
            Response.Cookies.Append("Token", "", cookieOption);
            return "已登出";
        }
        #endregion
    }
}
