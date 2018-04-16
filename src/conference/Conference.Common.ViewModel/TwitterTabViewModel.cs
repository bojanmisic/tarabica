namespace Conference.Common.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;

    public class TwitterTabViewModel : PageItemViewModel<object>
    {
        private bool loadingTweets;

        private ObservableCollection<TweetItemViewModel> tweets = new ObservableCollection<TweetItemViewModel>();

        private ICommand refreshTweetsCommand;

        private ICommand getOlderTweetsCommand;

        private ICommand tweetSelectedCommand;

        public TwitterTabViewModel(MultiPageViewModel<object> parentMultiPage)
            : base(parentMultiPage)
        {
            this.Title = "Twitter";
            this.Icon = "ticon-twitter";
        }

        public ICommand TweetSelectedCommand => this.tweetSelectedCommand ?? (this.tweetSelectedCommand = new ActionCommand<TweetItemViewModel>(this.TweetSelected));

        public ICommand RefreshTweetsCommand => this.refreshTweetsCommand ?? (this.refreshTweetsCommand = new ActionCommand(this.RefreshTweets));

        public ICommand GetOlderTweetsCommand => this.getOlderTweetsCommand ?? (this.getOlderTweetsCommand = new ActionCommand(this.GetOlderTweets));

        public ObservableCollection<TweetItemViewModel> Tweets
        {
            get
            {
                return this.tweets;
            }

            set
            {
                this.Set(ref this.tweets, value);
            }
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();

            this.RefreshTweets();
        }

        private void TweetSelected(TweetItemViewModel tweet)
        {
            var parentViewModel = this.ParentMultiPage as MainViewModel;

            if (parentViewModel != null)
            {
                if ((tweet.GetModel().Entities?.Urls?.Count ?? 0) > 0)
                {
                    parentViewModel.WebLauncher.TryOpenUri(tweet.GetModel().Entities.Urls[0].UrlString);
                }
            }
        }

        private async void RefreshTweets()
        {
            if (this.loadingTweets)
            {
                return;
            }

            this.loadingTweets = true;

            var parentViewModel = this.ParentMultiPage as MainViewModel;

            if (parentViewModel != null)
            {
                this.ParentMultiPage.IsLoading = true;
                var result = await parentViewModel.TwitterDataService.GetNewTweets(0);
                this.ParentMultiPage.IsLoading = false;

                if (result.Successful)
                {
                    var newTweets = result.Value;

                    if (newTweets.Statuses != null && newTweets.Statuses.Count > 0)
                    {
                        this.Tweets =
                            new ObservableCollection<TweetItemViewModel>(
                                newTweets.Statuses.Select(t => new TweetItemViewModel(t)).ToList());
                    }
                }
                else
                {
                    await parentViewModel.ContentDialogService.Alert("Error", result.ErrorMessage);
                }
            }

            this.loadingTweets = false;
        }

        private async void GetOlderTweets()
        {
            if (this.loadingTweets)
            {
                return;
            }

            this.loadingTweets = true;

            var parentViewModel = this.ParentMultiPage as MainViewModel;

            if (parentViewModel != null)
            {
                this.ParentMultiPage.IsLoading = true;
                var result = await parentViewModel.TwitterDataService.GetOlderTweets(this.Tweets?.Select(t => t.GetModel().Id).LastOrDefault() - 1 ?? 0);
                this.ParentMultiPage.IsLoading = false;

                if (result.Successful)
                {
                    var newTweets = result.Value;

                    if (newTweets.Statuses != null && newTweets.Statuses.Count > 0)
                    {
                        foreach (var tweet in newTweets.Statuses)
                        {
                            this.Tweets?.Add(new TweetItemViewModel(tweet));
                        }
                    }
                }
                else
                {
                    await parentViewModel.ContentDialogService.Alert("Error", result.ErrorMessage);
                }
            }

            //this.RaisePropertyChanged(() => this.Tweets);
            this.loadingTweets = false;
        }
    }
}
