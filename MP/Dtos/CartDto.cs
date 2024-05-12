namespace MP.Dtos
{
    public class CartDto
    {
        // public int cartId { get; set; }
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Space { get; set; } = null!;
        public int ItemNum { get; set; }
        public int ItemPrice { get; set; }
        public int ItemStore{ get; set; }

        public int discount{ get ; set; }
        public List<CartDto> Items { get; set; }

    }
}
