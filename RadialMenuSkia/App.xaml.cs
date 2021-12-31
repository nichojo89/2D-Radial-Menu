using System;
using SkiaSharp.Extended.Iconify;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RadialMenuSkia
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SKTextRunLookup.Instance.AddFontAwesome();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
