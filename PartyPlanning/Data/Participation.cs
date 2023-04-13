namespace PartyPlanning.Data
{
    public class Participation
    {
        public Guid IdParty { get; set; }
        public virtual Party? Party { get; set; }
        public Guid IdUser { get; set; }
        public virtual PartyUser? User { get; set; }
        public string Role { get; set; }
        public int PlaceDispo { get; set; }
        public bool IsSAM { get; set; }
        public DateTime DateArrivee { get; set; }
        public DateTime DateDepart { get; set; }


        public ICollection<Apport>? Apports { get; set; }

    }
}
