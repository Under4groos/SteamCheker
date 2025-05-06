namespace SteamCheker.Lib.Models
{
    public class SteamUser
    {
        // Fields to store user information
        public string? AccountName { get; set; }
        public string? PersonaName { get; set; }
        public string? SteamID { get; set; }
        public SteamUser()
        {

        }
        // Constructor to initialize the fields
        public SteamUser(string accountName, string personaName, string steamID)
        {
            AccountName = accountName;
            PersonaName = personaName;
            SteamID = steamID;
        }
    }
}
