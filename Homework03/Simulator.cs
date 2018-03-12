using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Homework02
{

    public class Simulator
    {
        public double[,] randomNumbers;
        public Simulator(Int32 N, Int32 I)
        {
            GaussianRNG g = new GaussianRNG();
            this.randomNumbers = g.RandomMatrix(N, I);
        }
        public double[] SimulatedPrice(double S, double K, double T, double r, double sigma, Int32 N, Int32 I)
        {
            //S:spotprice; K:strikeprice; T: the tenor; r:the drift=the risk-free rate; sigma:the volatility; 
            //N:the steps of the simulation
            //I: the number of the simulations
            double[] simulatedprice = new double[8];//output 8 different values.
            double dt = T / N;
            //t is the time steps.
            //T is the time to maturity, the tenor.
            double nudt = (r - 0.5 * Math.Pow(sigma, 2)) * dt;
            double sigsdt = sigma * Math.Sqrt(dt);
            double beta1 = -1;
            double erddt = Math.Exp(r * dt);
            double[,] Vu = new double[I, N];
            //establish the upper price matrix.
            double[,] Vd = new double[I, N];
            //establish the down price matrix.
            double[] Call = new double[I];
            double[] AntiCall = new double[I];
            //this is the call matrix.
            double[] Put = new double[I];
            double[] AntiPut = new double[I];
            //this is the put matrix.
            double[] delta = new double[N];
            double[,] CT = new double[1, 2]; CT[0, 0] = 0; CT[0, 1] = 0;
            double[,] PT = new double[1, 2]; PT[0, 0] = 0; PT[0, 1] = 0;
            double sumCT = 0, sumAntiCT = 0, sumPT = 0, sumAntiPT = 0;
            double sumCT2 = 0, sumAntiCT2 = 0, sumPT2 = 0, sumAntiPT2 = 0;
            double sumzc = 0, sumzp = 0, sumzc2 = 0, sumzp2 = 0;
            for(uint i=0;i<I;i++)
            {
                Vu[i, 0] = S;
                Vd[i, 0] = S;
            }
            for (uint j = 0; j < N; j++)
            {
                //for each simulation, the d1 which used in delta-hedged simulation is fixed, so I want to fix it before the simulation in order to make the Mento-carlo simulation more faster.
                double t = (j - 1) * dt;
                delta[j] = NormDistFunc((Math.Log(S / K) + (r + 0.5 * Math.Pow(sigma, 2) * t)) / (sigma * Math.Sqrt(t)));
            }
            for (uint i = 0; i < I; i++)
            {
                double callcv = 0;
                double putcv = 0;
                double Anticallcv = 0;
                double Antiputcv = 0;
                for (uint j = 1; j < N; j++)
                {
                    Vu[i, j] = Vu[i,j-1] * Math.Exp((r - 0.5 * Math.Pow(sigma, 2)) * dt + sigma * Math.Sqrt(dt) * this.randomNumbers[i, j]);
                    Vd[i, j] = Vd[i,j-1] * Math.Exp((r - 0.5 * Math.Pow(sigma, 2)) * dt + sigma * Math.Sqrt(dt) * (-1 * this.randomNumbers[i, j]));
                    callcv = callcv + (delta[j-1]) * (Vu[i, j] - Vu[i,j-1] * erddt);
                    Anticallcv = Anticallcv + delta[j-1] * (Vd[i, j] - Vd[i,j-1]* erddt);
                    putcv = putcv + (delta[j-1] - 1) * (Vu[i, j] - Vu[i,j-1] * erddt);
                    Antiputcv = Antiputcv + (delta[j-1] - 1) * (Vd[i, j] - Vd[i,j-1] * erddt);
                }
                Call[i] = Math.Max(Vu[i, N - 1] - K, 0) + beta1 * callcv;
                sumCT = sumCT + Call[i];
                sumCT2 = sumCT2 + Call[i] * Call[i];
                //this is the European call simulation
                AntiCall[i] = Math.Max(Vd[i, N - 1] - K, 0) + beta1* Anticallcv;
                sumAntiCT = sumAntiCT + AntiCall[i];
                sumAntiCT2 = sumAntiCT2 + AntiCall[i] * AntiCall[i];
                //this is the anti-path European call simulation
                sumzc = sumzc + (Call[i] + AntiCall[i])/2;
                sumzc2 = sumzc2 + 0.25 * Math.Pow((Call[i] + AntiCall[i]), 2);
                //sumzc and sumzc2 are used to calculate the two paths
                Put[i] = Math.Max(K - Vu[i, N - 1], 0) + beta1* putcv;
                sumPT = sumPT + Put[i];
                sumPT2 = sumPT2 + Put[i] * Put[i];
                //this European Put simulation
                AntiPut[i] = Math.Max(K - Vd[i, N - 1], 0) + beta1 * Antiputcv;
                sumAntiPT = sumAntiPT + AntiPut[i];
                sumAntiPT2 = sumAntiPT2 + AntiPut[i] * AntiPut[i];
                //this is the anti-path European Put simulation
                sumzp = sumzp + (Put[i] * AntiPut[i])/2;
                sumzp2 = sumzp2 + 0.25 * Math.Pow((Put[i] + AntiPut[i]), 2);
                //sumzp and sumzp2 are used to calculate the two paths
            }

            simulatedprice[0] = 0.5 * (sumCT + sumAntiCT) * Math.Exp(-r * T) / I;
            //the option value of european call option
            simulatedprice[1] = 0.5 * (sumPT + sumAntiPT) * Math.Exp(-r * T) / I;
            //the option value of european put option
            simulatedprice[2] = Math.Sqrt((sumCT2 - sumCT * sumCT / I) * Math.Exp(-2 * r * T) / (I - 1)) / Math.Sqrt(I);
            //the common standard error of european call option
            simulatedprice[3] = Math.Sqrt((sumPT2 - sumPT * sumPT / I) * Math.Exp(-2 * r * T) / (I - 1)) / Math.Sqrt(I);
            //the common standard error of european put option
            simulatedprice[4] = Math.Sqrt((sumzc2 - sumzc * sumzc / I) * Math.Exp(-2 * r * T) / (I - 1)) / Math.Sqrt(I);
            //the variance reduction standard error value of european call option
            simulatedprice[5] = Math.Sqrt((sumzp2 - sumzp * sumzp / I) * Math.Exp(-2 * r * T) / (I - 1)) / Math.Sqrt(I);
            //the variance reduction standard error value of european put option
            return simulatedprice;
        }
        public static double NormDistFunc(double x)
        {
            //the code of the Standard Normal distribution is from the CSDN.
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x) / Math.Sqrt(2.0);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return 0.5 * (1.0 + sign * y);
        }
        static void TestPhi()
        {
            // Select a few input values
            double[] x =
            {
        -3,
        -1,
        0.0,
        0.5,
        2.1
    };

            // Output computed by Mathematica
            // y = Phi[x]
            double[] y =
            {
        0.00134989803163,
        0.158655253931,
        0.5,
        0.691462461274,
        0.982135579437
    };

            double maxError = 0.0;
            for (int i = 0; i < x.Length; ++i)
            {
                double error = Math.Abs(y[i] - NormDistFunc(x[i]));
                if (error > maxError)
                    maxError = error;
            }

            Console.WriteLine("Maximum error: {0}", maxError);
        }

        public double[] Greeks(double S, double K, double T, double r, double sigma, Int32 N, Int32 I)
        {
            double[] _Greeks = new double[10];
            double df = 0.001;
            _Greeks[0] = (SimulatedPrice(S + df * S, K, T, r, sigma, N, I)[0] - SimulatedPrice(S - df * S, K, T, r, sigma, N, I)[0]) / (2 * df * S);
            //the delta greek of European Call Option
            _Greeks[1] = (SimulatedPrice(S + df * S, K, T, r, sigma, N, I)[1] - SimulatedPrice(S - df * S, K, T, r, sigma, N, I)[1]) / (2 * df * S);
            //the delta greek of European Put Option.
            _Greeks[2] = (SimulatedPrice(S + S * df, K, T, r, sigma, N, I)[0] - 2 * SimulatedPrice(S, K, T, r, sigma, N, I)[0] + SimulatedPrice(S - df * S, K, T, r, sigma, N, I)[0]) / (df * df * S * S);
            //the gamma greek of European Call Option.
            _Greeks[3] = (SimulatedPrice(S + S * df, K, T, r, sigma, N, I)[1] - 2 * SimulatedPrice(S, K, T, r, sigma, N, I)[1] + SimulatedPrice(S - df * S, K, T, r, sigma, N, I)[1]) / (df * df * S * S);
            //the gamma greek of European Put Option.
            _Greeks[4] = (SimulatedPrice(S, K, T, r, sigma + df * sigma, N, I)[0] - SimulatedPrice(S, K, T, r, sigma - df * sigma, N, I)[0]) / (2 * df * sigma);
            //the vega greek of European Call Option
            _Greeks[5] = (SimulatedPrice(S, K, T, r, sigma + df * sigma, N, I)[1] - SimulatedPrice(S, K, T, r, sigma - df * sigma, N, I)[1]) / (2 * df * sigma);
            //the vega greek of European Put Option.
            _Greeks[6] = -((SimulatedPrice(S, K, T + df * T, r, sigma, N, I)[0] - SimulatedPrice(S, K, T, r, sigma, N, I)[0]) / (df * T));
            //the theta greek of European Call Option
            _Greeks[7] = -((SimulatedPrice(S, K, T + df * T, r, sigma, N, I)[1] - SimulatedPrice(S, K, T, r, sigma, N, I)[1]) / (df * T));
            //the theta greek of European Put Option.
            _Greeks[8] = (SimulatedPrice(S, K, T, r + r * df, sigma, N, I)[0] - SimulatedPrice(S, K, T, r - r * df, sigma, N, I)[0]) / (2 * df * r);
            //the rho greek of European Call Option
            _Greeks[9] = (SimulatedPrice(S, K, T, r + r * df, sigma, N, I)[1] - SimulatedPrice(S, K, T, r - r * df, sigma, N, I)[1]) / (2 * df * r);
            //the rho greek of European Put Option.
            return _Greeks;
        }


    }
}
