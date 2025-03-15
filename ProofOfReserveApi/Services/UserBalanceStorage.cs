using ProofOfReserveApi.Models;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace ProofOfReserveApi.Services;

public class UserBalanceStorage : IUserBalanceStorage
{
    //This is regex designed to capture user balance data from user balance string.
    //We have 2 capture groups. group with index 1 will have userId, group with index 2 will have user balance.
    //Group with index 0 is whole string. 
    //You can check this regex using https://regex101.com/r/tG1jF6/1 web app.
    public const string UserBalanceDataRegex = "\\((\\d+),(\\d+)\\)";

    private readonly ConcurrentDictionary<int, UserBalance> userBalances = new();
    private readonly ILogger logger;

    public UserBalanceStorage(ILogger<UserBalanceStorage> logger)
    {
        this.logger = logger;
    }

    public void AddOrUpdateUser(string userData)
    {
        var userBalance = ParseUserData(userData);
        if (userBalance == null) 
        {
            return;
        }

        bool haveUser = userBalances.TryGetValue(userBalance.Id, out UserBalance? oldBalance);
        if (haveUser)
        {
            oldBalance!.Balance = userBalance.Balance;
        }
        else
        {
            userBalances.TryAdd(userBalance.Id, userBalance);
        }
    }

    public List<UserBalance> GetAllUsers()
    {
        return userBalances.Values.ToList();
    }

    public UserBalance? GetUser(int userId)
    {
        bool haveUser = userBalances.TryGetValue(userId, out UserBalance? userBalance);
        return userBalance;
    }

    public bool UserExists(int userId)
    {
        return userBalances.ContainsKey(userId);
    }

    private UserBalance? ParseUserData(string userData) 
    {
        var match = Regex.Match(userData, UserBalanceDataRegex);
        if (!match.Success) 
        {
            logger.LogWarning("The value {0} have incorrect format. Will be ignored.", userData);
            return null;
        }

        return new(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value);
    }
}
