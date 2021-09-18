using MaterialDesignThemes.Wpf;
using SAaMS_LW1.Helpers;
using SAaMS_LW1.Helpers.Enums;
using SAaMS_LW1.Helpers.Exceptions;
using SAaMS_LW1.Randoms;
using SAaMS_LW1.Sequences;
using SAaMS_LW1.ViewModels;
using System;
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
        private Distribution selectedDistribution;
        private readonly DistributionHelper distributionHelper = new();

        public MainWindow()
        {
            InitializeComponent();
            sbError.Message.ActionClick += (_, _) => sbError.IsActive = false;
        }

        private async Task Calculates()
        {
            int a = int.Parse(tbParamA.Text, CultureInfo.CurrentCulture);
            int r0 = int.Parse(tbParamR0.Text, CultureInfo.CurrentCulture);
            int m = int.Parse(tbParamM.Text, CultureInfo.CurrentCulture);

            btnGenerate.IsEnabled = false;

            LehmerRandom random = new(a, r0, m);
            LehmerSequence sequenceLehmer = new(random);
            distributionHelper.StartSequence = sequenceLehmer;
            IEnumerable<double> sequenceValue = await distributionHelper.GetDistribution(selectedDistribution);

            btnGenerate.IsEnabled = true;

            StatisticsGeneration statistics = new(sequenceValue);
            tblExpectedValue.Text = statistics.GetExpectedValue().ToString(CultureInfo.CurrentCulture);
            tblDispersion.Text = statistics.GetDispersion().ToString(CultureInfo.CurrentCulture);
            tblStandartDeviation.Text = statistics.GetStandartDeviation().ToString(CultureInfo.CurrentCulture);
            tblPeriod.Text = statistics.GetPeriod().ToString(CultureInfo.CurrentCulture);
            tblCheck.Text = statistics.GetChecked().ToString(CultureInfo.CurrentCulture);

            ChartViewModel chart = (ChartViewModel)DataContext;
            chart.CreateChart(statistics.GetHistogramDistribution());

            await Task.Delay(1500);
            statistics.ShowValues();
        }

        private void IntegerNumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new(@"^[0-9]$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void FloatNumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new(@"^[0-9\,]$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private async void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sbError.IsActive = false;

                selectedDistribution = (Distribution)cmbDistribution.SelectedIndex;
                if (selectedDistribution is not Distribution.None)
                {
                    dlgParameters.IsOpen = true;
                }
                else
                {
                    await Calculates();
                }
            }
            catch (Exception ex)
            {
                sbError.Message.Content = ex.Message;
                sbError.IsActive = true;
            }
        }

        private void DistributionParameters_DialogOpened(object sender, DialogOpenedEventArgs eventArgs)
        {
            tbParam1.Text = string.Empty;
            tbParam2.Text = string.Empty;
            tbParam2.Visibility = Visibility.Visible;

            switch (selectedDistribution)
            {
                case Distribution.None:
                    break;
                case Distribution.Exponential:
                    HintAssist.SetHint(tbParam1, "Введите коэффициент λ");
                    tbParam2.Visibility = Visibility.Collapsed;
                    break;
                case Distribution.Gauss:
                    HintAssist.SetHint(tbParam1, "Введите коэффициент m");
                    HintAssist.SetHint(tbParam2, "Введите коэффициент σ");
                    break;
                case Distribution.Gamma:
                    HintAssist.SetHint(tbParam1, "Введите коэффициент η");
                    HintAssist.SetHint(tbParam2, "Введите коэффициент λ");
                    break;
                case Distribution.Simpson:
                case Distribution.Triangular:
                case Distribution.Uniform:
                    HintAssist.SetHint(tbParam1, "Введите коэффициент a");
                    HintAssist.SetHint(tbParam2, "Введите коэффициент b");
                    break;
                default:
                    break;
            }
        }

        private async void DistributionParameters_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            try
            {
                distributionHelper.Parameter1Value = double.TryParse(tbParam1.Text, out double param1) ? param1
                    : throw new EmptyValueException("Parameter(s) cannot be empty.");

                if (tbParam2.IsVisible)
                {
                    distributionHelper.Parameter2Value = double.TryParse(tbParam2.Text, out double param2) ? param2
                        : throw new EmptyValueException("Parameter(s) cannot be empty.");
                }

                await Calculates();
            }
            catch (Exception ex)
            {
                sbError.Message.Content = ex.Message;
                sbError.IsActive = true;
            }
        }
    }
}
