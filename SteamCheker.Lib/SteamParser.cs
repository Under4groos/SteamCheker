using Microsoft.Win32;
using SteamCheker.Lib.Models;
using System.Text.RegularExpressions;
using static SteamCheker.Lib.Delegates.DSteamParser;

namespace SteamCheker.Lib
{
    public class SteamParser : IDisposable
    {

        public string? SteamPath
        {
            get; private set;
        }
        public string? loginusers_vdf
        {
            get; private set;
        }


        public List<SteamUser> SteamUsers { get; private set; }
        public SteamUser this[int index]
        {
            get
            {
                // если индекс имеется в массиве
                if (index >= 0 && index < SteamUsers.Count)
                    return SteamUsers[index];
                else
                    throw new ArgumentOutOfRangeException();
            }
            set
            {
                // если индекс есть в массиве
                if (index >= 0 && index < SteamUsers.Count)
                    SteamUsers[index] = value;
            }
        }
        public Loaded? OnLoaded;
        public Error? OnError;


        private RegistryKey currentUserKey = Registry.CurrentUser;
        private RegistryKey ValveSteam;
        private const string Regex_UsersPattern = "\\\"[0-9]+?\\\"[\\w\\W]+?\\{[\\w\\W]+?\\}";


        public SteamParser()
        {
            SteamUsers = new List<SteamUser>();

        }
        private void OpenRegistry()
        {
            ValveSteam = currentUserKey.OpenSubKey("SOFTWARE\\Valve\\Steam");
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
                        .Replace("\t", ":").Replace("::", ":")
                        .Replace("\"", ""))
                    .ToArray();
                if (!lines.Any())
                    continue;



                SteamUser steamUser = new SteamUser();

                int linec = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (i == 0)
                    {
                        steamUser.SteamID = line;
                        continue;
                    }

                    string[] spl = line.Split(':');
                    if (spl.Length == 2)
                    {
                        string vaslue = line.Substring(spl.First().Length + 1);
                        if (line.StartsWith("AccountName"))
                        {
                            steamUser.AccountName = vaslue;
                            continue;
                        }

                        if (line.StartsWith("PersonaName"))
                        {
                            steamUser.PersonaName = vaslue;
                            continue;
                        }

                    }
                }







                #region hide
                //for (int i = 0; i < lines.Length; i++)
                //{
                //    if (i == 1 && i == 10)
                //        continue;
                //    string _line = lines[i];

                //    if (i == 0)
                //    {
                //        steamUser.SteamID = _line;
                //        continue;
                //    }

                //    if (_line.Split("::") is string[] arr && arr.Length == 2)
                //    {
                //        switch (i)
                //        {
                //            case 2:
                //                steamUser.AccountName = arr[1];
                //                break;
                //            case 3:
                //                steamUser.PersonaName = arr[1];
                //                break;
                //        }


                //    }

                //} 
                #endregion
                SteamUsers.Add(steamUser);
            }
        }
        public void Init()
        {

            try
            {
                OpenRegistry();
                OnLoaded?.Invoke();
            }
            catch (Exception e)
            {

                OnError?.Invoke(e.Message);
            }

        }

        public void Dispose()
        {
            currentUserKey?.Dispose();
            ValveSteam?.Dispose();
        }
    }
}
