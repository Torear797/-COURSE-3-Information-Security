using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    class PrimeNumbersGenerator
    {
        private int length { get; set; }

        public PrimeNumbersGenerator(int _length)
        {
            length = _length;
        }

        private bool MillerRabinTest(BigInteger n, int k)
        {
            if (n == 2 || n == 3)
                return true;
            if (n < 2 || n % 2 == 0)
                return false;
            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }
            for (int i = 0; i < k; i++)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] _a = new byte[n.ToByteArray().LongLength];
                BigInteger a;
                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);
                BigInteger x = BigInteger.ModPow(a, t, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            return true;
        }

        private BigInteger getRandomBigNumber(int length)
        {
            int UnsignLength = length;
            if ((length + 1) % 8 == 0 || length % 8 == 0)
            {
                UnsignLength += 8;
            }
            byte[] data = new byte[Convert.ToInt32(Math.Ceiling(UnsignLength / 8.0))];
            Random random = new Random();
            random.NextBytes(data);
            BitArray gamma_value = new BitArray(data);
            for (int i = gamma_value.Length - 1; i >= length; i--)
            {
                gamma_value[i] = false;
            }
            gamma_value[0] = true;
            gamma_value[length - 1] = true;
            gamma_value.CopyTo(data, 0);
            return new BigInteger(data);
        }
        public BigInteger PrimeNumberGeneration()
        {
            while (true)
            {
                BigInteger bigInteger = getRandomBigNumber(length);
                if (MillerRabinTest(bigInteger, 40))
                {
                    return bigInteger;
                }
            }

        }

    }
}
