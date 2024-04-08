using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using MP.Models;

namespace MP.Dtos
{
    public class ItemDto
    {
        public string? ItemImg { get; set; }
        public string Brand { get; set; } = null!;
        public string Instruction { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Space { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public int ItemPrice { get; set; }
        public List<Img>? ImgList{get;set;}
    }
}
