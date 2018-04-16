namespace Conference.App.Windows
{
    using Common.Contracts.Data;
    using Common.Data;
    using Common.ViewModel;
    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Web;
    using OpenMVVM.WebView;
    using OpenMVVM.WebView.Windows;
    using OpenMVVM.Windows.PlatformServices;

    public class ViewModelLocator : ViewModelLocatorBase
    {
        public ViewModelLocator()
        {
            var ioc = IocInstanceFactory.DefaultWeb;

            // Infrastructure
            ioc.RegisterInstance<IHttpClient>(new AdvancedHttpClient() { UseGZip = false });
            //ioc.RegisterType<IBridge, WindowsBridge>();

            // Services
            ioc.RegisterType<ILifecycleService, LifecycleService>();
            ioc.RegisterType<INavigationService, NavigationService>();
            ioc.RegisterType<ICacheService, CacheService>();
            ioc.RegisterType<IConferenceDataService, ConferenceDataService>();
            ioc.RegisterType<IConferenceRepository, ConferenceRepository>();
            ioc.RegisterType<ITwitterDataService, TwitterDataService>();
            ioc.RegisterType<IEventTracker, NullEventTracker>();
            ioc.RegisterType<IWebLauncher, WebLauncher>();
            ioc.RegisterType<IContentDialogService, ContentDialogService>();
            ioc.RegisterType<IDispatcherService, DispatcherService>();
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
