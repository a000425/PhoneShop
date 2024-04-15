using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using MP.Models;

namespace MP.Dtos
{
    public class OrderItemDto
    {
        public int OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public int TotalPrice { get; set; }
        public string OrderStatus { get; set; }
    }
}