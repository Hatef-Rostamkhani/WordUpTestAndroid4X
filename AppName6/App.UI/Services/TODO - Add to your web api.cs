using Domain;
using MSharp.Framework;
using MSharp.Framework.Api;
using System;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Controllers.Api
{
    [RoutePrefix("api/v1/pushnotification")]
    [ApiAuthorize(Roles = "Member")]
    public class PushNotificationApiController : BaseApiController
    {
        public struct RegisterPushNotificationArgs
        {
            public string UserId;
            public string InstallationToken, PushNotificationToken, DeviceType;
        }

        [HttpPost, Route("register")]
        public IHttpActionResult Register(RegisterPushNotificationArgs args)
        {
            var device = UserDevice.FindByInstallationToken(args.InstallationToken)?.Clone() ?? new UserDevice();

            device.UserId = args.UserId;
            device.InstallationToken = args.InstallationToken;
            device.PushNotificationToken = args.PushNotificationToken;
            device.DeviceType = args.DeviceType;
            device.InstallationTime = LocalTime.Now;
            Database.Save(device);

            return Ok(device.PushNotificationToken);
        }

        [HttpPost, Route("unregister")]
        public IHttpActionResult Unregister(string installationToken)
        {
            var device = UserDevice.FindByInstallationToken(installationToken);
            if (device != null) Database.Update(device, d => d.PushNotificationToken = null);

            return Ok();
        }

        [HttpGet, Route("navigatioBadges")]
        public IHttpActionResult NavigatioBadges()
        {
            return Ok(new
            {
                XBadge = Member.CountUnreadX(),
                YBadge = Member.CountUnreadY()
                // ...
            });
        }

        [HttpPost, Route("UpdateAppIconBadge")]
        public IHttpActionResult UpdateAppIconBadge()
        {
            // TODO: Implement this.

            // var badgeCount = ...;
            // var activeDevices = Member.Devices...;
            // MSharp.Framework.Api.PushNotification.PushNotificationService.UpdateBadge(badgeCount, activeDevices);

            return Ok();
        }
    }
}