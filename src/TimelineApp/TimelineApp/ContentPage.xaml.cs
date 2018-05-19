using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace TimelineApp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class ContentPage : Page, INotifyPropertyChanged
    {
        private DocumentManager DocumentManager { get; } = DocumentManager.Instance;

        private AppContent _appContent;

        public event PropertyChangedEventHandler PropertyChanged;

        public AppContent AppContent
        {
            get { return _appContent; }
            set
            {
                _appContent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppContent)));
            }
        }

        public ContentPage()
        {
            this.InitializeComponent();
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
                        Frame.Navigate(typeof(MainPage));
                    }
                });
            }

            base.OnNavigatedTo(e);

            if (DocumentManager.Account == null)
            {
                await DocumentManager.LoadAccountAsync();
                if (DocumentManager.Account != null)
                {
                    await DocumentManager.LoadItemsAsync();
                }
                
                if (DocumentManager.Account == null || DocumentManager.Items == null)
                {
                    backToMainPage();
                    return;
                }
            }

            var id = e.Parameter as string;
            AppContent = await DocumentManager.GetItemByIdAsync(id);
            if (AppContent == null)
            {
                backToMainPage();
                return;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await DocumentManager.AddOrUpdateDocumentAsync(AppContent);
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
