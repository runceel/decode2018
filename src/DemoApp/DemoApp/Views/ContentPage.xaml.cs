using System;
using DemoApp.Services;
using DemoApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DemoApp.Views
{
    public sealed partial class ContentPage : Page
    {
        public ContentViewModel ViewModel { get; } = new ContentViewModel();

        public ContentPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            void backToMainPage()
            {
                var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {
                    if (Frame.CanGoBack)
                    {
                        Frame.GoBack();
                    }
                    else
                    {
                        NavigationService.Navigate(typeof(MainPage));
                    }
                });
            }

            base.OnNavigatedTo(e);

            if (ViewModel.DocumentManager.Account == null)
            {
                await ViewModel.DocumentManager.LoadAccountAsync();
                if (ViewModel.DocumentManager.Account != null)
                {
                    await ViewModel.DocumentManager.LoadItemsAsync();
                }

                if (ViewModel.DocumentManager.Account == null || ViewModel.DocumentManager.Items == null)
                {
                    backToMainPage();
                    return;
                }
            }

            var id = e.Parameter as string;
            ViewModel.AppContent = await ViewModel.DocumentManager.GetItemByIdAsync(id);
            if (ViewModel.AppContent == null)
            {
                backToMainPage();
                return;
            }
        }

    }
}
