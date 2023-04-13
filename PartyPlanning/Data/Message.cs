namespace PartyPlanning.Data
{
    public class Message
    {
        public Guid IdMessage { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }

        public virtual Party? Party { get; set; }
    }
}