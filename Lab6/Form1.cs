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

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       // char[] alphabet = new char[]  //Алфавит
        //{
        //    'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
        //    'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ','Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7','8', '9', '0'
        //};
        string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯ 1234567890,.()-";
        BigInteger p, q;

        private void button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                BigInteger bigIntegerP = PrimeNumberGeneration();
                System.Threading.Thread.Sleep(10);
                BigInteger bigIntegerQ = PrimeNumberGeneration();
                if (bigIntegerP != 0 && bigIntegerQ !=0)
                {
                    textBox1.Text = bigIntegerP.ToString();
                    textBox2.Text = bigIntegerQ.ToString();             
                    break;
                }
            }
        }
        public BigInteger getRandomBigNumber(int length)
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
            BigInteger bigInteger = getRandomBigNumber(Convert.ToInt32(textBox3.Text));
            if (!MillerRabinTest(bigInteger, 25))
            {
                return 0;
            }
            return bigInteger;
        }
        public bool MillerRabinTest(BigInteger n, int k)
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
        private void button3_Click(object sender, EventArgs e)//Кнопка для шифрования
        {
             p = BigInteger.Parse(textBox1.Text);
             q = BigInteger.Parse(textBox2.Text);
            string text = textBox4.Text.ToUpper(), KU = "", KR ="", cryptText = "";
            BigInteger n, fi, d, E, TextKey;
            n = p * q;
            fi = (p - 1) * (q - 1);
            E = GetE(fi);
            //E = 5;
            d = Getd(fi,E,n);
            KU = E.ToString() + " " + n.ToString();
            KR = d.ToString() + " " + n.ToString();
            using (FileStream fstream = new FileStream("KU.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(KU);
                fstream.Write(array, 0, array.Length);
            }
            using (FileStream fstream = new FileStream("KR.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(KR);
                fstream.Write(array, 0, array.Length);
            }
          //  TextKey = alphabet.IndexOf(text[0]);
            for (int i = 0; i < text.Length; i++)
            {
                TextKey = BigInteger.Pow(alphabet.IndexOf(text[i]),(int) E) % n;
                cryptText += TextKey.ToString() + " ";
            }
            textBox5.Text = cryptText;
            textBox7.Text = d.ToString();
            textBox6.Text = n.ToString();
        }
        BigInteger Gcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (b < a)
            {
                var t = a;
                a = b;
                b = t;
            }

            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            BigInteger gcd = Gcd(b % a, a, out x, out y);

            BigInteger newY = x;
            BigInteger newX = y - (b / a) * x;

            x = newX;
            y = newY;
            return gcd;
        }
        private void button2_Click(object sender, EventArgs e) //Чтение p и q из файла.
        {
            string number = "";
            using (FileStream fstream = File.OpenRead("EnterP.txt"))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                number = System.Text.Encoding.Default.GetString(array);
                p = Convert.ToInt64(number, 2);                     // ОЧЕНЬ ВАЖНО!!!!!!! Если знаешь как сделать для бига - милости прошу, а пока так!
                if (!MillerRabinTest(p, 25)) { MessageBox.Show("Р - не простое число!Введите другое Р."); p = 0; } 
                textBox1.Text = p.ToString();
            }
            using (FileStream fstream = File.OpenRead("EnterQ.txt"))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                number = System.Text.Encoding.Default.GetString(array);
                q = Convert.ToInt64(number, 2);                     // ОЧЕНЬ ВАЖНО!!!!!!! Если знаешь как сделать для бига - милости прошу, а пока так!
                if (!MillerRabinTest(p, 25)) { MessageBox.Show("q - не простое число!Введите другое q."); q = 0; }
                textBox2.Text = p.ToString();
            }
        }
        private BigInteger GetE(BigInteger n) //Получаем значение е
        {
            BigInteger e = 2,x,y;
            while (true)
            {
                if(e == n) break;
                if (Gcd(n,e, out x, out y) == 1)
                    break;
                else
                    e++;
            }
            return e;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.Text = textBox5.Text;
            textBox5.Text = "";
        }

        private void button4_Click(object sender, EventArgs e) //Кнопка для дешифрования
        {
            BigInteger d = BigInteger.Parse(textBox7.Text);
            BigInteger n = BigInteger.Parse(textBox6.Text);
            BigInteger TextKey;
            string text = textBox4.Text.ToUpper(), cryptText = "" , word = "";
            int j;
            for (int i = 0; i < text.Length; i++)
            {
                j = i; word = "";
                while (text[j].ToString() != " ")//Преобразуем считанное число-строку в строку
                {
                    word += text[j].ToString();
                    j++;
                }
                TextKey = BigInteger.Pow(BigInteger.Parse(word), (int)d) % n; //Вычисляем
                cryptText += alphabet[(int)TextKey];
                i = j;
            }
            textBox5.Text = cryptText;
            textBox7.Text = d.ToString();
            textBox6.Text = n.ToString();
        }

        private BigInteger Getd(BigInteger fi , BigInteger E, BigInteger n) //Получаем значение d
        {
            BigInteger d = 2;

            while (true)
            {
                if (d == n) break;
                if ((E * d) % fi == 1)
                    break;
                else
                    d++;
            }

            return d;
        }
    }
}
