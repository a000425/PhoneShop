using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MP.Models;
using MP.Dtos;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using MP.Repository;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace MP.Services
{
    public class BackService
    {
        private readonly BackRepository _repository;
        private readonly PhoneContext _phoneContext;
        public BackService(PhoneContext phoneContext,BackRepository repository)
        {
            _phoneContext = phoneContext;
            _repository = repository;
        }

        public string AddProduct(ItemDto itemDto){
            if(_repository.AddItem(itemDto)){
                _repository.AddFormat(itemDto);
                _repository.AddImg(itemDto);
                return "新增成功";
            }
            return "新增失敗";
        }
    }
}