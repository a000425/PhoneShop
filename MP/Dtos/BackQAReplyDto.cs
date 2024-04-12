namespace MP.Dtos
{
    public class BackQAReplyDto
    {

        public string ItemName { get; set; }

        public string Account { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime CreateTime { get; set; }

        public string? Reply { get; set; }

        public DateTime? ReplyTime { get; set; }
    }
}
