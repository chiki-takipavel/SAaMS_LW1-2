using LiveChartsCore;
using LiveChartsCore.Easing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SAaMS_LW1.ViewModels
{
    public class ChartViewModel
    {
        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<Axis> XAxes { get; set; }
        public ObservableCollection<Axis> YAxes { get; set; }

        public ChartViewModel()
        {
            Series = new ObservableCollection<ISeries>();

            XAxes = new ObservableCollection<Axis> { new Axis() };
            YAxes = new ObservableCollection<Axis> { new Axis() };
        }

        public void CreateChart((double minValue, double maxValue) xValuesRange, params IEnumerable<float>[] values)
        {
            Series.Clear();
            XAxes.Clear();
            YAxes.Clear();

            var maxCountSeries = values.OrderByDescending(x => x.Count()).First();
            var groupsCount = maxCountSeries.Count();

            var minXLimit = xValuesRange.minValue >= 0 ? Math.Floor(xValuesRange.minValue) : Math.Ceiling(xValuesRange.minValue);
            var maxXLimit = xValuesRange.maxValue >= 0 ? Math.Ceiling(xValuesRange.maxValue) : Math.Floor(xValuesRange.maxValue);
            var intervalSize = (maxXLimit - minXLimit) / groupsCount;

            var maxYLimit = maxCountSeries.Max();

            XAxes.Add(new Axis
            {
                Labeler = value => $"{minXLimit + (intervalSize * value):0.0#}"
            });

            YAxes.Add(new Axis
            {
                MinLimit = 0,
                MaxLimit = maxYLimit
            });

            foreach (var value in values)
            {
                var columnSeries = new ColumnSeries<float>
                {
                    GroupPadding = 2,
                    Name = string.Empty,
                    Stroke = null,
                    Values = value
                };

                columnSeries.PointMeasured += OnPointMeasured;

                Series.Add(columnSeries);
            }
        }

        private void OnPointMeasured(TypedChartPoint<float, RoundedRectangleGeometry, LabelGeometry, SkiaSharpDrawingContext> point)
        {
            var visual = point.Visual;
            var delayedFunction = new DelayedFunction(EasingFunctions.BuildCustomElasticOut(1.5f, 0.60f), point, 30f);

            _ = visual
                .TransitionateProperties(
                    nameof(visual.Y),
                    nameof(visual.Height))
                .WithAnimation(animation =>
                    animation
                        .WithDuration(delayedFunction.Speed)
                        .WithEasingFunction(delayedFunction.Function));
        }
    }
}
