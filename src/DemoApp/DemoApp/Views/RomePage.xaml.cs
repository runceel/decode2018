using System;

using DemoApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DemoApp.Views
{
    public sealed partial class RomePage : Page
    {
        public RomeViewModel ViewModel { get; } = new RomeViewModel();

        public RomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.RomeManager.Text = e.Parameter as string;
        }
    }
}
