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
                  }).ToList();

            return result;
        }

        #endregion
        #region 取得單筆商品規格

        #endregion
        #region 取得商品介紹
        
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