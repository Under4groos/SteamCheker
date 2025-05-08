
Библиотека для получения информации о Steam-аккаунтах на ПК. 

````cs
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

```cs
public class SteamUser
{
    // Fields to store user information
    public string? AccountName { get; set; }
    public string? PersonaName { get; set; }
    public string? SteamID { get; set; }
}
````

```
### Result - AccountName , PersonaName SteamID
```

TTsdt 76561198377054622
Avfrrr 76561198906840391
.Cool. 76561198226505242

```

```
