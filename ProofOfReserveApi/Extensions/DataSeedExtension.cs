using ProofOfReserveApi.Services;

namespace ProofOfReserveApi.Extensions;

public static class DataSeedExtension
{
    /// <summary>
    /// Pushes initial data into the user balance storage.
    /// </summary>
    /// <param name="app">The web app.</param>
    /// <param name="usersCount">The count of users to put in storage.</param>
    public static void PushInitialData(this WebApplication app, int usersCount = 8)
    {
        var storage = app.Services.GetRequiredService<IUserBalanceStorage>();

        for (int i = 1; i <= usersCount; i++)
        {
            string balance = GenerateBalanceValue(i);
            string dataString = $"({i},{balance})";
            storage.AddOrUpdateUser(dataString);
        }
    }

    private static string GenerateBalanceValue(int currentUserId)
    {
        string balance = string.Empty;
        do
        {
            balance += currentUserId.ToString();
        } 
        while (balance.Length < 4); //We want balance walues for around 4 digits long.

        return balance;
    }
}
