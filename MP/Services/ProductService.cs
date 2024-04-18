using MP.Models;
using MP.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace MP.Services
{
    public class ProductService
    {
        private readonly PhoneContext _phoneContext;
        public ProductService(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }
        
        #region 取得所有商品
        public IEnumerable<ProductDto> GetProduct(int sortway)
        {
        var result = (from p in _phoneContext.Item
                  join f in _phoneContext.Format on p.ItemId equals f.ItemId
                  where p.IsAvailable==true
                  group new { p, f } by new { p.ItemId, p.ItemName,f.Brand } into g
                  select new ProductDto
                  {
                      ItemId = g.Key.ItemId,
                      Brand = g.Key.Brand,
                      ItemName = g.Key.ItemName,
                      ItemPriceMin = g.Min(x => x.f.ItemPrice),
                      ItemImg = (from img in _phoneContext.Img
                             where img.FormatId == g.Min(x => x.f.FormatId)
                             orderby img.Id
                             select img.ItemImg).FirstOrDefault()
                  });
            if(sortway==0)
            {
                result = result.OrderByDescending(x => x.ItemId);
            }
            else if(sortway==1)
            {
                result = result.OrderBy(x=>x.ItemId);
            }
            else if(sortway==2)
            {
                result=result.OrderByDescending(x=>x.ItemPriceMin);
            }
            else if(sortway==3)
            {
                result=result.OrderBy(x=>x.ItemPriceMin);
            }
            else
            {
                throw new ArgumentException("無此排序方式");
            }
            return result;
        }
        #endregion
        #region 熱銷商品
        public IEnumerable<ProductDto> GetHotProduct(int sortway){
            var result = (from p in _phoneContext.Item
              join f in _phoneContext.Format on p.ItemId equals f.ItemId
              join oi in _phoneContext.OrderItem on p.ItemId equals oi.ItemId into orderItemsGroup
              orderby orderItemsGroup.Sum(x => x.ItemNum) descending
              group new { p, f, orderItemsGroup } by new { p.ItemId, p.ItemName, f.Brand } into g
              select new ProductDto
              {
                  ItemId = g.Key.ItemId,
                  Brand = g.Key.Brand,
                  ItemName = g.Key.ItemName,
                  ItemPriceMin = g.Min(x => x.f.ItemPrice),
                  ItemImg = (from img in _phoneContext.Img
                             where img.FormatId == g.Min(x => x.f.FormatId)
                             orderby img.Id
                             select img.ItemImg).FirstOrDefault()
              }).Take(8);

            if(sortway==0)
            {
                result = result.OrderByDescending(x => x.ItemId);
            }
            else if(sortway==1)
            {
                result = result.OrderBy(x=>x.ItemId);
            }
            else if(sortway==2)
            {
                result=result.OrderByDescending(x=>x.ItemPriceMin);
            }
            else if(sortway==3)
            {
                result=result.OrderBy(x=>x.ItemPriceMin);
            }
            else
            {
                throw new ArgumentException("無此排序方式");
            }
            return result;
        }
        #endregion
        #region 取得品牌所有商品與排序方式
        public IEnumerable<ProductDto> GetProductByBrand(string Brand ,int sortway){
           var result = (from p in _phoneContext.Item
                  join f in _phoneContext.Format on p.ItemId equals f.ItemId
                  where f.Brand == Brand
                  group new { p, f } by new { p.ItemId, p.ItemName,f.Brand } into g
                  select new ProductDto
                  {
                      ItemId = g.Key.ItemId,
                      Brand = g.Key.Brand,
                      ItemName = g.Key.ItemName,
                      ItemPriceMin = g.Min(x => x.f.ItemPrice),
                      ItemImg = (from img in _phoneContext.Img
                             where img.FormatId == g.Min(x => x.f.FormatId)
                             orderby img.Id
                             select img.ItemImg).FirstOrDefault()
                     
                  });
            if(sortway==0)
            {
                result = result.OrderByDescending(x => x.ItemId);
            }
            else if(sortway==1)
            {
                result = result.OrderBy(x=>x.ItemId);
            }
            else if(sortway==2)
            {
                result=result.OrderByDescending(x=>x.ItemPriceMin);
            }
            else if(sortway==3)
            {
                result=result.OrderBy(x=>x.ItemPriceMin);
            }
            else
            {
                throw new ArgumentException("無此排序方式");
            }

            return result;
        }
        #endregion
        #region 取得商品一覽(價格)
        public IEnumerable<ProductDto> GetProductByPrice(int MaxPrice,int MinPrice,int sortway){
            var result = (from p in _phoneContext.Item
                  join f in _phoneContext.Format on p.ItemId equals f.ItemId
                  group new { p, f } by new { p.ItemId, p.ItemName,f.Brand } into g
                  where g.Min(x => x.f.ItemPrice) <= MaxPrice && g.Min(x => x.f.ItemPrice) >= MinPrice
                  select new ProductDto
                  {
                      ItemId = g.Key.ItemId,
                      Brand = g.Key.Brand,
                      ItemName = g.Key.ItemName,
                      ItemPriceMin = g.Min(x => x.f.ItemPrice),
                      ItemImg = (from img in _phoneContext.Img
                             where img.FormatId == g.Min(x => x.f.FormatId)
                             orderby img.Id
                             select img.ItemImg).FirstOrDefault()
                  });
            if(sortway==0)
            {
                result = result.OrderByDescending(x => x.ItemId);
            }
            else if(sortway==1)
            {
                result = result.OrderBy(x=>x.ItemId);
            }
            else if(sortway==2)
            {
                result=result.OrderByDescending(x=>x.ItemPriceMin);
            }
            else if(sortway==3)
            {
                result=result.OrderBy(x=>x.ItemPriceMin);
            }
            else
            {
                throw new ArgumentException("無此排序方式");
            }

            return result;
        }
        #endregion
        #region 取得ItemId的所有商品規格
        public IEnumerable<ItemDto> GetProductById(int ItemId)
        {
            var colorandspace = (from format in _phoneContext.Format
                                 where format.ItemId==ItemId
                                 select new {Color = format.Color,Space = format.Space}).Distinct().ToList();
            foreach (var item in colorandspace)
            {
                Console.WriteLine($"Color: {item.Color}, Space: {item.Space}");
            }
            var result = (from item in _phoneContext.Item  
            join format in _phoneContext.Format on item.ItemId equals format.ItemId
            join img in _phoneContext.Img on format.FormatId equals img.FormatId
            where item.ItemId == ItemId 
            select new ItemDto
            {
                FormatId = format.FormatId,
                ItemId = item.ItemId,
                Store = format.Store,
                ItemImg = img.ItemImg,
                Brand = format.Brand,
                ItemName = item.ItemName,
                Color = format.Color,
                Space = format.Space,
                ItemPrice = format.ItemPrice,
                Instruction = item.Instruction
            }).ToList();
            return result;
        }
        #endregion
        #region 取得商品ID & 價格
        public Format GetId(string color, string space,int ItemId){
            var result = (from a in _phoneContext.Format
                        where a.Color == color && a.Space == space && a.ItemId == ItemId
                        select new Format{ 
                            FormatId = a.FormatId,
                            Brand = a.Brand,
                            Color = a.Color,
                            Space = a.Space,
                            ItemPrice = a.ItemPrice,
                            ItemId = a.ItemId,
                            Store = a.Store
                            } ).SingleOrDefault();
            return result;
        }
        #endregion
        #region 取得商品的所有QA
        public IEnumerable<FrontQADto> GetQAById(int itemId)
        {
            var result = (from p in _phoneContext.QA
                          join i in _phoneContext.Item on p.ItemId equals i.ItemId
                          where p.ItemId == itemId
                          select new FrontQADto
                          {
                              Account=p.Account,
                              Content=p.Content,
                              CreateTime=p.CreateTime,
                              Reply=p.Reply,
                              ReplyTime=p.ReplyTime
                          });

            return result;
            
        }
        #endregion
        #region 提問ProductQA
        public string ProductQA(int ItemId, string User, string content)
        {
            var item = _phoneContext.Item.SingleOrDefault(i => i.ItemId == ItemId);
            if (item != null)
            {
                var qa = new QA
                {
                    ItemId = ItemId,
                    Account = User,
                    Content = content,
                    CreateTime = DateTime.Now 
                };
                _phoneContext.QA.Add(qa);
                _phoneContext.SaveChanges();
                return "提問成功";
            }
            else
            {
                return "查無此商品QA";
            }
        }
        #endregion
    }
}
//查詢所有商品LINQ傳SQL寫法
/*SELECT
	ItemId,
    ItemName,
    MaxPrice,
    MinPrice,
    Img
FROM (
    SELECT
		p.ItemId,
        p.ItemName,
        MAX(f.MaxPrice) AS MaxPrice,
        MIN(f.MinPrice) AS MinPrice,
        (SELECT TOP 1 ItemImg FROM Img WHERE Img.FormatId = MIN(f.FormatId) ORDER BY Img.Id) AS Img
    FROM
        Item p
    JOIN (
        SELECT
            i.ItemId,
            MAX(f.ItemPrice) AS MaxPrice,
            MIN(f.ItemPrice) AS MinPrice,
            MIN(f.FormatId) AS FormatId
        FROM
            Item i
        JOIN Format f ON i.ItemId = f.ItemId
        GROUP BY
            i.ItemId
    ) f ON p.ItemId = f.ItemId
    GROUP BY
        p.ItemName,p.ItemId
) AS SubQuery;
*/