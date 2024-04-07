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
                        join pi in (from img in _phoneContext.Img
                                    group img by img.FormatId into imgGroup
                                    select new
                                    {
                                        FormatId = imgGroup.Key,
                                        ItemImg = imgGroup.OrderBy(img => img.Id).FirstOrDefault().ItemImg
                                    })
                        on p.FormatId equals pi.FormatId into joined
                        from pi in joined.DefaultIfEmpty()
                        group new { p, pi } by new { p.ItemName, pi.ItemImg } into grouped
                        select new ProductDto
                        {
                            ItemName = grouped.Key.ItemName,
                            ItemPriceMax = grouped.Max(x => x.p.ItemPrice),
                            ItemPriceMin = grouped.Min(x => x.p.ItemPrice),
                            ItemImg = grouped.Key.ItemImg
                        });

            return result;
        }
        #endregion
        
        
    }
}
//查詢所有商品LINQ傳SQL寫法
/*SELECT
    ProductName,
    MaxPrice,
    MinPrice,
    Img
FROM (
    SELECT
        p.ItemName AS ProductName,
        MAX(p.ItemPrice) AS MaxPrice,
        MIN(p.ItemPrice) AS MinPrice,
        pi.ItemImg AS Img,
        ROW_NUMBER() OVER (PARTITION BY p.ItemName ORDER BY p.ItemName) AS RowNum
    FROM
        Item p
    LEFT JOIN (
        SELECT
            FormatId,
            ItemImg,
            ROW_NUMBER() OVER (PARTITION BY FormatId ORDER BY Id) AS RowNum
        FROM
            Img
    ) pi ON p.FormatId = pi.FormatId
    WHERE
        pi.RowNum = 1
    GROUP BY
        p.ItemName, pi.ItemImg
) AS SubQuery
WHERE
    RowNum = 1;
*/