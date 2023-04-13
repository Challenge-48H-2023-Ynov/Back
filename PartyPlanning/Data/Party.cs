namespace PartyPlanning.Data
{
    public class Party
    {
        public Guid IdParty { get; set; }
        public Guid IdUser { get; set; }
        public virtual PartyUser? User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Adresse { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFin { get; set; }

        public ICollection<Participation>? Participations { get; set; }
        public ICollection<Apport>? Besoins { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<Invitation>? Invitations { get; set; }
    }
}