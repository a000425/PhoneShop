namespace MP.Dtos
{
    public class CartDto
    {
        public string ItemName { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Space { get; set; } = null!;
        public int ItemNum { get; set; }
        public int ItemPrice { get; set; }
    }
}
