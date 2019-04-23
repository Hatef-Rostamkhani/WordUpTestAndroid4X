namespace UI.Modules
{
    using System.Threading.Tasks;
    using Zebble;

    partial class MainMenu
    {
        public async Task Page1Tapped() => await Nav.Go<Pages.Page1>(PageTransition.Fade);

        public async Task Page2Tapped() => await Nav.Go<Pages.Page2>(PageTransition.Fade);

        public async Task Page3Tapped() => await Nav.Go<Pages.Page3>(PageTransition.Fade);

        public async Task ContactTapped() => await Nav.Go<Pages.Page4>(PageTransition.Fade);
    }
}