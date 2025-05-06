using Microsoft.Win32;
using SteamCheker.Lib.Models;
using System.Text.RegularExpressions;

namespace SteamCheker.Lib
{
    public class SteamParser : IDisposable
    {
        private const string Regex_UsersPattern = "\\\"[0-9]+?\\\"[\\w\\W]+?\\{[\\w\\W]+?\\}";
        public string? SteamPath
        {
            get; private set;
        }
        public string? loginusers_vdf
        {
            get; private set;
        }

        public List<SteamUser> SteamUsers { get; private set; }

        public SteamParser()
        {
            SteamUsers = new List<SteamUser>();

        }
        public void Init()
        {

            using (RegistryKey currentUserKey = Registry.CurrentUser)
            {
                using (RegistryKey ValveSteam = currentUserKey.OpenSubKey("SOFTWARE\\Valve\\Steam"))
                {
                    SteamPath = Path.GetFullPath((ValveSteam.GetValue("SteamPath") ?? string.Empty).ToString());
                    if (!Directory.Exists(SteamPath))
                        throw new Exception("Directory not found!");

                    loginusers_vdf = Path.GetFullPath(Path.Combine(SteamPath, "config\\loginusers.vdf"));
                    if (!File.Exists(loginusers_vdf))
                        throw new Exception("File not found!");

                    string readFile = File.ReadAllText(loginusers_vdf);
                    if (string.IsNullOrEmpty(readFile))
                        throw new Exception("File is Empty!");
                    SteamUsers.Clear();
                    IEnumerable<string> usersf = (from str in Regex.Matches(readFile, Regex_UsersPattern) select str.Value);
                    foreach (string item in usersf)
                    {
                        string[] lines = item.Split('\n')
                            .Select(l =>
                                l.Trim()
                                .Replace("\t", ":")
                                .Replace("\"", ""))
                            .ToArray();
                        if (!lines.Any())
                            continue;


                        SteamUser steamUser = new SteamUser();

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (i == 1 && i == 10)
                                continue;
                            string _line = lines[i];

                            if (i == 0)
                            {
                                steamUser.SteamID = _line;
                                continue;
                            }

                            if (_line.Split("::") is string[] arr && arr.Length == 2)
                            {
                                switch (i)
                                {
                                    case 2:
                                        steamUser.AccountName = arr[1];
                                        break;
                                    case 3:
                                        steamUser.PersonaName = arr[1];
                                        break;
                                }


                            }

                        }
                        SteamUsers.Add(steamUser);
                    }


                }
            }

        }

        public void Dispose()
        {

        }
    }
}
