using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MP.Dtos;
using MP.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

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
                IsAvailable = true,
                CreateTime = DateTime.Now
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
                itemDto.ItemId = (from a in _phoneContext.Item
                          where a.ItemName == itemDto.ItemName
                          select a.ItemId).SingleOrDefault();
                var format = new Models.Format {
                Brand = itemDto.Brand,
                Color = itemDto.Color,
                ItemPrice = itemDto.ItemPrice,
                Store = itemDto.Store,
                Space = itemDto.Space,
                ItemId = itemDto.ItemId
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
                    FormatId = (from a in _phoneContext.Format
                                where a.ItemId == itemDto.ItemId && a.Space == itemDto.Space && a.Color == itemDto.Color
                                select a.FormatId).SingleOrDefault(),
                    ItemImg = itemDto.ItemImg
                };
                _phoneContext.Img.Add(img);
                _phoneContext.SaveChanges();
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
        }

        public string UPItem(int ItemId){
            try{
                var item = (from a in _phoneContext.Item
                            where a.ItemId == ItemId
                            select a).SingleOrDefault();
                item.IsAvailable = true;
                _phoneContext.Update(item);
                _phoneContext.SaveChanges();
            }
            catch{
                return "上架失敗";
            }
            if(!CheckStore(ItemId))
                return "上架失敗";
            return "上架成功";
        }
            public string DownItem(int ItemId){
            try{
                var item = (from a in _phoneContext.Item
                            where a.ItemId == ItemId
                            select a).SingleOrDefault();
                item.IsAvailable = false;
                _phoneContext.Update(item);
                _phoneContext.SaveChanges();
            }
            catch (Exception e){
                throw new Exception(e.ToString());
            }
            return "下架成功";
            
        }
        public bool CheckStore(int ItemId){
            var item = (from a in _phoneContext.Format
                        where a.ItemId == ItemId
                        select a.Store).Sum();
            if(item > 0){
                return true;
            }
            return false;
            
        }
    }
}
