using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace InkingDemo
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly InkAnalyzer _inkAnalyzer = new InkAnalyzer();
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var camera = new CameraCaptureUI();
            var file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            using (var s = await file.OpenReadAsync())
            {
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(s);
                picture.Source = bitmap;
            }
        }

        private async void RecognizeButton_Click(object sender, RoutedEventArgs e)
        {
            var inkStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            // Ensure an ink stroke is present.
            if (inkStrokes.Count > 0)
            {
                _inkAnalyzer.AddDataForStrokes(inkStrokes);
                var inkAnalysisResults = await _inkAnalyzer.AnalyzeAsync();

                // Have ink strokes on the canvas changed?
                if (inkAnalysisResults.Status == InkAnalysisStatus.Updated)
                {
                    var inkdrawingNodes =
                        _inkAnalyzer.AnalysisRoot.FindNodes(
                            InkAnalysisNodeKind.InkDrawing);
                    foreach (InkAnalysisInkDrawing node in inkdrawingNodes)
                    {
                        if (node.DrawingKind != InkAnalysisDrawingKind.Drawing)
                        {
                            // Draw an Ellipse object on the recognitionCanvas (circle is a specialized ellipse).
                            if (node.DrawingKind == InkAnalysisDrawingKind.Circle || node.DrawingKind == InkAnalysisDrawingKind.Ellipse)
                            {
                                DrawEllipse(node);
                            }
                            // Draw a Polygon object on the recognitionCanvas.
                            else
                            {
                                DrawPolygon(node);
                            }
                            foreach (var strokeId in node.GetStrokeIds())
                            {
                                var stroke = inkCanvas.InkPresenter.StrokeContainer.GetStrokeById(strokeId);
                                stroke.Selected = true;
                            }
                        }
                        _inkAnalyzer.RemoveDataForStrokes(node.GetStrokeIds());
                    }
                    inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
                }
            }
        }

        // Draw an ellipse on the recognitionCanvas.
        private void DrawEllipse(InkAnalysisInkDrawing shape)
        {
            var points = shape.Points;
            var ellipse = new Ellipse();

            ellipse.Width = shape.BoundingRect.Width;
            ellipse.Height = shape.BoundingRect.Height;

            Canvas.SetTop(ellipse, shape.BoundingRect.Top);
            Canvas.SetLeft(ellipse, shape.BoundingRect.Left);

            var stroke = inkCanvas.InkPresenter.StrokeContainer.GetStrokeById(shape.GetStrokeIds().First());
            ellipse.Stroke = new SolidColorBrush(stroke.DrawingAttributes.Color);
            ellipse.StrokeThickness = 2;
            recognitionCanvas.Children.Add(ellipse);
        }

        // Draw a polygon on the recognitionCanvas.
        private void DrawPolygon(InkAnalysisInkDrawing shape)
        {
            var points = new List<Point>(shape.Points);
            var polygon = new Polygon();

            foreach (var point in points)
            {
                polygon.Points.Add(point);
            }

            var stroke = inkCanvas.InkPresenter.StrokeContainer.GetStrokeById(shape.GetStrokeIds().First());
            polygon.Stroke = new SolidColorBrush(stroke.DrawingAttributes.Color);
            polygon.StrokeThickness = 2;
            recognitionCanvas.Children.Add(polygon);
        }
    }
}
