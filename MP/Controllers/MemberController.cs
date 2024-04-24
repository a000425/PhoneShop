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
using Microsoft.AspNetCore.Identity;

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
        private readonly IConfiguration _configuration;
        public MemberController(PhoneContext phoneContext,IMapper mapper, MemberService services,MailService mail, IConfiguration configuration)
        {
            _phoneContext = phoneContext;
            _mapper = mapper;
            _services = services;
            _mail = mail;
            _configuration = configuration;
            
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
                var success = new{Status=200,Message="註冊成功"};
                var jsonsuccess = JsonConvert.SerializeObject(success);
                return Content(jsonsuccess,"application/json");
            }
            else{
                var success = new{Status=400,Message="帳號已註冊"};
                var jsonsuccess = JsonConvert.SerializeObject(success);
                return Content(jsonsuccess,"application/json");
            }
            
        }
        #endregion
        #region 驗證結果
        [HttpGet("EmailValidate")]
        public async Task<IActionResult> Get([FromQuery]string Account, string AuthCode)
        {
            if (await _services.EmailValidateAsync(Account, AuthCode))
            {
                var success = new{Status=200,Message="驗證成功"};
                var jsonsuccess = JsonConvert.SerializeObject(success);
                return Content(jsonsuccess,"application/json");
            }
            else
            {
                var fail = new{Status=400,Message="驗證失敗"};
                var jsonfail = JsonConvert.SerializeObject(fail);
                return Content(jsonfail,"application/json");
            }   
        }
        #endregion
        #region 登入
        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginDto loginDto){
            var result = _services.Login(loginDto.Account1,loginDto.Password);
            if (result == "登入成功")
            {
                string token = _services.GenerateToken(loginDto);
                var cookieOption = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    SameSite = SameSiteMode.Lax,
                    // Secure = true
                };
                Response.Cookies.Append("Token", token, cookieOption);
                var response = new { Status = 200, Message = "已登入" };
                var jsongoodResponse = JsonConvert.SerializeObject(response); // 序列化為 JSON 格式的字符串
                return Content(jsongoodResponse, "application/json"); // 返回 JSON 格式的響應
            }
            else{
                var response = new { Status = 400, Message = result };
                var jsongoodResponse = JsonConvert.SerializeObject(response); // 序列化為 JSON 格式的字符串
                return Content(jsongoodResponse, "application/json");
            }
        }
        #endregion
        #region 密碼修改
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changeDto){
            if (HttpContext.User.Identity.Name == null)
            {
                var response = new { Status = 400, Message = "密碼修改失敗" };
                var jsongoodResponse = JsonConvert.SerializeObject(response); // 序列化為 JSON 格式的字符串
                return Content(jsongoodResponse, "application/json");
            }
            var result =await _services.ChangePassword(HttpContext.User.Identity.Name,changeDto.OldPassword,changeDto.NewPassword);
            if(result=="密碼更改成功"){
                var response = new{Status=200,Messsage=result};
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse,"application/json");
            }
            else{
                var response = new{Status=200,Messsage=result};
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse,"application/json");
            }
        }
        #endregion
        #region 登出
        [HttpDelete]
        [Authorize]
        public IActionResult  logout()
        {
            var cookieOption = new CookieOptions{
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true
            };
            Response.Cookies.Append("Token", "", cookieOption);
            var response = new{Status=200,Message="已登出"};
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse,"application/json");
        }
        #endregion
        #region 取得帳號測試
        [Authorize("Admin")]
        [HttpGet("trytrysee")]
        public IActionResult trytrysee() 
        {
            var response = new { Status = 200, Messsage =  HttpContext.User.Identity.Name + HttpContext.User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Role).Value };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse, "application/json");
        }
        #endregion
    }
}
