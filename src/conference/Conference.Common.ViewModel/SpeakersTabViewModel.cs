namespace Conference.Common.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Linq;

    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;

    public class SpeakersTabViewModel : PageItemViewModel<object>
    {
        private bool initialized;

        private ObservableCollection<SpeakerItemViewModel> speakers;

        private IMvvmCommand speakerSelectedCommand;

        public SpeakersTabViewModel(MultiPageViewModel<object> parentMultiPage)
            : base(parentMultiPage)
        {
            this.Title = "Predavači";
            this.Icon = "ticon-speaker";
        }

        public IMvvmCommand SpeakerSelectedCommand => this.speakerSelectedCommand ?? (this.speakerSelectedCommand = new ActionCommand<SpeakerItemViewModel>(
            (speaker) =>
            {
                this.ParentMultiPage.NavigationService.NavigateTo("SpeakerView", speaker);
            }));

        public ObservableCollection<SpeakerItemViewModel> Speakers
        {
            get
            {
                return this.speakers;
            }

            set
            {
                this.Set(ref this.speakers, value);
            }
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();

            if (!this.initialized)
            {
                var mainViewModel = this.ParentMultiPage as MainViewModel;

                if (mainViewModel != null)
                {
                    this.Speakers = new ObservableCollection<SpeakerItemViewModel>(mainViewModel.ConferenceData.Speakers.Select(s => new SpeakerItemViewModel(s, mainViewModel.ConferenceData, mainViewModel.ConferenceRepository)));
                }

                this.initialized = true;
            }
        }
    }
}
