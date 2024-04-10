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
                  group new { p, f } by new { p.ItemId, p.ItemName } into g
                  select new ProductDto
                  {
                      ItemId = g.Key.ItemId,
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
            // Console.WriteLine("請選擇您想要的Color和Space：");
            foreach (var item in colorandspace)
            {
                Console.WriteLine($"Color: {item.Color}, Space: {item.Space}");
            }
            // Console.WriteLine("請輸入您選擇的Color：");
            // string userColor = Console.ReadLine();
            // Console.WriteLine("請輸入您選擇的Space：");
            // string userSpace = Console.ReadLine();
            // Console.WriteLine("請輸入您選擇的數量：");
            // int userNum = int.Parse(Console.ReadLine());
            var result = (from item in _phoneContext.Item  
            join format in _phoneContext.Format on item.ItemId equals format.ItemId
            join img in _phoneContext.Img on format.FormatId equals img.FormatId
            where item.ItemId == ItemId /*&& format.Store!=0 && format.Color==userColor && format.Space==userSpace && userNum<format.Store*/
            select new ItemDto
            {
                FormatId = format.FormatId,
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