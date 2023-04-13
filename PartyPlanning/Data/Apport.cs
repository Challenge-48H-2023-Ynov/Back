namespace PartyPlanning.Data
{
    public class Apport
    {
        public Guid IdApport { get; set; }
        public string Type { get; set; }
        public string Nom { get; set; }
        public int Quantity { get; set; }

        public ICollection<Party>? Parties { get; set; }
        public virtual PartyUser? User { get; set; }
    }
}