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
    [Authorize("Admin")]
    public class BackController : ControllerBase
    {
        private readonly BackService _backService;
        public BackController(BackService backService){
            _backService = backService;
        }
        #region 新增商品
        [HttpPost("AddProduct")]
        public IActionResult AddProduct(ItemDto itemDto){
            var result = _backService.AddProduct(itemDto);
            var response = new { Status = 200, Message = result };
            var jsongoodResponse = JsonConvert.SerializeObject(response);
            return Content(jsongoodResponse, "application/json");
        }
        #endregion
    }
}
