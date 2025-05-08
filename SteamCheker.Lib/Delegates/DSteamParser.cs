namespace SteamCheker.Lib.Delegates
{
    public class DSteamParser
    {
        public delegate void Loaded();
        public delegate void Error(string message);
    }
}
