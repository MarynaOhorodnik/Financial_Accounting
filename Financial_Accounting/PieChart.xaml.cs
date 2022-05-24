using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Financial_Accounting
{
    /// <summary>
    /// Interaction logic for PieChart.xaml
    /// </summary>
    public partial class PieChart : UserControl
    {
        public PieChart()
        {
            InitializeComponent();

            DataContext = this;

            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Надходження",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Value_Total.Income_total)},
                    DataLabels = false,
                    Fill = Brushes.GreenYellow
                },
                new PieSeries
                {
                    Title = "Витрати",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Value_Total.Outcome_total)},
                    DataLabels = false,
                    Fill = Brushes.MediumPurple
                }
            };
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}
