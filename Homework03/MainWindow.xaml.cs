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
using System.Diagnostics;

namespace Homework02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public bool reduction, optionvalue;
        Stopwatch watch = new Stopwatch();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnstart_Click(object sender, RoutedEventArgs e)
        {
            watch.Start();
            lbltimer.Content = "It is counting now......";
        }

        private void btnstop_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            lbltimer.Content = watch.Elapsed.Hours.ToString()
                + ":"
                + watch.Elapsed.Minutes.ToString()
                + ":"
                + watch.Elapsed.Seconds.ToString()
                + ":"
                + watch.Elapsed.Milliseconds.ToString();
        }

        private void btnreset_Click(object sender, RoutedEventArgs e)
        {
            watch.Reset();
            lbltimer.Content = "00:00:00:00";
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            reduction = true;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            optionvalue = true;

        }
        private bool IsNumberic(string oText)
        {
            try
            {
                int var1 = Convert.ToInt32(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void spotprice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumberic(spotprice.Text) == false)
            {
                MessageBox.Show("please enter the true value!");
            }

        }
        private void strikeprice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumberic(strikeprice.Text) == false)
            {
                MessageBox.Show("please enter the true value!");
            }

        }

        private void timetomaturity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumberic(strikeprice.Text) == false)
            {
                MessageBox.Show("please enter the true value!");
            }
        }

        private void drift_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumberic(strikeprice.Text) == false)
            {
                MessageBox.Show("please enter the true value!");
            }
        }

        private void volatility_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumberic(strikeprice.Text) == false)
            {
                MessageBox.Show("please enter the true value!");
            }
        }

        private void steps_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumberic(strikeprice.Text) == false)
            {
                MessageBox.Show("please enter the true value!");
            }
        }

        private void numberofsimulations_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumberic(strikeprice.Text) == false)
            {
                MessageBox.Show("please enter the true value!");
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            watch.Reset();
            lbltimer.Content = "00:00:00:00";
            watch.Start();
            double[] price = new double[4];
            double S = Convert.ToDouble(spotprice.Text);
            double K = Convert.ToDouble(strikeprice.Text);
            double T = Convert.ToDouble(timetomaturity.Text);
            double r = Convert.ToDouble(drift.Text);
            double sigma = Convert.ToDouble(volatility.Text);
            Int32 N = Convert.ToInt32(steps.Text);
            Int32 I = Convert.ToInt32(numberofsimulations.Text);

            Simulator simulator = new Simulator(N,I);

            double[] result = simulator.SimulatedPrice(S, K, T, r, sigma, N, I);
            double[] result2 = simulator.Greeks(S, K, T, r, sigma, N, I);
            if (reduction == true)
            {
                if (optionvalue == true)
                {
                    string s = "European Call Option (using delta-based control variate)" + "\r\n"
                    + "the price: " + result[0] + "\r\n"
                    + "Antithetic Standard Error:" + result[4] + "\r\n"
                    + "the Delta:" + result2[0] + "\r\n" +
                    "the Gamma:" + result2[2] + "\r\n" +
                    "the Vega:" + result2[4] + "\r\n"
                    + "the Theta+:" + result2[6] + "\r\n"
                    + "the Rho:" + result2[8]
                    ;
                    ResultShow.Content = s;
                    reduction = false;
                    optionvalue = false;
                    watch.Stop();
                    lbltimer.Content = watch.Elapsed.Hours.ToString()
                        + ":"
                        + watch.Elapsed.Minutes.ToString()
                        + ":"
                        + watch.Elapsed.Seconds.ToString()
                        + ":"
                        + watch.Elapsed.Milliseconds.ToString();
                }
                else
                {
                    string s = "European Put Option (using delta-based control variate)" + "\r\n"
                        + "the price:" + result[1] + "\r\n"
                        + "the Antithetic Standard Error:" + result[5] + "\r\n"
                        + "the delta:" + result2[1] + "\r\n"
                        + "the gamma:" + result2[3] + "\r\n"
                        + "the Vega:" + result2[5] + "\r\n"
                        + "the Theta:" + result2[7] + "\r\n"
                        + "the Rho:" + result2[9];
                    ResultShow.Content = s;
                    reduction = false;
                    optionvalue = false;
                    watch.Stop();
                    lbltimer.Content = watch.Elapsed.Hours.ToString()
                        + ":"
                        + watch.Elapsed.Minutes.ToString()
                        + ":"
                        + watch.Elapsed.Seconds.ToString()
                        + ":"
                        + watch.Elapsed.Milliseconds.ToString();
                }

            }
            else
            {
                if (optionvalue == true)
                {
                    string s = "European Call Option (using delta-based control variate)" + "\r\n"
                       + "the price:" + result[0] + "\r\n"
                       + "the Standard Error:" + result[2] + "\r\n"
                       + "the delta:" + result2[0] + "\r\n"
                       + "the gamma:" + result2[2] + "\r\n"
                       + "the Vega:" + result2[4] + "\r\n"
                       + "the Theta:" + result2[6] + "\r\n"
                       + "the Rho:" + result2[8];
                    ;
                    ResultShow.Content = s;
                    reduction = false;
                    optionvalue = false;
                    watch.Stop();
                    lbltimer.Content = watch.Elapsed.Hours.ToString()
                        + ":"
                        + watch.Elapsed.Minutes.ToString()
                        + ":"
                        + watch.Elapsed.Seconds.ToString()
                        + ":"
                        + watch.Elapsed.Milliseconds.ToString();
                }
                else
                {
                    string s = "European Put Option (using delta-based control variate)" + "\r\n"
                       + "the price:" + result[1] + "\r\n"
                       + "the Standard Error:" + result[3] + "\r\n"
                       + "the delta:" + result2[1] + "\r\n"
                       + "the gamma:" + result2[3] + "\r\n"
                       + "the Vega:" + result2[5] + "\r\n"
                       + "the Theta:" + result2[7] + "\r\n"
                       + "the Rho:" + result2[9];
                    ResultShow.Content = s;
                    reduction = false;
                    optionvalue = false;
                    watch.Stop();
                    lbltimer.Content = watch.Elapsed.Hours.ToString()
                        + ":"
                        + watch.Elapsed.Minutes.ToString()
                        + ":"
                        + watch.Elapsed.Seconds.ToString()
                        + ":"
                        + watch.Elapsed.Milliseconds.ToString();
                }

            }

        }


    }



}
