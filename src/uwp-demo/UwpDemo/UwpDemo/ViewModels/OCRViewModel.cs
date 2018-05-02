using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.UI.Popups;
using Caliburn.Micro;

namespace UwpDemo.ViewModels
{
    public class OCRViewModel : Screen
    {
        private string _output;
        private string _status;

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyOfPropertyChange();
            }
        }

        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                NotifyOfPropertyChange();
            }
        }


        public async Task Run()
        {
            Status = "Running OCR...";

            var engine = OcrEngine.TryCreateFromLanguage(new Language("en-US"));
            if (engine == null)
            {
                var dialog = new MessageDialog("Error creating the OCR engine");
                await dialog.ShowAsync();
                return;
            }

            var stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("UwpDemo.Images.ocr-embedded.png");
            var decoder = await BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
            var bmp = await decoder.GetSoftwareBitmapAsync();
            var result = await engine.RecognizeAsync(bmp);

            Output = string.Join("\r\n", result.Lines.Select(l => l.Text));

            Status = $"Done.\t{result.Lines.Count} Lines of text recognized.";
        }
    }
}
