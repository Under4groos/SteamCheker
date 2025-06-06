﻿

using SteamCheker.Lib;

using (SteamParser steam = new SteamParser())
{

    steam.OnLoaded += () =>
    {
        foreach (SteamCheker.Lib.Models.SteamUser user in steam.SteamUsers)
        {
            Console.WriteLine($"{user.PersonaName} {user.SteamID}");
        }
    };
    steam.OnError += (message) =>
    {
        Console.WriteLine(message);
    };

    steam.Init();
}