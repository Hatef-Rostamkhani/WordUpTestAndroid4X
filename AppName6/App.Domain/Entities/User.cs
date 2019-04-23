namespace Domain
{
    using System;
    using System.Threading.Tasks;
    using Zebble;
    using Zebble.Device;

    public class User
    {
        public static User Current { get; set; }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => FirstName + " " + LastName;

        public static async Task<bool> LoadFromSessionToken()
        {
            var token = Api.GetSessionToken();

            if (token.LacksValue()) return false;

            Task onRefreshed(User user)
            {
                Current = user;
                if (user == null) Log.Error("Failed to load the user for current session token.");
                return Task.CompletedTask;
            }

            Current = await Api.Get<User>("api/v1/member/FindByToken", new { token },
                cacheChoice: ApiResponseCache.PreferThenUpdate, refresher: onRefreshed);

            return Current != null;
        }
    }
}