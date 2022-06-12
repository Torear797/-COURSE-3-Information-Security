using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        string alfafit = "абвгдеёжзийклмнопрстуфхцчшщъьыэюя_,.АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ",OriginalPath = "", processedPath = "", Key = "";
        int LenthKey = 0; List<Group> Masiv = new List<Group>(); Random rand = new Random();
        public string GetPermutationString(string OriginalText)
        {
            string Outtext = "";

            for (int i = 0; i < OriginalText.Length; i = i + 2)
            {
                Outtext += OriginalText[i];
            }
            for (int i = 1; i < OriginalText.Length; i = i + 2)
            {
                Outtext += OriginalText[i];
            }
            return Outtext;
        }

        private void button1_Click(object sender, EventArgs e) //Шифровать
        {
            richTextBox2.Text = "";
            if (richTextBox1.Text != "")
            {
                if (comboBox1.Text == "Шифр перестановки без ключа")
                {
                    richTextBox2.Text = GetPermutationString(richTextBox1.Text);
                }
                else
                if (comboBox1.Text == "Шифр перестановки с ключом")
                {
                    Masiv.Clear();
                    LenthKey = Convert.ToInt32(numericUpDown1.Value);
                    Key = textBox1.Text;
                    if(richTextBox1.Text.Length % LenthKey != 0) { MessageBox.Show("Не подходящий ключ!");return; }
                    int F = Convert.ToInt32(textBox1.Text[0])-48, F2 = 1;
                    for (int i = 1; i < Key.Length; i++) F = F * (Convert.ToInt32(textBox1.Text[i]-48));
                    for (int i = 1; i <= LenthKey; i++) F2 = F2 * i;
                    if (F2 == F)
                    {
                        int id = 1;
                        for (int i = 0; i < richTextBox1.Text.Length; i++)
                        {

                            Masiv.Add(new Group() { Name = richTextBox1.Text[i].ToString(), index = id });
                            if (id == LenthKey) id = 0;
                            id++;
                        }
                        for (int i = 0; i < richTextBox1.Text.Length; i = i + LenthKey)
                        {
                            for (int k = 0; k < LenthKey; k++)
                            {
                                for (int j = 0; j < LenthKey; j++)
                                {
                                    if ((Convert.ToInt32(textBox1.Text[k] - 48) == Masiv[j + i].index))
                                    {
                                        richTextBox2.Text += Masiv[j + i].Name;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else MessageBox.Show("Не верный ключ шифрования!");
                }
                else
                if (comboBox1.Text == "Комбинированный шифр.")
                {
                    Masiv.Clear();
                    LenthKey = Convert.ToInt32(numericUpDown1.Value);
                    Key = textBox1.Text;
                    if (richTextBox1.Text.Length % LenthKey != 0) { MessageBox.Show("Не подходящий ключ!"); return; }
                    int F = Convert.ToInt32(textBox1.Text[0]) - 48, F2 = 1;
                    for (int i = 1; i < Key.Length; i++) F = F * (Convert.ToInt32(textBox1.Text[i] - 48));
                    for (int i = 1; i <= LenthKey; i++) F2 = F2 * i;
                    if (F2 == F)
                    {
                        int id = 1;
                        for (int i = 0; i < richTextBox1.Text.Length; i++)
                        {

                            Masiv.Add(new Group() { Name = richTextBox1.Text[i].ToString(), index = id });
                            if (id == LenthKey) id = 0;
                            id++;
                        }
                        for (int k = 0; k < LenthKey; k++)
                        {
                            for (int i = 0; i < richTextBox1.Text.Length; i++)
                            {

                                if ((Convert.ToInt32(textBox1.Text[k] - 48) == Masiv[i].index))
                                {
                                    richTextBox2.Text += Masiv[i].Name;
                                }

                            }
                        }

                    }
                    else MessageBox.Show("Не верный ключ шифрования!");
                }
                else
                if (comboBox1.Text == "С двойной перестановкой")
                {
                    string CryptText = GetPermutationString(richTextBox1.Text);
                    richTextBox2.Text = GetPermutationString(CryptText);
                }
                if (processedPath != "")
                {
                    using (FileStream fstream = new FileStream(processedPath, FileMode.OpenOrCreate))
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes(richTextBox2.Text);
                        fstream.Write(array, 0, array.Length);
                    }
                }
            }
            else MessageBox.Show("Введите сообщение!");
        }
        private void button2_Click(object sender, EventArgs e) //Дешифровать
        {
            richTextBox2.Text = "";
            if (richTextBox1.Text != "")
            {
                
                if (comboBox1.Text == "Шифр перестановки без ключа")
                {
                    string text = "";
                    if (richTextBox1.Text.Length % 2 != 0) LenthKey = richTextBox1.Text.Length / 2 + 1; else LenthKey = richTextBox1.Text.Length / 2;
                    for (int i = 0; i < LenthKey; i++)
                    {
                        text += richTextBox1.Text[i];
                        if(i + LenthKey < richTextBox1.Text.Length) text += richTextBox1.Text[i + LenthKey];
                    }
                    richTextBox2.Text = text;
                }
                else
                if (comboBox1.Text == "Шифр перестановки с ключом")
                {
                    Masiv.Clear();
                    Key = textBox1.Text;
                    LenthKey = Convert.ToInt32(numericUpDown1.Value);
      
                    if (richTextBox1.Text.Length % LenthKey != 0) { MessageBox.Show("Не подходящий ключ!"); return; }
                    int F = Convert.ToInt32(textBox1.Text[0]) - 48, F2 = 1;
                    for (int i = 1; i < Key.Length; i++) F = F * (Convert.ToInt32(textBox1.Text[i] - 48));
                    for (int i = 1; i <= LenthKey; i++) F2 = F2 * i;

                    if (F2 == F)
                    {
                        int id = 0;
                        for (int i = 0; i < richTextBox1.Text.Length; i++)
                        {          
                            Masiv.Add(new Group() { Name = richTextBox1.Text[i].ToString(), index = Convert.ToInt32(Key[id] - 48) });
                            if (id == LenthKey - 1) id = 0;
                            else
                            id++;
                        }
                        for (int i = 0; i < richTextBox1.Text.Length; i = i + LenthKey)
                        {
                            for (int k = 1; k <= LenthKey; k++)
                            {
                                for (int j = 0; j < LenthKey; j++)
                                {
                                    if (k == Masiv[j + i].index)
                                    {
                                        richTextBox2.Text += Masiv[j + i].Name;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else MessageBox.Show("Не верный ключ Дешифрования!");
                }
                else
                if (comboBox1.Text == "Комбинированный шифр.")
                {
                    Masiv.Clear();
                    LenthKey = Convert.ToInt32(numericUpDown1.Value);
                    Key = textBox1.Text;
                    if (richTextBox1.Text.Length % LenthKey != 0) { MessageBox.Show("Не подходящий ключ!"); return; }
                    int F = Convert.ToInt32(textBox1.Text[0]) - 48, F2 = 1;
                    for (int i = 1; i < Key.Length; i++) F = F * (Convert.ToInt32(textBox1.Text[i] - 48));
                    for (int i = 1; i <= LenthKey; i++) F2 = F2 * i;
                    if (F2 == F)
                    {
                        int id = richTextBox1.Text.Length / LenthKey ,  index = 0;

                        for (int i = 0; i < richTextBox1.Text.Length; i = i + id)
                        {
                            for (int j = 0; j < id; j++)
                            {
                                Masiv.Add(new Group() { Name = richTextBox1.Text[i + j].ToString(), index = Convert.ToInt32(textBox1.Text[index] - 48) });
                            }
                            index++;
                        }
                       

                        for (int j = 0; j < id; j++)
                        {
                            for (int k = 1; k <= LenthKey; k++)
                            {
                                for (int i = 0; i < richTextBox1.Text.Length; i++)
                                {
                                    if (k == Masiv[i].index)
                                    {
                                        richTextBox2.Text += Masiv[i + j].Name;
                                        break;
                                    }

                                }
                            }
                        }

                    }
                    else MessageBox.Show("Не верный ключ шифрования!");
                }
                else
                if (comboBox1.Text == "С двойной перестановкой")
                {
                    string text = "", text2 = "";
                    if (richTextBox1.Text.Length % 2 != 0) LenthKey = richTextBox1.Text.Length / 2 + 1; else LenthKey = richTextBox1.Text.Length / 2;

                    for (int i = 0; i < LenthKey; i++)
                    {
                        text += richTextBox1.Text[i];
                        if (i + LenthKey < richTextBox1.Text.Length) text += richTextBox1.Text[i + LenthKey];
                    }

                    for (int i = 0; i < LenthKey; i++)
                    {
                        text2 += text[i];
                        if (i + LenthKey < text.Length) text2 += text[i + LenthKey];
                    }

                    richTextBox2.Text = text2;
                }
                if (processedPath != "")
                {
                    using (FileStream fstream = new FileStream(processedPath, FileMode.OpenOrCreate))
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes(richTextBox2.Text);
                        fstream.Write(array, 0, array.Length);
                    }
                }
            }
            else MessageBox.Show("Введите сообщение!");
        }
        private void button4_Click(object sender, EventArgs e) //Выбор исходного файла
        {
            string filename = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                filename = openFileDialog1.FileName;
            if (filename != "")
            {
                OriginalPath = filename;
                label9.Text = GetFileName(OriginalPath);

                using (FileStream fstream = File.OpenRead(OriginalPath))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    richTextBox1.Text = System.Text.Encoding.Default.GetString(array);
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)//Очистить исходный файл
        {
            OriginalPath = ""; label9.Text = "Файл не выбран.";
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            label6.Text = richTextBox1.Text.Length.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string filename = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                filename = openFileDialog1.FileName;
            if (filename != "")
            {
                processedPath = filename;
                label8.Text = GetFileName(processedPath);
            }
        } //Выбор обработанного файла
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Шифр перестановки без ключа")
            {
                numericUpDown1.ReadOnly = true;
                textBox1.ReadOnly = true;
                button8.Visible = false;
            }
            else
           if (comboBox1.Text == "Шифр перестановки с ключом")
            {
                numericUpDown1.ReadOnly = false;
                textBox1.ReadOnly = false;
                button8.Visible = false;
            }
            else
           if (comboBox1.Text == "Комбинированный шифр.")
            {
                numericUpDown1.ReadOnly = false;
                textBox1.ReadOnly = false;
                button8.Visible = true;
            }
            else
           if (comboBox1.Text == "С двойной перестановкой")
            {
                numericUpDown1.ReadOnly = true;
                textBox1.ReadOnly = true;
                button8.Visible = false;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox2.Text; richTextBox2.Text = ""; 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int LenthKey = Convert.ToInt32(numericUpDown1.Value);
            int k = 0;
            textBox1.Text = "";
            for (int i = 0; i < LenthKey; i++)
            {
                do
                {
                    k = rand.Next(1, LenthKey+1);
                }
                while (textBox1.Text.Contains(k.ToString()));
                textBox1.Text += k.ToString();
            }

            DialogResult dialogResult = MessageBox.Show("Оставить ключ?", "Подбор ключа.", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                button8_Click(null, null);
            }
        }

        private void button6_Click(object sender, EventArgs e) //Очистить обработанный файл
        {
            processedPath = ""; label8.Text = "Файл не выбран.";
        }
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            label7.Text = richTextBox2.Text.Length.ToString();
        }
        public string GetFileName(string FilePath)
        {
            string Filename = "";
            for(int i = FilePath.Length-1; i > 0; i--)
            {
                if (FilePath[i] != '\\') Filename = FilePath[i] + Filename;
                else break;
            }
            return Filename;
        } // Получение имени файла
    }
}
