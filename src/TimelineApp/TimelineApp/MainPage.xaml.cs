using AdaptiveCards.Rendering.Uwp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserActivities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace TimelineApp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DocumentManager DocumentManager { get; } = DocumentManager.Instance;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += MainPage_AccountCommandsRequested;
            await DocumentManager.LoadAccountAsync();
            if (DocumentManager.Account != null)
            {
                await DocumentManager.LoadItemsAsync();
            }
            else
            {
                AccountsSettingsPane.Show();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested -= MainPage_AccountCommandsRequested;
        }

        private async void MainPage_AccountCommandsRequested(AccountsSettingsPane sender, AccountsSettingsPaneCommandsRequestedEventArgs args)
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
                await DocumentManager.SetAccountAsync(result.ResponseData[0].WebAccount);
                await DocumentManager.LoadItemsAsync();
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as AppContent;
            Frame.Navigate(typeof(ContentPage), item.Id);
        }

        private async void AddContent_Click(object sender, RoutedEventArgs e)
        {
            var newItem = await DocumentManager.CreateNewAsync(textBoxTitle.Text);
            Frame.Navigate(typeof(ContentPage), newItem.Id);
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            AccountsSettingsPane.Show();
        }

        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            await DocumentManager.ClearAsync();
        }
    }
}
