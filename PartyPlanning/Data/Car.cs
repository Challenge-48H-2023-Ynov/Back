namespace PartyPlanning.Data;

public class Car
{
    public Guid IdCar { get; set; }
    public int PlaceDispo { get; set; }
    public bool IsSAM { get; set; }

    public virtual PartyUser? Owner { get; set; }
}