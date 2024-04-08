using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace MP.Dtos
{
    public class ProductDto
    {
        public int ItemId { get; set; }
        public string ItemName{get;set;} = null!;
        public int ItemPriceMax{get;set;}
        public int ItemPriceMin{get;set;}
        public string ItemImg{get;set;} = null!;
    }
}
