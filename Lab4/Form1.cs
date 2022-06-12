using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.IO;
using System.Collections;

namespace IS_4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        static List<int> PrimeNumbers = new List<int>();
        bool karmaykl = false;
        List<BigInteger> karmayklList = new List<BigInteger>() {561,1105, 41041, 62745, 63973, 75361, 101101, 126217, 172081, 188461, 278545, 340561, 449065, 552721, 656601, 658801, 670033, 748657, 838201, 852841, 997633, 1033669, 1082809, 1569457, 1773289, 2100901, 2113921, 2433601, 2455921 };
        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch findTime = new Stopwatch();
            BigInteger number = (BigInteger)numericUpDown1.Value;
            if (comboBox1.SelectedIndex == 0)
            {
                findTime.Start();
                if (Ferma(number))
                {
                    findTime.Stop();
                    label6.Text = findTime.ElapsedMilliseconds.ToString();
					if (karmaykl == false)
                    MessageBox.Show("Число скорее всего простое");
                }
                else
                {
                    findTime.Stop();
                    label6.Text = findTime.ElapsedMilliseconds.ToString();
                   MessageBox.Show("Число составное");
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                findTime.Start();
                if (MillerRabinTest(number, (int)numericUpDown2.Value))
                {
                    findTime.Stop();
                    label6.Text = findTime.ElapsedMilliseconds.ToString();
                    MessageBox.Show("Число скорее всего простое");
                }
                else
                {
                    findTime.Stop();
                    label6.Text = findTime.ElapsedMilliseconds.ToString();
                    MessageBox.Show("Число составное");
                } 
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                findTime.Start();
                if (Combine(number, (int)numericUpDown2.Value))
                {
                    findTime.Stop();
                    label6.Text = findTime.ElapsedMilliseconds.ToString();
                    MessageBox.Show("Число скорее всего простое");
                }
                else
                {
                    findTime.Stop();
                    label6.Text = findTime.ElapsedMilliseconds.ToString();
                    MessageBox.Show("Число составное");
                }
            }
        }
        public bool Ferma(BigInteger n)
        {
            karmaykl = false;
            if (n < 5 || n % 2 == 0) return false;    
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] _a = new byte[n.ToByteArray().LongLength];
            BigInteger a;
            do
            {
                rng.GetBytes(_a);
                a = new BigInteger(_a);
            }
            while (a < 2 || a >= n - 1);
            BigInteger r = BigInteger.ModPow(a, n - 1, n);
            if (r == 1)
			{ 
				if (karmayklList.Contains(n)) { MessageBox.Show("Число Кармайкла."); karmaykl = true; }
				return true; 
			}
			else return false;
        }
        public bool MillerRabinTest(BigInteger n, int k)
        {
            // если n == 2 или n == 3 - эти числа простые
            if (n == 2 || n == 3) return true;
            // если n < 2 или n четное
            if (n < 2 || n % 2 == 0)  return false;
            //Представление n − 1 в виде (2^s)·t, где t нечётно, делаю с помощью последовательного деления n - 1 на 2

            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            {
                // генерация случайного чилса с диапазоне [2, n − 2]
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] _a = new byte[n.ToByteArray().LongLength];
                BigInteger a;
                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);
                // x = a^t mod n
                BigInteger x = BigInteger.ModPow(a, t, n);
                if (x == 1 || x == n - 1)continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1) return false;
                    if (x == n - 1)break;
                }
                if (x != n - 1) return false;
            }
            return true;
        }
        public bool Combine(BigInteger n, int k)
        {
            if (n == 2 || n == 3) return true;
            if (n < 2 || n % 2 == 0)return false;
            BigInteger t = n - 1;
            for (int i = 3; i < 1000; i++)
                if (n % i == 0) return false; //если != 0 - то число составное.
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
                if (x == 1 || x == n - 1) continue;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1) return false;
                    if (x == n - 1)break;
                }
                if (x != n - 1) return false;
            }
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch findTime = new Stopwatch();
            findTime.Start();
            button3.Visible = false; button4.Visible = true;
            if (PrimeNumbers.Count == 0)
            {
                ReadFile();
            }
            string text = "";
            while (true)
            {
                BigInteger bigInteger = Gen();

                if (bigInteger != 0)
                {
                    text = bigInteger.ToString();
                    numericUpDown1.Value = Convert.ToUInt64(text);
                    break;
                }
            }
            findTime.Stop();
            label6.Text = findTime.ElapsedMilliseconds.ToString();
        }
        public void ReadFile()
        {
            using (StreamReader sr = new StreamReader("inp.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    PrimeNumbers.Add(Convert.ToInt32(line));
                }

            }
        }
        public BigInteger Gen()
        {
            BigInteger bigInteger = RandomBIGINT(Convert.ToInt32(numericUpDown2.Value));
            foreach (int PrimeNumber in PrimeNumbers)
            {
                if (bigInteger % PrimeNumber == 0)
                    return 0;
            }
            button3.Visible = true;
            if (!MillerRabinTest(bigInteger, 5))
            {
                button4.Visible = false;
                return 0;
            } 
             button4.Visible = true;
            return bigInteger;
        }
        public BigInteger RandomBIGINT(int length)
        {
            int UnsignLength = length;
            if ((length + 1) % 8 == 0 || length % 8 == 0)
               UnsignLength += 8;
            byte[] data = new byte[Convert.ToInt32(Math.Ceiling(UnsignLength / 8.0))];
            Random random = new Random();
            random.NextBytes(data);
            BitArray gamma_value = new BitArray(data);
            for (int i = gamma_value.Length - 1; i > length; i--)
             gamma_value[i] = false;
            
            gamma_value[0] = true;
            gamma_value[length - 1] = true;
            gamma_value.CopyTo(data, 0);
            BigInteger bigInteger = new BigInteger(data);
            return new BigInteger(data);
        }    
    }
}