using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯ 1234567890,.()-";
        static void DBL_INT_ADD(ref uint a, ref uint b, uint c)
        {
            if (a > 0xffffffff - c) ++b; a += c;
        }
        static uint ROTLEFT(uint a, byte b)
        {
            return ((a << b) | (a >> (32 - b)));
        }
        static uint ROTRIGHT(uint a, byte b)
        {
            return (((a) >> (b)) | ((a) << (32 - (b))));
        }
        static uint CH(uint x, uint y, uint z)
        {
            return (((x) & (y)) ^ (~(x) & (z)));
        }
        static uint MAJ(uint x, uint y, uint z)
        {
            return (((x) & (y)) ^ ((x) & (z)) ^ ((y) & (z)));
        }
        static uint EP0(uint x)
        {
            return (ROTRIGHT(x, 2) ^ ROTRIGHT(x, 13) ^ ROTRIGHT(x, 22));
        }
        static uint EP1(uint x)
        {
            return (ROTRIGHT(x, 6) ^ ROTRIGHT(x, 11) ^ ROTRIGHT(x, 25));
        }
        static uint SIG0(uint x)
        {
            return (ROTRIGHT(x, 7) ^ ROTRIGHT(x, 18) ^ ((x) >> 3));
        }
        static uint SIG1(uint x)
        {
            return (ROTRIGHT(x, 17) ^ ROTRIGHT(x, 19) ^ ((x) >> 10));
        }
        struct SHA256_CTX
        {
            public byte[] data;  //Размер сообщения 64 бита
            public uint datalen;
            public uint[] bitlen;
            public uint[] state;
        }
        static uint[] k = {
    0x428a2f98,0x71374491,0xb5c0fbcf,0xe9b5dba5,0x3956c25b,0x59f111f1,0x923f82a4,0xab1c5ed5,
    0xd807aa98,0x12835b01,0x243185be,0x550c7dc3,0x72be5d74,0x80deb1fe,0x9bdc06a7,0xc19bf174,
    0xe49b69c1,0xefbe4786,0x0fc19dc6,0x240ca1cc,0x2de92c6f,0x4a7484aa,0x5cb0a9dc,0x76f988da,
    0x983e5152,0xa831c66d,0xb00327c8,0xbf597fc7,0xc6e00bf3,0xd5a79147,0x06ca6351,0x14292967,
    0x27b70a85,0x2e1b2138,0x4d2c6dfc,0x53380d13,0x650a7354,0x766a0abb,0x81c2c92e,0x92722c85,
    0xa2bfe8a1,0xa81a664b,0xc24b8b70,0xc76c51a3,0xd192e819,0xd6990624,0xf40e3585,0x106aa070,
    0x19a4c116,0x1e376c08,0x2748774c,0x34b0bcb5,0x391c0cb3,0x4ed8aa4a,0x5b9cca4f,0x682e6ff3,
    0x748f82ee,0x78a5636f,0x84c87814,0x8cc70208,0x90befffa,0xa4506ceb,0xbef9a3f7,0xc67178f2
};
        static void SHA256Transform(ref SHA256_CTX ctx, byte[] data)
        {
            uint a, b, c, d, e, f, g, h, i, j, t1, t2;
            uint[] m = new uint[64];

            for (i = 0, j = 0; i < 16; ++i, j += 4)
                m[i] = (uint)((data[j] << 24) | (data[j + 1] << 16) | (data[j + 2] << 8) | (data[j + 3]));

            for (; i < 64; ++i)
                m[i] = SIG1(m[i - 2]) + m[i - 7] + SIG0(m[i - 15]) + m[i - 16];

            a = ctx.state[0];
            b = ctx.state[1];
            c = ctx.state[2];
            d = ctx.state[3];
            e = ctx.state[4];
            f = ctx.state[5];
            g = ctx.state[6];
            h = ctx.state[7];

            for (i = 0; i < 64; ++i)
            {
                t1 = h + EP1(e) + CH(e, f, g) + k[i] + m[i];
                t2 = EP0(a) + MAJ(a, b, c);
                h = g;
                g = f;
                f = e;
                e = d + t1;
                d = c;
                c = b;
                b = a;
                a = t1 + t2;
            }

            ctx.state[0] += a;
            ctx.state[1] += b;
            ctx.state[2] += c;
            ctx.state[3] += d;
            ctx.state[4] += e;
            ctx.state[5] += f;
            ctx.state[6] += g;
            ctx.state[7] += h;
        }
        static void SHA256Init(ref SHA256_CTX ctx)
        {
            ctx.datalen = 0;
            ctx.bitlen[0] = 0;
            ctx.bitlen[1] = 0;
            ctx.state[0] = 0x6a09e667;
            ctx.state[1] = 0xbb67ae85;
            ctx.state[2] = 0x3c6ef372;
            ctx.state[3] = 0xa54ff53a;
            ctx.state[4] = 0x510e527f;
            ctx.state[5] = 0x9b05688c;
            ctx.state[6] = 0x1f83d9ab;
            ctx.state[7] = 0x5be0cd19;
        }
        static void SHA256Update(ref SHA256_CTX ctx, byte[] data, uint len)
        {
            for (uint i = 0; i < len; ++i)
            {
                ctx.data[ctx.datalen] = data[i];
                ctx.datalen++;

                if (ctx.datalen == 64)
                {
                    SHA256Transform(ref ctx, ctx.data);
                    DBL_INT_ADD(ref ctx.bitlen[0], ref ctx.bitlen[1], 512);
                    ctx.datalen = 0;
                }
            }
        }
        static void SHA256Final(ref SHA256_CTX ctx, byte[] hash)
        {
            uint i = ctx.datalen;

            if (ctx.datalen < 56)
            {
                ctx.data[i++] = 0x80;

                while (i < 56)
                    ctx.data[i++] = 0x00;
            }
            else
            {
                ctx.data[i++] = 0x80;

                while (i < 64)
                    ctx.data[i++] = 0x00;

                SHA256Transform(ref ctx, ctx.data);
            }

            DBL_INT_ADD(ref ctx.bitlen[0], ref ctx.bitlen[1], ctx.datalen * 8);
            ctx.data[63] = (byte)(ctx.bitlen[0]);
            ctx.data[62] = (byte)(ctx.bitlen[0] >> 8);
            ctx.data[61] = (byte)(ctx.bitlen[0] >> 16);
            ctx.data[60] = (byte)(ctx.bitlen[0] >> 24);
            ctx.data[59] = (byte)(ctx.bitlen[1]);
            ctx.data[58] = (byte)(ctx.bitlen[1] >> 8);
            ctx.data[57] = (byte)(ctx.bitlen[1] >> 16);
            ctx.data[56] = (byte)(ctx.bitlen[1] >> 24);
            SHA256Transform(ref ctx, ctx.data);

            for (i = 0; i < 4; ++i)
            {
                hash[i] = (byte)(((ctx.state[0]) >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 4] = (byte)(((ctx.state[1]) >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 8] = (byte)(((ctx.state[2]) >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 12] = (byte)((ctx.state[3] >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 16] = (byte)((ctx.state[4] >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 20] = (byte)((ctx.state[5] >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 24] = (byte)((ctx.state[6] >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 28] = (byte)((ctx.state[7] >> (int)(24 - i * 8)) & 0x000000ff);
            }
        }
        static string SHA256(string data)
        {
            SHA256_CTX ctx = new SHA256_CTX();
            ctx.data = new byte[64];
            ctx.bitlen = new uint[2];
            ctx.state = new uint[8];

            byte[] hash = new byte[32]; //32 - размер слова
            string hashStr = string.Empty;

            SHA256Init(ref ctx);
            SHA256Update(ref ctx, Encoding.Default.GetBytes(data), (uint)data.Length);
            SHA256Final(ref ctx, hash);

            for (int i = 0; i < 32; i++)
            {
                hashStr += string.Format("{0:X2}", hash[i]);
            }

            return hashStr;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if (text != "")
                textBox2.Text = SHA256(text).ToLower();
            else
                MessageBox.Show("Введите сообщение!");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e) //Создание подписи RSA
        {
            BigInteger d = BigInteger.Parse(textBox7.Text);
            BigInteger n = BigInteger.Parse(textBox6.Text);
            BigInteger s;
            string text = textBox8.Text.ToUpper(), signature ="";
            for(int i = 0; i < text.Length; i++)
            {
                if (alphabet.Contains(text[i].ToString()))
                {
                    s = BigInteger.ModPow(alphabet.IndexOf(text[i]), d, n);
                    signature += s.ToString() + " ";
                }
            }
            textBox10.Text = signature;
        }
        private BigInteger GetE(BigInteger n) //Получаем значение е
        {
            BigInteger e = 2;
            while (true)
            {
                if (e == n) break;
                if (BigInteger.GreatestCommonDivisor(n, e) == 1)
                    break;
                else
                    e++;
            }
            return e;
        }
        private BigInteger Getd(BigInteger fi, BigInteger E, BigInteger n) //Получаем значение d
        {
            BigInteger x = 0, y = 0;
            Gcd(fi, E, out x, out y);
            if (x < 0)
            {
                x = BigInteger.ModPow(x, 1, fi);
                x = BigInteger.Add(x, fi);
            }
            return x;
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
        private void button5_Click_1(object sender, EventArgs e) //Проверка подписи RSA
        {
            BigInteger p = BigInteger.Parse(textBox5.Text);
            BigInteger q = BigInteger.Parse(textBox4.Text);
            BigInteger fi = BigInteger.Multiply((p - 1), (q - 1));
            BigInteger E = GetE(fi);
            BigInteger n = BigInteger.Parse(textBox6.Text);
            BigInteger checkSignature,s;
            string originalText = textBox9.Text, checkText = "";
            string[] splitwords = textBox11.Text.Split(' ');
           
            foreach (string words in splitwords)
            {
                if (words != "")
                {
                    s = BigInteger.Parse(words);
                    checkSignature = BigInteger.ModPow(s, E, n);
                    checkText += alphabet[(int)checkSignature];
                }
            }

            if (originalText == checkText) MessageBox.Show("Подпись оригинальная!");
            else MessageBox.Show("Подпись поддельная!");
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            BigInteger bigIntegerP = 0, bigIntegerQ = 0;
            PrimeNumbersGenerator primeNumbersGenerator = new PrimeNumbersGenerator(Int32.Parse(textBox3.Text));
            bigIntegerP = primeNumbersGenerator.PrimeNumberGeneration();
            System.Threading.Thread.Sleep(20);
            bigIntegerQ = primeNumbersGenerator.PrimeNumberGeneration();

            BigInteger n = BigInteger.Multiply(bigIntegerP, bigIntegerQ);
            BigInteger fi = BigInteger.Multiply((bigIntegerP - 1), (bigIntegerQ - 1));
            BigInteger E = GetE(fi);
            BigInteger d = Getd(fi, E, n);

            textBox5.Text = bigIntegerP.ToString();
            textBox4.Text = bigIntegerQ.ToString();
            textBox7.Text = d.ToString();
            textBox6.Text = n.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox9.Text = textBox8.Text.ToUpper();
            textBox11.Text = textBox10.Text;
        }

        private void button8_Click(object sender, EventArgs e) //Создание подписи Эль-гамаля
        {
            PrimeNumbersGenerator primeNumbers = new PrimeNumbersGenerator(Int32.Parse(textBox16.Text));
            BigInteger p, A, X, y, k,a;
            p = BigInteger.Parse(textBox13.Text); //Генерация р     
            A = BigInteger.Parse(textBox14.Text);//Генерация А
            X = BigInteger.Parse(textBox17.Text);

            y = BigInteger.ModPow(A, X, p); //Вычисление у
            textBox18.Text = y.ToString();

            k = BigInteger.Parse(textBox19.Text);
         
            a = BigInteger.ModPow(A, k, p);
            textBox15.Text = a.ToString(); //Генерация a

            BigInteger m = new BigInteger(SHA256Byte(textBox12.Text));
            m = 797;
            if (m < 0)
                m = BigInteger.Negate(m);

            BigInteger t1 = 0, t2 = 0;
            RAE(k, p - 1, out t1, out t2);//Нахождение мультиплекативной инверсии

            BigInteger b = BigInteger.ModPow(BigInteger.Multiply(m - BigInteger.Multiply(X, a), t1), 1, p - 1);

            if (b < 0)
            {
                b = BigInteger.Add(b, p - 1); //генерация б
            }
            textBox20.Text = b.ToString();
        }
        static byte[] SHA256Byte(string data)
        {
            SHA256_CTX ctx = new SHA256_CTX();
            ctx.data = new byte[64];
            ctx.bitlen = new uint[2];
            ctx.state = new uint[8];

            byte[] hash = new byte[32];

            SHA256Init(ref ctx);
            SHA256Update(ref ctx, Encoding.Default.GetBytes(data), (uint)data.Length);
            SHA256Final(ref ctx, hash);

            return hash;
        }
        public BigInteger RAE(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
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
            BigInteger gcd = RAE(BigInteger.ModPow(b, 1, a), a, out x, out y);
            BigInteger newY = x;
            BigInteger newX = y - BigInteger.Multiply(BigInteger.Divide(b, a), x);
            x = newX;
            y = newY;
            return gcd;
        }

        private void button7_Click(object sender, EventArgs e) //Проверка подписи эль-Гамаля
        {
                BigInteger digest = new BigInteger(SHA256Byte(textBox26.Text));
                if (digest < 0)
                    digest = BigInteger.Negate(digest);
            digest = 797;

            BigInteger y, r, s, yr, rs, g, gm, p;

                p = BigInteger.Parse(textBox13.Text);
                g = BigInteger.Parse(textBox14.Text);
                y = BigInteger.Parse(textBox23.Text);
                r = BigInteger.Parse(textBox25.Text);
                s = BigInteger.Parse(textBox21.Text);

                yr = BigInteger.ModPow(y, r, p);
                rs = BigInteger.ModPow(r, s, p);

                BigInteger proiz = BigInteger.Multiply(yr, rs);
                proiz = BigInteger.ModPow(proiz, 1, p);
                gm = BigInteger.ModPow(g, digest, p);

            if (BigInteger.Compare(proiz, gm) == 0)
                MessageBox.Show("Подпись оригинальная!");
            else
                MessageBox.Show("Подпись поддельная!");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox23.Text = textBox18.Text;
            textBox25.Text = textBox15.Text;
            textBox21.Text = textBox20.Text;
            textBox26.Text = textBox12.Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            PrimeNumbersGenerator primeNumbers = new PrimeNumbersGenerator(Int32.Parse(textBox16.Text));
            BigInteger p, A, X, y, k, r;
            p = primeNumbers.PrimeNumberGeneration(); //Генерация р
            System.Threading.Thread.Sleep(50);
            A = primeNumbers.PrimeNumberGeneration();//Генерация А
            textBox13.Text = p.ToString();
            textBox14.Text = A.ToString();
            System.Threading.Thread.Sleep(50);
            do
            {
                X = primeNumbers.PrimeNumberGeneration(); //Генерация х
            } while (X > p);
            textBox17.Text = X.ToString();
            System.Threading.Thread.Sleep(50);
            do
            {
                k = primeNumbers.PrimeNumberGeneration(); //Генерация к
            } while (k > p - 1);
            textBox19.Text = k.ToString();
        }
    }
}
