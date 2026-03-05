using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TestTaskOne.Views.Weighting
{
    public partial class WeightingChartView : UserControl
    {
        public WeightingChartView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}