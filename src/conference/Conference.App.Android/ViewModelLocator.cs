namespace Conference.App.Android
{
    using Conference.Common.Contracts.Data;
    using Conference.Common.Data;
    using Conference.Common.ViewModel;

    using OpenMVVM.Android.PlatformServices;
    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Web;
    using OpenMVVM.WebView;
    using OpenMVVM.WebView.Android;

    public class ViewModelLocator : ViewModelLocatorBase
    {
        public ViewModelLocator()
        {
            var ioc = IocInstanceFactory.DefaultWeb;

            // Infrastructure
            ioc.RegisterInstance<IHttpClient>(new AdvancedHttpClient() { UseGZip = false });
            ioc.RegisterType<IBridge, AndroidBridge>();

            // Services
            ioc.RegisterType<IFileServices, FileServices>();
            ioc.RegisterType<ILifecycleService, LifecycleService>();
            ioc.RegisterType<INavigationService, NavigationService>();
            ioc.RegisterType<ICacheService, CacheService>();
            ioc.RegisterType<IConferenceDataService, ConferenceDataService>();
            ioc.RegisterType<IConferenceRepository, ConferenceRepository>();
            ioc.RegisterType<ITwitterDataService, TwitterDataService>();
            ioc.RegisterType<IEventTracker, NullEventTracker>();
            ioc.RegisterType<IWebLauncher, WebLauncher>();
            ioc.RegisterType<IDispatcherService, DispatcherService>();
            ioc.RegisterInstance<IContentDialogService>(new ContentDialogService(ioc.GetInstance<IDispatcherService>()));
            ioc.RegisterType<IReminderService, ReminderService>();

            // ViewModels
            ioc.RegisterType<MainViewModel>();
            ioc.RegisterType<SessionViewModel>();
            ioc.RegisterType<SpeakerViewModel>();
            ioc.RegisterType<FavoritesViewModel>();
        }

        public MainViewModel MainViewModel => InstanceFactory.GetInstance<MainViewModel>();

        public SessionViewModel SessionViewModel => InstanceFactory.GetInstance<SessionViewModel>();

        public SpeakerViewModel SpeakerViewModel => InstanceFactory.GetInstance<SpeakerViewModel>();

        public FavoritesViewModel FavoritesViewModel => InstanceFactory.GetInstance<FavoritesViewModel>();
    }
}
