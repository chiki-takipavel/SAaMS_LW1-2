using LiveChartsCore;
using LiveChartsCore.Easing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
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

        public void CreateChart(params IEnumerable<float>[] values)
        {
            Series.Clear();
            XAxes.Clear();
            YAxes.Clear();

            var maxCountSeries = values.OrderByDescending(x => x.Count()).First();
            var groupsCount = maxCountSeries.Count();
            var maxLimit = maxCountSeries.Max();

            XAxes.Add(new Axis
            {
                Labeler = value => string.Format("{0:0.0#}", value / groupsCount)
            });

            YAxes.Add(new Axis
            {
                MinLimit = 0,
                MaxLimit = maxLimit
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
