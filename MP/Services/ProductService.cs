using MP.Models;
using MP.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public IEnumerable<ProductDto> GetProduct()
        {
        var result = (from p in _phoneContext.Item
                  join f in _phoneContext.Format on p.ItemId equals f.ItemId
                  group new { p, f } by new { p.ItemId, p.ItemName,f.Brand } into g
                  select new ProductDto
                  {
                      ItemId = g.Key.ItemId,
                      Brand = g.Key.Brand,
                      ItemName = g.Key.ItemName,
                      ItemPriceMax = g.Max(x => x.f.ItemPrice),
                      ItemPriceMin = g.Min(x => x.f.ItemPrice),
                      ItemImg = (from img in _phoneContext.Img
                             where img.FormatId == g.Min(x => x.f.FormatId)
                             orderby img.Id
                             select img.ItemImg).FirstOrDefault()
                  });

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
                ItemPrice = format.ItemPrice
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
        public string ProductQA(int ItemId, string User,string content)
        {
            var item = _phoneContext.Item.SingleOrDefault(i => i.ItemId == ItemId);
            if (item != null)
            {
                QA qa = new QA
                {

                    ItemId = ItemId,
                    Account=User,
                    Content=content,
                    CreateTime=DateTime.Now

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