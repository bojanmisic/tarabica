namespace Conference.App.Ios
{
    using System;

    using cdeutsch;

    using Foundation;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.WebView;
    using OpenMVVM.WebView.Ios;

    using UIKit;

    public partial class WebViewController : OpenMVVM.WebView.Ios.WebViewController
    {
        public WebViewController(IntPtr handle)
            : base(handle, new ViewModelLocator())
        {

        }

        public override void ViewDidLoad()
        {
            base.webView = this.WebView;
            this.WebView.BackgroundColor = UIColor.FromRGB(2, 109, 189);
            base.ViewDidLoad();
        }
    }
}