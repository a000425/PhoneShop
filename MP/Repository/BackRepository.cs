using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MP.Dtos;
using MP.Models;

namespace MP.Repository
{
    public class BackRepository
    {
        private readonly PhoneContext _phoneContext;
        public BackRepository(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }
        public bool AddItem(ItemDto itemDto){
            try{
                var Item = new Item {
                ItemName = itemDto.ItemName,
                Instruction = itemDto.Instruction,
                IsAvailable = true
            };
            _phoneContext.Item.Add(Item);
            _phoneContext.SaveChanges();
            }
            catch{
                return false;
            }
            return true;
        }
        public void AddFormat(ItemDto itemDto){
            try{
                var format = new Models.Format {
                Brand = itemDto.Brand,
                Color = itemDto.Color,
                ItemPrice = itemDto.ItemPrice,
                Store = itemDto.Store,
                Space = itemDto.Space,
                ItemId = (from a in _phoneContext.Item
                          where a.ItemName == itemDto.ItemName
                          select a.ItemId).SingleOrDefault()
                };
                _phoneContext.Format.Add(format);
                _phoneContext.SaveChanges();
            }
            catch(Exception e){
                throw new Exception(e.ToString());
            }
        }
        public void AddImg(ItemDto itemDto){
            try{
                var img = new Models.Img {
                    FormatId = (from i in _phoneContext.Item
                                join f in _phoneContext.Format 
                                on i.ItemName equals itemDto.ItemName
                                where f.Color == itemDto.Color && f.Space == itemDto.Space & f.Brand == itemDto.Brand
                                select f.FormatId).SingleOrDefault(),
                    ItemImg = itemDto.ItemImg
                };
                _phoneContext.Img.Add(img);
                _phoneContext.SaveChanges();
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
        }
    }
}
