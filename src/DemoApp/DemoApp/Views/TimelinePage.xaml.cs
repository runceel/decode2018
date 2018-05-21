using System;
using DemoApp.Models;
using DemoApp.Services;
using DemoApp.ViewModels;
using Windows.Security.Authentication.Web.Core;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DemoApp.Views
{
    public sealed partial class TimelinePage : Page
    {
        public TimelineViewModel ViewModel { get; } = new TimelineViewModel();

        public TimelinePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += TimelinePage_AccountCommandsRequested;
            await ViewModel.DocumentManager.LoadAccountAsync();
            if (ViewModel.DocumentManager.Account != null)
            {
                await ViewModel.DocumentManager.LoadItemsAsync();
            }
            else
            {
                AccountsSettingsPane.Show();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested -= TimelinePage_AccountCommandsRequested;
        }

        private async void TimelinePage_AccountCommandsRequested(AccountsSettingsPane sender, AccountsSettingsPaneCommandsRequestedEventArgs args)
        {
            var d = args.GetDeferral();
            var msaProvider = await WebAuthenticationCoreManager.FindAccountProviderAsync(
                "https://login.microsoft.com",
                "consumers");
            var command = new WebAccountProviderCommand(msaProvider, GetMsaTokenAsync);
            args.WebAccountProviderCommands.Add(command);
            d.Complete();
        }

        private async void GetMsaTokenAsync(WebAccountProviderCommand command)
        {
            var request = new WebTokenRequest(command.WebAccountProvider, "wl.basic");
            var result = await WebAuthenticationCoreManager.RequestTokenAsync(request);
            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                await ViewModel.DocumentManager.SetAccountAsync(result.ResponseData[0].WebAccount);
                await ViewModel.DocumentManager.LoadItemsAsync();
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as AppContent;
            NavigationService.Navigate(typeof(ContentPage), item.Id);
        }
    }
}
