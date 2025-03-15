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

        for(int i = 1; i <= usersCount; i++)
        {
            string dataString = $"({i},{i}{i}{i}{i})";
            storage.AddOrUpdateUser(dataString);
        }
    }
}
