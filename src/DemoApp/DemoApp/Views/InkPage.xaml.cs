using System;
using System.Collections.Generic;
using System.Linq;
using DemoApp.ViewModels;
using Windows.Foundation;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DemoApp.Views
{
    public sealed partial class InkPage : Page
    {
        private readonly InkAnalyzer _inkAnalyzer = new InkAnalyzer();
        public InkViewModel ViewModel { get; } = new InkViewModel();

        public InkPage()
        {
            InitializeComponent();
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
            ellipse.StrokeThickness = stroke.DrawingAttributes.Size.Width;
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
            polygon.StrokeThickness = stroke.DrawingAttributes.Size.Width;
            recognitionCanvas.Children.Add(polygon);
        }
    }
}
