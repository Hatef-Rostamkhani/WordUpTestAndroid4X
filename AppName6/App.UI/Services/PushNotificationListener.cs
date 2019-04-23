namespace UI.Services
{
    using System;
    using Domain;
    using Zebble;
    using Newtonsoft.Json.Linq;
    using System.Threading.Tasks;
    using Zebble.Device;

    public class PushNotificationListener
    {
        internal static void Initialize()
        {
            PushNotification.ReceivedMessage.Handle(OnReceivedMessage);
            PushNotification.Registered.Handle(OnRegistered);
            PushNotification.UnRegistered.Handle(OnUnRegistered);

            App.WentIntoBackground += () => Thread.Pool.Run(Api.UpdateAppIconBadge);
        }

        internal static async Task Register()
        {
            if (User.Current == null)
                throw new Exception("A non-logged in user was registered for push notification!");

            await PushNotification.Register();
        }

        /// <param name="registerationToken">The registration token provided by Apple, Google or Microsoft.</param>
        static async Task OnRegistered(string registerationToken)
        {
            if (User.Current == null)
                throw new Exception("A non-logged in user was registered for push notification!");

            var successful = await Api.Post("api/v1/authentication/push-notification/register",
                new
                {
                    User = User.Current.Id,
                    InstallationToken = UIRuntime.GetInstallationToken(),
                    PushNotificationToken = registerationToken,
                    DeviceType = OS.Platform
                });

            if (!successful)
                throw new Exception("The server failed to register the push notification token.");
        }

        static async Task OnUnRegistered()
        {
            var successful = await Api.Post("api/v1/authentication/push-notification/unregister",
                new
                {
                    InstallationToken = UIRuntime.GetInstallationToken()
                });

            if (!successful)
                Log.Error("The server failed to unregister this device from receiving push notifications.");
        }

        static async Task OnReceivedMessage(NotificationMessage message)
        {
            Log.Message("Push notification data received! " + message.Data);

            if (!App.IsActive || Nav.CurrentPage == null)
            {
                // The app is not active and ready.
                Log.Warning("UI is not open and ready, so the notification was ignored!");
            }
            else
            {
                await ProcessAlertNotification(message.Data);
                await ProcessToastNotification(message.Data);
                await ProcessBadgeUpdate(message.Data);

                // Page data may be changed:
                Nav.DisposeCache();
            }
        }

        static Task ProcessAlertNotification(JObject data)
        {
            // Should display a message?

            var title = data["AlertTitle"]?.ToString();
            var body = data["AlertBody"]?.ToString();

            if (title.HasValue() || body.HasValue())
                return Alert.Show(title, body);
            else
                return Task.CompletedTask;
        }

        static Task ProcessToastNotification(JObject data)
        {
            // Should display a message?

            var message = data["ToastMessage"]?.ToString();
            if (message.HasValue()) return Alert.Toast(message);
            else return Task.CompletedTask;
        }

        static Task ProcessBadgeUpdate(JObject data)
        {
            // Often a push notification is handled by changing badge number on certain tabs or menu items.

            var xBadge = data["xBadge"].ToStringOrEmpty().TryParseAs<int>();
            if (xBadge.HasValue) NavigationBadges.XBadge.Set(xBadge.Value);

            var yBadge = data["yBadge"].ToStringOrEmpty().TryParseAs<int>();
            if (yBadge.HasValue) NavigationBadges.YBadge.Set(yBadge.Value);
            // ...

            return Task.CompletedTask;
        }
    }
}