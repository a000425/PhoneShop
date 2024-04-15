using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using MP.Models;
using MP.Dtos;

namespace MP.Dtos
{
    public class OrderInfoDto
    {
        public string Brand { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Space { get; set; } = null!;
        public int ItemNum { get; set; }
        public string Name { get; set; } = null!;
        public string Cellphone { get; set; }
        public string Email { get; set; } = null!;
        public string Address { get; set; }

    }
}