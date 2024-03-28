using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MP.Dtos;
using MP.Models;
using MP.Services;
using System.Security;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MPController : ControllerBase
    {
        private readonly PhoneContext _phoneContext;
        private readonly IMapper _mapper;
        private readonly MemberService _services;
        private readonly MailService _mail;
        public MPController(PhoneContext phoneContext,IMapper mapper, MemberService services,MailService mail)
        {
            _phoneContext = phoneContext;
            _mapper = mapper;
            _services = services;
            _mail = mail;
        }
        // GET: api/<MPController>
        // 偵錯用 保留
        [HttpGet("Test")]
        public IEnumerable<RegisterDto> Get()
        {
            var result = _phoneContext.Account
                //.Include(a=>a.Cart)
                //.Include(a=>a.Order)
                //.Include(a=>a.QA)
                .Select(a => a);
            var map = _mapper.Map<IEnumerable<RegisterDto>>(result);
            return map;
        }

        // GET api/<MPController>/5
        [HttpGet("Register/{account}")]
        public RegisterDto Get(string account)
        {
            var result = (from a in _phoneContext.Account where a.Account1 == account select a)
                //.Include(a => a.Cart)
                //.Include(a => a.Order)
                //.Include(a => a.QA)
                .SingleOrDefault();  //傳回序列唯一的項目，若序列是空的則傳回預設值
            return _mapper.Map<RegisterDto>(result);
        }

        //[HttpGet("Login/{account}")]
        //public LoginDto Get(string account, string password)
        //{
        //    var member= (from a in _phoneContext.Account where a.Account1==account select a)
        //        .Include(a=>a.Cart)
        //        .Include(a=>a.Order)
        //        .Include(a=>a.QA)
        //        .SingleOrDefault();
        //    if (member == null)
        //    {
        //        return Dto.NotFound();
        //    }
        //}

        // POST api/<MPController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account newmember)
        {
            //if(newmember == null || string.IsNullOrWhiteSpace(newmember.Account1) || string.IsNullOrWhiteSpace(newmember.Password))
            //{
            //    return BadRequest("帳號密碼不得為空");
            //}
            if(!_services.CheckAccount(newmember.Account1)){
                await _services.RegisterAsync(newmember);
                string TempMail = System.IO.File.ReadAllText("../MP/MailBody/MailBody.html");
                string ValidateUrl = $"{Request.Scheme}://{Request.Host}/api/MP/EmailValidate?Account={newmember.Account1}&AuthCode={newmember.AuthCode}";
                string mailBody = _mail.GetMailBody(TempMail,newmember.Account1,ValidateUrl);
                _mail.SendMail(mailBody,newmember.Email);
                return Ok("註冊成功");
            }
            else{
                return BadRequest("帳號已註冊");
            }
            
        }
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
        /*[HttpPut("PasswordChange")] 
        [Authorize]
        public async Task<IActionResult> Put([FromForm]PasswordChangeDto ChangePassword)
        {
            if(_services.CheckPassword(ChangePassword.OldPassword))
            {
                await _services.PasswordChange(ChangePassword.Password, User.Identity.Name);
                return Ok("更改成功");
            }
            return BadRequest("更改失敗");
        }*/
        // PUT api/<MPController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MPController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
