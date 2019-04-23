namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Zebble;
    using Zebble.Device;

    /// <summary>
    /// Acts as a gateway to the server API. You can customise its behaviour if needed.
    /// </summary>
    public partial class Api : BaseApi
    {
        public static async Task<bool> Login(string email, string password)
        {
            var sessionToken = await Post<string>("api/v1/auth/login", new
            {
                Email = email,
                Password = password,
                installationToken = UIRuntime.GetInstallationToken()
            });

            if (sessionToken.LacksValue()) return false;

            IO.File("SessionToken.txt").WriteAllText(sessionToken);
            if (!await User.LoadFromSessionToken()) return false;
            await UI.Services.PushNotificationListener.Register();
            return true;
        }

        /// <summary>
        /// Updates the app icon's new data badge on all installations of the app for the current user (across different devices).
        /// </summary>
        public static Task UpdateAppIconBadge() => Post("api/v1/pushnotification/UpdateAppIconBadge", OnError.Ignore);

        public static async Task<bool> Logout()
        {
            var successful = await Post<bool>("api/v1/auth/logout", new
            {
                SessionToken = BaseApi.GetSessionToken(),
                InstallationToken = UIRuntime.GetInstallationToken() // To disable sending push notifications to this device.
            });

            if (successful)
            {
                IO.File("SessionToken.txt").Delete();
                User.Current = null;

                Nav.DisposeCache();
                await Api.DisposeCache();
            }

            return successful;
        }
    }
}