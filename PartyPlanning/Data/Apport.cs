namespace PartyPlanning.Data
{
    public class Apport
    {
        public Guid IdApport { get; set; }
        public string Type { get; set; }
        public string Nom { get; set; }
        public int Quantity { get; set; }

        public virtual Participation? Participation { get; set; }

        public Guid? IdParty { get; set; }
        public virtual Party? Party { get; set; }

        public Guid? IdUser { get; set; }
        public virtual PartyUser? User { get; set; }

    }
}