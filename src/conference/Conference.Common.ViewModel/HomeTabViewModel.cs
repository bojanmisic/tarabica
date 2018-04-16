namespace Conference.Common.ViewModel
{
    using OpenMVVM.Core.Pages;

    public class HomeTabViewModel : PageItemViewModel<object>
    {
        private bool initialized;

        public HomeTabViewModel(MultiPageViewModel<object> parentMultiPage)
            : base(parentMultiPage)
        {
            this.Title = "Home";
            this.Icon = "ticon-home";
        }

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
