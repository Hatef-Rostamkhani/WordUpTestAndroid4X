namespace UI.Pages
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Zebble;

    partial class Welcome
    {
        public override async Task OnInitializing()
        {
            await base.OnInitializing();

            await WhenShown(async () =>
            {
                await Waiting.Show(block: false);
                await Task.Delay(1.Seconds());
                await Thread.Pool.Run(LaunchFirstPage);
            });
        }

        static async Task LaunchFirstPage()
        {
            Page firstPage = null;

            if (await User.LoadFromSessionToken())
                firstPage = new Page2(); // TODO: Go to Home Page...
            else
                firstPage = new Page1(); // TODO: Go to Login / Register page

            await Nav.Go(firstPage, PageTransition.Fade);
        }
    }
}