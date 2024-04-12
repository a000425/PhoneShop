namespace MP.Dtos
{
    public class FrontQADto
    {
        public string Account { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime CreateTime { get; set; }

        public string? Reply { get; set; }

        public DateTime? ReplyTime { get; set; }
    }
}
