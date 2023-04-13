namespace PartyPlanning.Data
{
    public class Invitation
    {
        public Guid IdParty { get; set; }
        public virtual Party? Party { get; set; }
        public Guid IdUser { get; set; }
        public virtual PartyUser? User { get; set; }
    }
}
