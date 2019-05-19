namespace Conference.Common.ViewModel
{
    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;

    public class InfoTabViewModel : PageItemViewModel<object>
    {
        private bool initialized;

        private IMvvmCommand privacyPolicyCommand;

        public InfoTabViewModel(MultiPageViewModel<object> parentMultiPage)
            : base(parentMultiPage)
        {
            this.Title = "Info";
            this.Icon = "ticon-location";
        }

        public IMvvmCommand PrivacyPolicyCommand => this.privacyPolicyCommand ?? (this.privacyPolicyCommand = new ActionCommand(
            () =>
            {
                var parentViewModel = this.ParentMultiPage as MainViewModel;
                parentViewModel?.WebLauncher.TryOpenUri("https://tarabica.org/Donation/Terms");
            }));

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();

            if (!this.initialized)
            {
                this.initialized = true;
            }
        }
    }
}
