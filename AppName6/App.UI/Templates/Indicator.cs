namespace UI.Templates
{
    using System;
    using System.Threading.Tasks;
    using Zebble;

    public class Indicator : View
    {
        public bool Block;
        public ImageView Image { get; }

        public Indicator()
        {
            Image = new ImageView
            {
                Id = "Spinner",
                Path = "Images/Icons/Spinner.png",
                Alignment = Alignment.Middle,
            }.ZIndex(1000).Width(100.Percent());
        }

        public override async Task OnInitializing()
        {
            await base.OnInitializing();
            await Add(Image);

            Image.Animate(new Animation
            {
                Change = () => Image.Rotation(359),
                Repeats = -1,
                Easing = AnimationEasing.Linear
            }).RunInParallel();
        }

        public override Task ApplyStyles()
        {
            Css.Margin(left: (Zebble.Device.Screen.Width - ActualWidth) / 2);
            return base.ApplyStyles();
        }
    }
}
