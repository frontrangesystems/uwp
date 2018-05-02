using System;

using UwpDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UwpDemo.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
