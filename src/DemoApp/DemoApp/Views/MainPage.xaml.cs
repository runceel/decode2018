using System;
using System.Diagnostics;
using DemoApp.ViewModels;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

namespace DemoApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
            Debug.WriteLine(Package.Current.Id.FamilyName);
        }
    }
}
