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
        #region 分頁方法
        public IEnumerable<ForPagingDto<ProductDto>> Page(IEnumerable<ProductDto> product, int pageNum)
        {
            ForPagingDto<ProductDto> pagingDto = new ForPagingDto<ProductDto>();
            int totalitem = product.Count();
            pagingDto.NowPage = pageNum;
            pagingDto.MaxPage = (int)Math.Ceiling((double)totalitem / pagingDto.Pageitem);
            pagingDto.Itemcount = totalitem;
            pagingDto.SetRightPage();
            var pageProduct = product.Skip((pagingDto.NowPage - 1) * pagingDto.Pageitem).Take(pagingDto.Pageitem);
            pagingDto.Items = pageProduct.ToList();
            return new List<ForPagingDto<ProductDto>> { pagingDto };
        }
        #endregion
        #region 取得所有商品
        public IEnumerable<ForPagingDto<ProductDto>> GetProduct(int sortway, int nowPage)
        {
            var result = (from p in _phoneContext.Item
                          join f in _phoneContext.Format on p.ItemId equals f.ItemId
                          where p.IsAvailable == true
                          group new { p, f } by new { p.ItemId, p.ItemName, f.Brand } into g
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
            if (sortway == 0)
            {
                result = result.OrderByDescending(x => x.ItemId);
            }
            else if (sortway == 1)
            {
                result = result.OrderBy(x => x.ItemId);
            }
            else if (sortway == 2)
            {
                result = result.OrderByDescending(x => x.ItemPriceMin);
            }
            else if (sortway == 3)
            {
                result = result.OrderBy(x => x.ItemPriceMin);
            }
            else
            {
                throw new ArgumentException("無此排序方式");
            }
            return Page(result, nowPage);
        }
        #endregion
        #region 熱銷商品
        public IEnumerable<ProductDto> GetHotProduct(int sortway)
        {
            var result = (from p in _phoneContext.Item
                          join f in _phoneContext.Format on p.ItemId equals f.ItemId
                          join oi in _phoneContext.OrderItem on p.ItemId equals oi.ItemId into orderItemsGroup
                          orderby orderItemsGroup.Sum(x => x.ItemNum) descending
                          where p.IsAvailable == true
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

            if (sortway == 0)
            {
                result = result.OrderByDescending(x => x.ItemId);
            }
            else if (sortway == 1)
            {
                result = result.OrderBy(x => x.ItemId);
            }
            else if (sortway == 2)
            {
                result = result.OrderByDescending(x => x.ItemPriceMin);
            }
            else if (sortway == 3)
            {
                result = result.OrderBy(x => x.ItemPriceMin);
            }
            else
            {
                throw new ArgumentException("無此排序方式");
            }
            return result;
        }
        #endregion
        #region 取得品牌所有商品與排序方式
        public IEnumerable<ForPagingDto<ProductDto>> GetProductByBrand(string Brand, int sortway, int nowPage)
        {
            var result = (from p in _phoneContext.Item
                          join f in _phoneContext.Format on p.ItemId equals f.ItemId
                          where f.Brand == Brand && p.IsAvailable == true
                          group new { p, f } by new { p.ItemId, p.ItemName, f.Brand } into g
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
            if (sortway == 0)
            {
                result = result.OrderByDescending(x => x.ItemId);
            }
            else if (sortway == 1)
            {
                result = result.OrderBy(x => x.ItemId);
            }
            else if (sortway == 2)
            {
                result = result.OrderByDescending(x => x.ItemPriceMin);
            }
            else if (sortway == 3)
            {
                result = result.OrderBy(x => x.ItemPriceMin);
            }
            else
            {
                throw new ArgumentException("無此排序方式");
            }

            return Page(result, nowPage);
        }
        #endregion
        #region 取得商品一覽(價格)
        public IEnumerable<ForPagingDto<ProductDto>> GetProductByPrice(int MaxPrice, int MinPrice, int sortway, int nowPage)
        {
            var result = (from p in _phoneContext.Item
                          join f in _phoneContext.Format on p.ItemId equals f.ItemId
                          where p.IsAvailable == true
                          group new { p, f } by new { p.ItemId, p.ItemName, f.Brand } into g
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
            if (sortway == 0)
            {
                result = result.OrderByDescending(x => x.ItemId);
            }
            else if (sortway == 1)
            {
                result = result.OrderBy(x => x.ItemId);
            }
            else if (sortway == 2)
            {
                result = result.OrderByDescending(x => x.ItemPriceMin);
            }
            else if (sortway == 3)
            {
                result = result.OrderBy(x => x.ItemPriceMin);
            }
            else
            {
                throw new ArgumentException("無此排序方式");
            }

            return Page(result, nowPage);
        }
        #endregion
        #region 取得ItemId的所有商品規格
        public IEnumerable<ItemDto> GetProductById(int ItemId)
        {
            var colorandspace = (from format in _phoneContext.Format
                                 where format.ItemId == ItemId
                                 select new { Color = format.Color, Space = format.Space }).Distinct().ToList();
            foreach (var item in colorandspace)
            {
                Console.WriteLine($"Color: {item.Color}, Space: {item.Space}");
            }
            var result = (from item in _phoneContext.Item
                          join format in _phoneContext.Format on item.ItemId equals format.ItemId
                          join img in _phoneContext.Img on format.FormatId equals img.FormatId into imgGroup
                          from img in imgGroup.DefaultIfEmpty()
                          where item.ItemId == ItemId && item.IsAvailable == true
                          select new ItemDto
                          {
                              FormatId = format.FormatId,
                              ItemId = item.ItemId,
                              Store = format.Store,
                              //ItemImg = img.ItemImg,
                              Brand = format.Brand,
                              ItemName = item.ItemName,
                              Color = format.Color,
                              Space = format.Space,
                              ItemPrice = format.ItemPrice,
                              Instruction = item.Instruction,
                              ItemImg = (from f in _phoneContext.Format
                                         join im in _phoneContext.Img on f.FormatId equals im.FormatId
                                         where f.ItemId == ItemId
                                         select im.ItemImg).ToList()
                          }).GroupBy(F => F.FormatId).Select(g => g.First());
            return result;
        }
        #endregion
        #region 取得商品ID & 價格
        public Format GetId(string color, string space, int ItemId)
        {
            var result = (from a in _phoneContext.Format
                          where a.Color == color && a.Space == space && a.ItemId == ItemId
                          select new Format
                          {
                              FormatId = a.FormatId,
                              Brand = a.Brand,
                              Color = a.Color,
                              Space = a.Space,
                              ItemPrice = a.ItemPrice,
                              ItemId = a.ItemId,
                              Store = a.Store
                          }).SingleOrDefault();
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
                              Account = p.Account,
                              Content = p.Content,
                              CreateTime = p.CreateTime,
                              Reply = p.Reply,
                              ReplyTime = p.ReplyTime
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
        #region 推薦系統(相似)
        private static Dictionary<string, int> _colorDictionary = new Dictionary<string, int>
        {
            { "Blue", 0 },
            { "Black", 1 },
            { "White", 2 },
            { "Gold", 3 },
            { "Red", 4 },
        };
        private static Dictionary<string, int> _spaceMapping = new Dictionary<string, int>
        {
            { "128GB", 0 },
            { "256GB", 1 },
            { "512GB", 2 },
            { "1TB", 3 }
        };
        private static Dictionary<string, int> _brandMapping = new Dictionary<string, int>
        {
            { "Apple", 0 },
            { "SAMSUNG", 1 },
            { "ASUS", 2 },
            { "SONY", 3 },
            { "vivo", 4 },
            { "htc", 5 },
            { "OPPO", 6 },
            { "realme", 7 },
            { "Nokia", 8 },
            { "小米", 9 },
            { "HUAWEI", 10 },
            { "Google", 11 }
        };
        private static readonly int[] PriceRanges = { 5000, 10000, 15000, 20000, 25000, 30000, 35000, 40000 };
        private static int VectorLength = PriceRanges.Length + 1;

        private static double[] GetColorVector(string color)
        {
            var vector = new double[_colorDictionary.Count];
            if (_colorDictionary.ContainsKey(color))
            {
                vector[_colorDictionary[color]] = 1;
            }
            return vector;
        }
        private static double[] GetSpaceVector(string space)
        {
            var vector = new double[_spaceMapping.Count];
            if (_spaceMapping.ContainsKey(space))
            {
                vector[_spaceMapping[space]] = 1;
            }
            return vector;
        }
        private static double[] GetBrandVector(string brand)
        {
            var vector = new double[_brandMapping.Count];
            if (_brandMapping.ContainsKey(brand))
            {
                vector[_brandMapping[brand]] = 3;
            }
            return vector;
        }
        private static double[] GetPriceVector(int price)
        {
            var vector = new double[VectorLength];
            int index = PriceRanges.Length;
            for (int i = 0; i < PriceRanges.Length; i++)
            {
                if (price <= PriceRanges[i])
                {
                    index = i;
                    break;
                }
            }
            vector[index] = 2;
            return vector;
        }
        public static double[] GetFeatureVector(ItemDto product)
        {
            var colorVector = GetColorVector(product.Color);
            var spaceVector = GetSpaceVector(product.Space);
            var brandVector = GetBrandVector(product.Brand);
            var priceVector = GetPriceVector(product.ItemPrice);
            var featureVector = colorVector.Concat(spaceVector).Concat(brandVector).Concat(priceVector).ToArray();
            return featureVector;
        }
        private static double CosineSimilarity(double[] vectorA, double[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be of same length");

            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += Math.Pow(vectorA[i], 2);
                magnitudeB += Math.Pow(vectorB[i], 2);
            }

            magnitudeA = Math.Sqrt(magnitudeA);
            magnitudeB = Math.Sqrt(magnitudeB);

            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;

            return dotProduct / (magnitudeA * magnitudeB);
        }

        public List<ItemDto> SimilarProducts(ItemDto itemDto, int topN = 4)
        {
            var product = (from f in _phoneContext.Format
                            where f.ItemId == itemDto.ItemId && f.FormatId == itemDto.FormatId
                            select new ItemDto
                            {
                                ItemId = itemDto.ItemId,
                                FormatId = itemDto.FormatId,
                                Color = f.Color,
                                Space = f.Space,
                                ItemPrice = f.ItemPrice,
                                Brand = f.Brand
                            }).SingleOrDefault();

            if (product == null)
            {
                throw new ArgumentException("错误");
            }

            var productFeatureVector = GetFeatureVector(product);

            var similarProducts = (from i in _phoneContext.Item
                           join f in _phoneContext.Format on i.ItemId equals f.ItemId
                           join pi in _phoneContext.Img on f.FormatId equals pi.FormatId
                           join oi in _phoneContext.OrderItem on f.FormatId equals oi.FormatId into orderGroup
                           where i.ItemId != itemDto.ItemId
                           let similarity = CosineSimilarity(productFeatureVector, GetFeatureVector(new ItemDto { Color = f.Color, Space = f.Space, ItemPrice = f.ItemPrice, Brand = f.Brand}))
                           select new
                           {
                               Item = i,
                               Format = f,
                               Img = pi.ItemImg,
                               Similarity = similarity,
                               Count = orderGroup.Sum(og => og.ItemNum)
                           })
                           .AsEnumerable() // 將查詢結果加載到內存中
                           .GroupBy(p => new { p.Format.Brand, p.Item.ItemId, p.Format.FormatId, p.Format.Color, p.Format.Space, p.Item.ItemName, p.Format.ItemPrice })
                           .OrderByDescending(g => g.Max(x => x.Similarity))
                           .ThenByDescending(g => g.Sum(x => x.Count))
                           .Take(topN)
                           .Select(g => new ItemDto
                           {
                               ItemId = g.Key.ItemId,
                               Brand = g.Key.Brand,
                               FormatId = g.Key.FormatId,
                               Color = g.Key.Color,
                               Space = g.Key.Space,
                               ItemName = g.Key.ItemName,
                               ItemPrice = g.Key.ItemPrice,
                               ItemImg = g.Select(x => x.Img).ToList(),
                           })
                           .ToList();


            return similarProducts;
        }
        #endregion
        #region 推薦系統(其他人也購買)
        public List<ItemDto> Otherbuy(ItemDto itemDto, int topN = 4)
        {
            var duplicate = (from oi in _phoneContext.OrderItem
                            where oi.ItemId == itemDto.ItemId && oi.FormatId == itemDto.FormatId
                            select new ItemDto{
                                OrderId = oi.OrderId
                            }).ToList();
            
            var orderIds = duplicate.Select(d => d.OrderId).Distinct().ToList();

            var otherItems = (from oi in _phoneContext.OrderItem
                  join f in _phoneContext.Format on oi.FormatId equals f.FormatId
                  join o in _phoneContext.Order on oi.OrderId equals o.OrderId 
                  join img in _phoneContext.Img on oi.FormatId equals img.FormatId
                  join i in _phoneContext.Item on oi.ItemId equals i.ItemId 
                  where orderIds.Contains(oi.OrderId) && oi.ItemId != itemDto.ItemId
                  group new { oi, img } by new { f.Brand, i.ItemId, f.FormatId, f.Color, f.Space, i.ItemName, f.ItemPrice, img.ItemImg } into g
                  select new ItemDto
                  {
                      // OrderId = g.First().OrderId,
                      ItemId = g.Key.ItemId,
                      FormatId = g.Key.FormatId,
                      Brand = g.Key.Brand,
                      Color = g.Key.Color,
                      Space = g.Key.Space,
                      ItemName = g.Key.ItemName,
                      ItemPrice = g.Key.ItemPrice,
                      TotalCount = g.Sum(x => x.oi.ItemNum),
                      ItemImg = g.Select(x => x.img.ItemImg).ToList()
                  })
                  .OrderByDescending(x => x.TotalCount)
                  .Take(topN)
                  .ToList();



                otherItems = otherItems.GroupBy(x => new { x.ItemId, x.FormatId, x.Brand, x.ItemName, x.ItemPrice })
                       .Select(g => g.First())
                       .ToList();

                return otherItems;

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