namespace MP.Dtos
{
    public class CartDto
    {
        // public int cartId { get; set; }
<<<<<<< HEAD
        public int Id { get; set; }
=======
>>>>>>> eb60b6660a6771860b5eb140b627fa53ccb6edcf
        public string Brand { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Space { get; set; } = null!;
        public int ItemNum { get; set; }
        public int ItemPrice { get; set; }
        public List<CartDto> Items { get; set; }

    }
}
