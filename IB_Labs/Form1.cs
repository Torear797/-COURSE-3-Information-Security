using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IB_Labs
{
    public partial class Form1 : Form
    {
        string alfavit = "абвгдеёжзийклмнопрстуфхцчшщъьыэюя ,." , P = "", C = "",inputFile = "", outputFile = "", Key = "";
        string NewP = "", NewMatr = "" , alfavit2 = "абвгдеёжзийклмнопрстуфхцчшщъьыэюя ,.!", alfavit3 = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        string[,] matr = new string[6, 6]; int x; int y;
        int K = 0;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowCount = 6;
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Width = 20;
            dataGridView1.Columns[1].Width = 20;
            dataGridView1.Columns[2].Width = 20;
            dataGridView1.Columns[3].Width = 20;
            dataGridView1.Columns[4].Width = 20;
            dataGridView1.Columns[5].Width = 20;
        }
        int Gcd(int a, int b, out int x, out int y)
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

            int gcd = Gcd(b % a, a, out x, out y);

            int newY = x;
            int newX = y - (b / a) * x;

            x = newX;
            y = newY;
            return gcd;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Text = "Шифровать";
            button6.Text = "Шифровать";
            button7.Text = "Шифровать";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Text = "Расшифровать";
            button6.Text = "Расшифровать";
            button7.Text = "Расшифровать";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            inputFile = "";
            label4.Text = "Файл не выбран!";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = ""; richTextBox2.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int Count = 0; string s = "",mes = ""; double Freq = 0;
            for (int i = 0; i < C.Length; i++)
            {
                for (int j = 0; j < C.Length; j++)
                {
                    if (C[i] == C[j]) Count++;
                }
                if (!s.Contains(C[i]))
                {
                    Freq = (double)100 / C.Length;
                    mes += "\nСимвол " + C[i] + " встречается " + Count + " раз. Частота " + Freq * Count + "%.";
                    s += C[i];
                }
                Count = 0;
            }
            MessageBox.Show(mes, "Частота.");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            button10_Click(0, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            outputFile = "";
            label5.Text = "Файл не выбран!";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "") { MessageBox.Show("Введите текст!", "Ошибка!"); return; }
            K = Convert.ToInt32(textBox2.Text);
            if ((Gcd(K, alfavit2.Length, out x, out y) != 1) || (x < 0)) { MessageBox.Show("Не подходящее смещение!"); return;}
            if (button6.Text == "Шифровать")
            {
                P = (richTextBox1.Text).ToLower();
                C = "";

                for (int i = 0; i < richTextBox1.Text.Length; i++)
                {
                    for (int j = 0; j < alfavit2.Length; j++)
                    {
                        if (P[i] == alfavit2[j])
                        {
                            C = C + alfavit2[(j * K) % alfavit2.Length];
                            break;
                        }
                    }
                }
                richTextBox2.Text = C;
                if (outputFile != "")
                {
                    using (FileStream fstream = new FileStream(outputFile, FileMode.OpenOrCreate))
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes(C);
                        fstream.Write(array, 0, array.Length);
                    }
                }
            }
            else
            {
                    P = "";
                    C = richTextBox1.Text;

                    for (int i = 0; i < richTextBox1.Text.Length; i++)
                    {
                        for (int j = 0; j < alfavit2.Length; j++)
                        {
                            if (C[i] == alfavit2[j])
                            {
                                P = P + alfavit2[(j * x) % alfavit2.Length];
                                break;
                            }
                        }
                    }
                    richTextBox2.Text = P;
                    if (inputFile != "")
                    {
                        using (FileStream fstream = new FileStream(inputFile, FileMode.OpenOrCreate))
                        {
                            byte[] array = System.Text.Encoding.Default.GetBytes(P);
                            fstream.Write(array, 0, array.Length);
                        }
                    }
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked) radioButton2.Checked = true;
            else radioButton1.Checked = true;
            richTextBox1.Text = richTextBox2.Text;
            richTextBox2.Text = "";
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "") { MessageBox.Show("Введите текст!","Ошибка!"); return; }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                    dataGridView1.Rows[i].Cells[j].Value = "";
            }

            Key = textBox3.Text; NewP = ""; NewMatr = "";
            int Mi = 0, Mj = 0, KeyLenth = 0, Ni = 0; ;
            while (KeyLenth != Key.Length)
            {
                if (Mj == 6)
                {
                    Mi++; Mj = 0;
                }
                if (!NewMatr.Contains(Key[KeyLenth].ToString()))
                {
                    matr[Mi, Mj] = Key[KeyLenth].ToString();
                    NewMatr = NewMatr + Key[KeyLenth].ToString();
                    Mj++;
                }
                KeyLenth++;
            }

            while (Ni != alfavit.Length)
            {
                if (Mj == 6)
                {
                    Mi++; Mj = 0;
                }
                if (!NewMatr.Contains(alfavit[Ni].ToString()))
                {
                    matr[Mi, Mj] = alfavit[Ni].ToString();
                    NewMatr = NewMatr + matr[Mi, Mj];
                    Mj++;
                }
                Ni++;
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                    dataGridView1.Rows[i].Cells[j].Value = matr[i, j];
            }

            if (button7.Text == "Шифровать")
            {
                P = (richTextBox1.Text).ToLower();
                C = "";

                for(int i = 0; i < P.Length; i=i+2)
                {
                    if(i + 1 < P.Length)
                        if (P[i] == P[i + 1]) NewP = NewP + P[i] + "я" + P[i + 1];
                        else NewP = NewP + P[i] + P[i + 1];
                }
                if (NewP.Length % 2 != 0) NewP += "я";

                string a = "", b = ""; int Ai = 0, Aj = 0, Bi = 0, Bj = 0;
                for(int k = 0; k < NewP.Length / 2; k++)
                {
                    a = NewP[k * 2].ToString(); b = NewP[k * 2 + 1].ToString();
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if(matr[i,j] == a) { Ai = i;Aj = j;}
                            else
                            if(matr[i,j] == b) { Bi = i; Bj = j;}
                        }
                    }


                    if (Aj == Bj)
                    {
                        if (Ai == 5) Ai = 0;
                        if (Bi == 5) Bi = 0;
                        C += matr[Ai + 1, Aj] + matr[Bi + 1, Bj];
                    }
                    else
               if (Ai == Bi)
                    {
                        if (Aj == 5) Aj = 0;
                        if (Bj == 5) Bj = 0;
                        C += matr[Ai, Aj + 1] + matr[Ai, Bj + 1];
                    }
                    else
                    {
                        C += matr[Ai, Bj] + matr[Bi, Aj];
                    }

                }
                richTextBox2.Text = C;
            }
            else
            {
                P = "";
                C = richTextBox1.Text;

                string a = "", b = ""; int Ai = 0, Aj = 0, Bi = 0, Bj = 0;
                for (int k = 0; k < C.Length / 2; k++)
                {
                   
                    a = C[k * 2].ToString(); b = C[k * 2 + 1].ToString();
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (matr[i, j] == a) { Ai = i; Aj = j; }
                            else
                            if (matr[i, j] == b) { Bi = i; Bj = j; }
                        }
                    }

                    if (Aj == Bj)
                    {
                        if (Ai == 0) Ai = 5;
                        if (Bi == 0) Bi = 5;

                        P += matr[Ai - 1, Aj] + matr[Bi - 1, Bj];
                    }
                    else
               if (Ai == Bi)
                    {
                        if (Aj == 0) Aj = 5;
                        if (Bj == 0) Bj = 5;
                        P += matr[Ai, Aj - 1] + matr[Ai, Bj - 1];
                    }
                    else
                    {
                        P += matr[Ai, Bj] + matr[Bi, Aj];
                    }

                }
                richTextBox2.Text = P;

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string filename = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            filename = openFileDialog1.FileName;
            if (filename != "")
            {
                 inputFile = filename;
                 label4.Text = inputFile;

                using (FileStream fstream = File.OpenRead(inputFile))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    richTextBox1.Text = System.Text.Encoding.Default.GetString(array);
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string filename = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            filename = openFileDialog1.FileName;
            if (filename != "")
            {
                outputFile = filename;
                label5.Text = outputFile;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "") { MessageBox.Show("Введите текст!", "Ошибка!"); return; }
            if (button1.Text == "Шифровать")
            {
                K = Convert.ToInt32(textBox1.Text);
                P = richTextBox1.Text;
                C = "";

                for (int i = 0; i < richTextBox1.Text.Length; i++)
                {
                    for (int j = 0; j < alfavit3.Length; j++)
                    {
                        if (P[i] == alfavit3[j])
                        {
                            if(K > 0)
                            C = C + alfavit3[(j + K) % alfavit3.Length];
                            else C = C + alfavit3[(j + K + alfavit3.Length) % alfavit3.Length];
                            break;
                        }
                    }
                }
                richTextBox2.Text = C;


                if (outputFile != "")
                {
                    using (FileStream fstream = new FileStream(outputFile, FileMode.OpenOrCreate))
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes(C);
                        fstream.Write(array, 0, array.Length);
                    }
                }
            }
            else
            {
                K = Convert.ToInt32(textBox1.Text);
                P = "";
                C = richTextBox1.Text;

                for (int i = 0; i < richTextBox1.Text.Length; i++)
                {
                    for (int j = 0; j < alfavit3.Length; j++)
                    {
                        if (C[i] == alfavit3[j])
                        {
                            P = P + alfavit3[(j - K + alfavit3.Length) % alfavit3.Length];
                            break;
                        }
                    }
                }
                richTextBox2.Text = P;
                if (inputFile != "")
                {
                    using (FileStream fstream = new FileStream(inputFile, FileMode.OpenOrCreate))
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes(P);
                        fstream.Write(array, 0, array.Length);
                    }
                }
            }
        }
    }
}