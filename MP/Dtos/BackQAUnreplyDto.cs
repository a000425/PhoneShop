namespace MP.Dtos
{
    public class BackQAUnreplyDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }

        public string Account { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime CreateTime { get; set; }

    }
}
