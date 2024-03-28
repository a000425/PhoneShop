using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MP.Dtos;
using MP.Models;
using MP.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MPController : ControllerBase
    {
        private readonly PhoneContext _phoneContext;
        private readonly IMapper _mapper;
        private readonly RegisterService _services;
        public MPController(PhoneContext phoneContext,IMapper mapper, RegisterService services)
        {
            _phoneContext = phoneContext;
            _mapper = mapper;
            _services = services;
        }
        // GET: api/<MPController>
        [HttpGet]
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
            await _services.RegisterAsync(newmember);
            return Ok("註冊成功");
            
        }

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
