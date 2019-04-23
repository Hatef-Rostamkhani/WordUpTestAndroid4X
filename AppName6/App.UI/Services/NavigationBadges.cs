namespace UI.Services
{
    using Domain;
    using System;
    using System.Threading.Tasks;
    using Zebble;

    public class NavigationBadges
    {
        // TODO: For each tab, menu item or button that you need to show a badge, define a member here.
        // Then you can bind the UI elements to them.
        public static readonly Bindable<int> XBadge = new Bindable<int>();
        public static readonly Bindable<int> YBadge = new Bindable<int>();
        // ...

        class Info
        {
            public int XBadge, YBadge/*, ...*/;
        }

        public static async Task Refresh()
        {
            var info = await Api.Get<Info>("api/v1/pushnotification/navigatioBadges", OnError.Ignore);
            if (info == null) return;

            XBadge.Value = info.XBadge;
            YBadge.Value = info.YBadge;
        }
    }
}