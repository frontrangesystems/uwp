using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml;
using UwpDemo.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace UwpDemo.Views
{
    public sealed partial class InkPage : Page
    {
        public InkPage()
        {
            InitializeComponent();

            InkCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Pen;

            var drawingAttributes = new InkDrawingAttributes
            {
                Color = Windows.UI.Colors.Black,
                IgnorePressure = false,
                FitToCurve = true
            };
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private InkViewModel ViewModel
        {
            get { return DataContext as InkViewModel; }
        }


        private async void Recognize_OnClick(object sender, RoutedEventArgs e)
        {
            var analyzer = new InkAnalyzer();
            var strokes = InkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            if (strokes.Count <= 0)
            {
                return;
            }

            analyzer.AddDataForStrokes(strokes);

            var results = await analyzer.AnalyzeAsync();
            if (results.Status != InkAnalysisStatus.Updated)
            {
                return;
            }

            // find recognized strokes
            var textNodes = analyzer.AnalysisRoot.FindNodes(InkAnalysisNodeKind.InkWord);

            // draw all text
            foreach (InkAnalysisInkWord node in textNodes)
            {
                DrawText(node.RecognizedText, node.BoundingRect);

                foreach (var strokeId in node.GetStrokeIds())
                {
                    var stroke = InkCanvas.InkPresenter.StrokeContainer.GetStrokeById(strokeId);
                    if (UseSystemFocusVisuals)
                    {
                        stroke.Selected = true;
                    }
                }
                analyzer.RemoveDataForStrokes(node.GetStrokeIds());
            }
            InkCanvas.InkPresenter.StrokeContainer.DeleteSelected();

            var shapeNodes = analyzer.AnalysisRoot.FindNodes(InkAnalysisNodeKind.InkDrawing);

            // draw shapes
            foreach (InkAnalysisInkDrawing node in shapeNodes)
            {
                if (node.DrawingKind == InkAnalysisDrawingKind.Drawing)
                {
                    // unsupported shape
                    continue;
                }

                Draw(node);

                foreach (var strokeId in node.GetStrokeIds())
                {
                    var stroke = InkCanvas.InkPresenter.StrokeContainer.GetStrokeById(strokeId);
                    stroke.Selected = true;
                }

                analyzer.RemoveDataForStrokes(node.GetStrokeIds());
            }
            InkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
        }

        private void Draw(InkAnalysisInkDrawing node)
        {
            if (node.DrawingKind == InkAnalysisDrawingKind.Circle || node.DrawingKind==InkAnalysisDrawingKind.Ellipse)
            {
                DrawEllipse(node);
            }
            else
            {
                DrawPolygon(node);
            }
        }

        private void DrawText(string recognizedText, Rect boundingRect)
        {
            var text = new TextBlock();
            Canvas.SetTop(text, boundingRect.Top);
            Canvas.SetLeft(text, boundingRect.Left);

            text.Text = recognizedText;
            text.FontSize = boundingRect.Height;

            RecognitionCanvas.Children.Add(text);
        }

        private void DrawEllipse(InkAnalysisInkDrawing shape)
        {
            var ellipse = new Ellipse
            {
                Width = shape.BoundingRect.Width,
                Height = shape.BoundingRect.Height
            };
            
            Canvas.SetTop(ellipse, shape.BoundingRect.Top);
            Canvas.SetLeft(ellipse, shape.BoundingRect.Left);

            var brush = new SolidColorBrush(Windows.UI.ColorHelper.FromArgb(255, 0, 0, 255));
            ellipse.Stroke = brush;
            ellipse.StrokeThickness = 2;
            RecognitionCanvas.Children.Add(ellipse);
        }

        private void DrawPolygon(InkAnalysisInkDrawing shape)
        {
            var points = new List<Point>(shape.Points);
            var polygon = new Polygon();

            foreach (var point in points)
            {
                polygon.Points.Add(point);
            }

            var brush = new SolidColorBrush(Windows.UI.ColorHelper.FromArgb(255, 0, 0, 255));
            polygon.Stroke = brush;
            polygon.StrokeThickness = 2;
            RecognitionCanvas.Children.Add(polygon);
        }

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            InkCanvas.InkPresenter.StrokeContainer.Clear();
            RecognitionCanvas.Children.Clear();
        }
    }
}
