﻿namespace Conference.App.Android
{
    using global::Android.App;
    using global::Android.Graphics;
    using global::Android.OS;
    using global::Android.Views;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.WebView.Android;

    [Activity(Label = "@string/ApplicationName")]
    public class MainActivity : WebViewActivity
    {
        public MainActivity()
            : base(new ViewModelLocator())
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.RequestWindowFeature(WindowFeatures.NoTitle);

            this.SetContentView(Resource.Layout.Main);
            var webView = this.FindViewById<global::Android.Webkit.WebView>(Resource.Id.webView);
            this.InitializeWebView(webView);

            // Set StatusBar Background
            Window window = this.Window;
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                window.SetStatusBarColor(Color.Rgb(0, 171, 248));
                window.SetNavigationBarColor(Color.Rgb(2, 109, 189));
            }
        }
    }
}

