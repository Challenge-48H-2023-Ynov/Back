namespace PartyPlanning.Data
{
    public class Party
    {
        public Guid IdParty { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Adresse { get; set; }
        public DateTime Date { get; set; }

        public ICollection<PartyUser>? Members { get; set; }
        public ICollection<Apport>? Apports { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}