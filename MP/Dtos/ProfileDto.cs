using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace MP.Dtos
{
    public class ProfileDto
    {
        public string Account1 { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Cellphone { get; set; }
        public string Email { get; set; } = null!;
    }
}
