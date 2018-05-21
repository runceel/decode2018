using System;

using DemoApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DemoApp.Views
{
    public sealed partial class DataGridPage : Page
    {
        public DataGridViewModel ViewModel { get; } = new DataGridViewModel();

        public DataGridPage()
        {
            InitializeComponent();
        }
    }
}
