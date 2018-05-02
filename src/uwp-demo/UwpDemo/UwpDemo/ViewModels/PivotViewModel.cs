using Caliburn.Micro;

using UwpDemo.Helpers;

namespace UwpDemo.ViewModels
{
    public class PivotViewModel : Conductor<Screen>.Collection.OneActive
    {
        protected override void OnInitialize()
        {
            Items.Add(new MainViewModel { DisplayName = "PivotItem_Main/Header".GetLocalized() });
            Items.Add(new OCRViewModel { DisplayName = "PivotItem_OCR/Header".GetLocalized() });
            Items.Add(new SettingsViewModel { DisplayName = "PivotItem_Settings/Header".GetLocalized() });
        }
    }
}
