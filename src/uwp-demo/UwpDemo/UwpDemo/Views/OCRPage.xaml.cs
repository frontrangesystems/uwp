using System;

using UwpDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UwpDemo.Views
{
    public sealed partial class OCRPage : Page
    {
        public OCRPage()
        {
            InitializeComponent();
        }

        private OCRViewModel ViewModel
        {
            get { return DataContext as OCRViewModel; }
        }
    }
}
