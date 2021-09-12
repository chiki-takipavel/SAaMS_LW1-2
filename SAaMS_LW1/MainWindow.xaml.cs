using SAaMS_LW1.Helpers;
using SAaMS_LW1.Sequences;
using SAaMS_LW1.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SAaMS_LW1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            int a = int.Parse(tbParamA.Text, CultureInfo.CurrentCulture);
            int r0 = int.Parse(tbParamR0.Text, CultureInfo.CurrentCulture);
            int m = int.Parse(tbParamM.Text, CultureInfo.CurrentCulture);

            btnGenerate.IsEnabled = false;
            LehmerSequence sequence = new(a, r0, m);
            IEnumerable<double> sequenceValue = await Task.Run(() => sequence.ProvideSequence());
            btnGenerate.IsEnabled = true;

            StatisticsGeneration statistics = new(sequenceValue);
            tbExpectedValue.Text = statistics.GetExpectedValue().ToString(CultureInfo.CurrentCulture);
            tbDispersion.Text = statistics.GetDispersion().ToString(CultureInfo.CurrentCulture);
            tbStandartDeviation.Text = statistics.GetStandartDeviation().ToString(CultureInfo.CurrentCulture);
            tbPeriod.Text = statistics.GetPeriod().ToString(CultureInfo.CurrentCulture);
            tbCheck.Text = statistics.GetChecked().ToString(CultureInfo.CurrentCulture);

            ChartViewModel chart = (ChartViewModel)DataContext;
            chart.CreateChart(statistics.GetDistribution());

            await Task.Delay(1500);
            statistics.ShowValues();
        }

        private void FloatNumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new(@"^[0-9]$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
