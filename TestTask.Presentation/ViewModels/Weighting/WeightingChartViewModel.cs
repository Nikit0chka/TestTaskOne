using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TestTaskOne.Models;

namespace TestTaskOne.ViewModels.Weighting
{
    public class WeightingChartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GrafModels> _chartData;

        public WeightingChartViewModel()
        {
            XAxes = new Axis[]
            {
                new Axis
                {
                   Name = "Время",
                    Labeler = value =>
                    {
                        long ticks = (long)value;
                        if (ticks < DateTime.MinValue.Ticks) ticks = DateTime.MinValue.Ticks;
                        if (ticks > DateTime.MaxValue.Ticks) ticks = DateTime.MaxValue.Ticks;
                        return new DateTime(ticks).ToString("HH:mm:ss.fff");
                    },
                    UnitWidth = TimeSpan.FromSeconds(1).Ticks,
                    MinStep = TimeSpan.FromMilliseconds(100).Ticks
                }
            };

            YAxes = new Axis[]
            {
                new Axis { Name = "Вес / Количество" }
            };

            // Начальное состояние — пустой массив
            Series = Array.Empty<ISeries>();
        }

        public ObservableCollection<GrafModels> ChartData
        {
            get => _chartData;
            set
            {
                if (_chartData != value)
                {
                    _chartData = value;
                    UpdateSeries();
                    OnPropertyChanged();
                }
            }
        }

        public ISeries[] Series { get; private set; }
        public Axis[] XAxes { get; }
        public Axis[] YAxes { get; }

        private void UpdateSeries()
        {
            if (_chartData != null && _chartData.Any())
            {
                Series = new ISeries[]
                {
                    new LineSeries<GrafModels>
                    {
                        Name = "Суммарный вес",
                        Values = _chartData,
                        Mapping = (model, _) => new Coordinate(model.Date.Ticks, (float)model.TotalWeight),
                        Fill = null,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                        GeometryStroke = null,
                        GeometrySize = 3
                    },
                    new LineSeries<GrafModels>
                    {
                        Name = "Количество взвешиваний",
                        Values = _chartData,
                        Mapping = (model, _) => new Coordinate(model.Date.Ticks, model.Count),
                        Fill = null,
                        Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 },
                        GeometryStroke = null,
                        GeometrySize = 3
                    }
                };
            }
            else
            {
                Series = Array.Empty<ISeries>();
            }

            OnPropertyChanged(nameof(Series));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}