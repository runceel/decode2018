using System;
using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ConfirmCloseApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += MainPage_CloseRequested;
        }

        private async void MainPage_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            e.Handled = true;
            var d = new MessageDialog("閉じてもよろしいですか？");
            var okCommand = new UICommand("OK");
            d.Commands.Add(okCommand);
            var cancelCommand = new UICommand("Cancel");
            d.Commands.Add(cancelCommand);
            var r = await d.ShowAsync();
            if (r == okCommand)
            {
                Application.Current.Exit();
            }
        }
    }
}
