using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Form1 : Form
    {
        Thread receiveThread;
        static string remoteAddress; // хост для отправки данных //IP компаньона 
        static int remotePort; // порт для отправки данных //Порт компаньона
        static int localPort; // локальный порт для прослушивания входящих подключений  // Мой порт
        public bool continueRecieve = true;

        BigInteger x = 0;
        static BigInteger G = 12;
        static BigInteger p = 35911;
        BigInteger K = 0;

        public Form1()
        {
            InitializeComponent();
        }
        private void ReceiveMessage()
        {
            string messageRecv = "";
            UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (continueRecieve == true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    BigInteger message = new BigInteger(data);
                    messageRecv = message.ToString();    
                    SetTextSafe(message.ToString());
                    SetTextSafe2(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
        private void SendMessage()
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                BigInteger message = BigInteger.Parse(textBox11.Text);
                //string message = textBox2.Text; // сообщение для отправки
                byte[] data = message.ToByteArray();
                sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int Number = 0; bool tryOK = false;

            try
            {
                Number = Convert.ToInt32(textBox1.Text, 2);
                tryOK = true;
                MessageBox.Show("Число в 2 системе.");
            }
            catch (Exception)
            {
                try
                {
                    Number = Convert.ToInt32(textBox1.Text, 10);
                    tryOK = true;
                    MessageBox.Show("Число в 10 системе.");
                }
                catch (Exception)
                {
                    try
                    {
                        Number = Convert.ToInt32(textBox1.Text, 16);
                        tryOK = true;
                        MessageBox.Show("Число в 16 системе.");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Не верный формат числа.");
                    }
                }
            }
           // if (tryOK && MillerRabinTest(Number, 5))
            {
                Number = Convert.ToInt32(Number);
                dataGridView1.RowCount = 100;
                dataGridView1.ColumnCount = 2;
                Stopwatch findTime = new Stopwatch();
                findTime.Start();

                GetPRoot(Number);

                findTime.Stop();
                label6.Text = (findTime.Elapsed).ToString();

            }
          //  else MessageBox.Show("НЕ верное число.","Ошибка.");
        }
        public BigInteger GetPRoot(BigInteger p)
        {
            int T = 0;
            for (BigInteger i = 0; i < p; i++) 
            if (IsPRoot(p, i))
                {
                    if (T == 99) break;
                        dataGridView1.Rows[T].Cells[0].Value = T;
                        dataGridView1.Rows[T].Cells[1].Value = i;
                    T++;
                }
            return 0;
        }
        public bool IsPRoot(BigInteger p, BigInteger a)
        {
            if (a == 0 || a == 1) return false;
            BigInteger last = 1; 
            HashSet<BigInteger> set = new HashSet<BigInteger>();
            for (BigInteger i = 0; i < p - 1; i++)
            {
                last = (last * a) % p;
                if (set.Contains(last)) return false;
                set.Add(last);
            }
            return true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (receiveThread == null)
            {
                remoteAddress = textBox5.Text;
                remotePort = (int)numericUpDown1.Value;
                localPort = (int)numericUpDown2.Value;
                receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                MessageBox.Show("Соединение установлено!");
                button3.Text = "Разрыв";
            }
            else {
                continueRecieve = false;
                receiveThread.Abort();
                MessageBox.Show("Соединение завершено!");
                button3.Text = "Подключение";
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (receiveThread != null)
            {
                continueRecieve = false;
                receiveThread.Abort();
            }
            Application.Exit();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox9.Text != "")
            {
                 x = BigInteger.Parse(textBox2.Text);
                 K = BigInteger.ModPow(G, x, p);
                textBox11.Text = K.ToString();
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
        void SetTextSafe(string newText)
        {
            if (textBox3.InvokeRequired) textBox3.Invoke(new Action<string>((s) => textBox3.Text = s), newText);
            else  textBox3.Text = newText;
        }
        void SetTextSafe2(BigInteger newText)
        {
            x = BigInteger.Parse(textBox2.Text);
            BigInteger R = BigInteger.ModPow(newText, x, p);

            if (textBox4.InvokeRequired) textBox4.Invoke(new Action<string>((s) => textBox4.Text = s), R.ToString());
            else textBox4.Text = R.ToString();
        }
        public bool MillerRabinTest(BigInteger n, int k)
        {
            // если n == 2 или n == 3 - эти числа простые
            if (n == 2 || n == 3) return true;
            // если n < 2 или n четное
            if (n < 2 || n % 2 == 0) return false;
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
                if (x == 1 || x == n - 1) continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1) return false;
                    if (x == n - 1) break;
                }
                if (x != n - 1) return false;
            }
            return true;
        }
    }
}
