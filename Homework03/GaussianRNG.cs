using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework02
{
    public class GaussianRNG
    {
        ///it is used to generate normally distributed random numbers
        ///Gaussian(Box-Muller) Random Number Generator Class
        double iset, gset;
        Random r1, r2;
        public GaussianRNG()
        {
            r1 = new Random(unchecked((int)DateTime.Now.Ticks));
            r2 = new Random(~unchecked((int)DateTime.Now.Ticks));
            iset = 0;
        }
        public double NextDouble()
        {
            double x, y, v1, v2;
            if (iset == 0)
            {
                do
                {
                    v1 = 2.0 * r1.NextDouble() - 1.0;
                    v2 = 2.0 * r2.NextDouble() - 1.0;
                    y = v1 * v1 + v2 * v2;
                }
                while (y >= 1.0 || y == 0.0);
                x = Math.Sqrt(-2.0 * Math.Log(y) / y);
                gset = v1 * x;
                iset = 1;
                return v2 * x;
            }
            else
            {
                iset = 0;
                return gset;
            }
        }
        public double[,] RandomMatrix(Int32 N, Int32 I)
        {
            double[,] RanNum = new double[I, N];
            for (uint i = 0; i < I; i++)
            {
                for (uint j = 0; j < N; j++)
                {
                    RanNum[i, j] = NextDouble();
                }

            }
            return RanNum;
        }
    }
}
