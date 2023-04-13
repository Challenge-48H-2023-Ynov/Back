namespace PartyPlanning.Data
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string Organisateur = "Organisateur";
        public const string Participant = "Participant";
        public const string Rat = "Rat";
        public const string Pilote = "Pilote";

        public static string[] GetAllRoles() => new string[] { Admin, Organisateur, Participant, Rat, Pilote };
    }
}