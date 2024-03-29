﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MP.Dtos;
using MP.Models;
using MP.Repository;
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
                .Select(a => a);
            var map = _mapper.Map<IEnumerable<RegisterDto>>(result);
            return map;
        }

        // GET api/<MPController>/5
        [HttpGet("Register/{account}")]
        public RegisterDto Get(string account)
        {
            var result = (from a in _phoneContext.Account where a.Account1 == account select a)
                .SingleOrDefault();  //傳回序列唯一的項目，若序列是空的則傳回預設值
            return _mapper.Map<RegisterDto>(result);
        }

        // POST api/<MPController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account newmember)
        {
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

        [HttpPost("Login")]
        public string Post(LoginDto loginDto){
            var result = _services.Login(loginDto.Account1,loginDto.Password);
            return result;
        }

        [HttpPost("ChangePassword")]
        public async Task<string> ChangePassword(ChangePasswordDto changeDto){
            var result =await _services.ChangePassword(changeDto.Account,changeDto.OldPassword,changeDto.NewPassword);
            return result;
        }

        // PUT api/<MPController>/5
        [HttpPut]
        public void Put(int id)
        {
        }

        // DELETE api/<MPController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
