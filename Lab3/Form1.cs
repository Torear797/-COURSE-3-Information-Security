using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        string  OriginalPath = "", codingPath = "E:\\Programs\\YandexDisk\\Projects C#\\IB_Labs\\Шифр.txt", codingPath2 = "E:\\Programs\\YandexDisk\\Projects C#\\IB_Labs\\исх.txt";
        string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯ 1234567890";
        int Nk = 4, Nr = 10, Nb = 4;
        
        byte[] SboxE = new byte[]
{
            0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76,0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0,0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15,0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75,
            0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84,0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF,0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8,0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2,
            0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73,0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB,0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79,0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08,
            0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A,0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E,0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF,0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16
};
        byte[] InvSbox = new byte[]{
        0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
        0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
        0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
        0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
        0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
        0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
        0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
        0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
        0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
        0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
        0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
        0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
        0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
        0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
        0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
        0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d
};
        byte[,] Rcon2 = new byte[4, 10]
{
            {0x01, 0x02,  0x04,  0x08,  0x10,  0x20,  0x40,  0x80,  0x1B, 0x3B},
            {0x00, 0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00, 0x00},
            {0x00, 0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00, 0x00},
            {0x00, 0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00, 0x00}
};
        byte[] Rcon = new byte[40]
{
            0x01, 0x02,  0x04,  0x08,  0x10,  0x20,  0x40,  0x80,  0x1B, 0x3B,
            0x00, 0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00, 0x00,
            0x00, 0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00, 0x00,
            0x00, 0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00,  0x00, 0x00 
};
        byte[,] GF = new byte[4, 4]
{
            {0x02, 0x03,  0x01,  0x01},
            {0x01, 0x02,  0x03,  0x01},
            {0x01, 0x01,  0x02,  0x03},
            {0x03, 0x01,  0x01,  0x02}
};
        public string GetFileName(string FilePath)
        {
            string Filename = "";
            for (int i = FilePath.Length - 1; i > 0; i--)
            {
                if (FilePath[i] != '\\') Filename = FilePath[i] + Filename;
                else break;
            }
            return Filename;
        } // Получение имени файла
        private void button4_Click(object sender, EventArgs e)
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
        private void button7_Click(object sender, EventArgs e)
        {
            if(richTextBox2.Text[richTextBox2.Text.Length-1] == ' ') 
            richTextBox1.Text = richTextBox2.Text.Substring(0, richTextBox2.Text.Length-1);
            else
            richTextBox1.Text = richTextBox2.Text;
            richTextBox2.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            string Text = richTextBox1.Text, Key = RSA_Decode(textBox5.Text), inputText = "";
            while (Text.Length % 16 != 0) Text += "$";
            int Block = Text.Length / 16;
            while (Key.Length % Convert.ToInt16(comboBox1.Text) != 0) Key += "$";   
            byte[] SecretKey = System.Text.Encoding.Default.GetBytes(Key);

            byte[] KeySchedule = new byte[240];

            KeyExpansion(SecretKey, KeySchedule);

            inputText = "";

            for (int k = 0; k < Block; k++)
            {
             byte[] bytestring = System.Text.Encoding.Default.GetBytes(Text.Substring(k * 16, 16));
             byte[,] State = new byte[4, Nb];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        State[i, j] = bytestring[i * 4 + j];
                    }
                }    
                AddRoundKey(State, KeySchedule, 0);
                for (int i = 1; i < Nr; i++)
                {
                    SubBytes(State);
                    ShiftRows(State);
                    MixColumns(State);
                    AddRoundKey(State, KeySchedule, i);              
                }
                SubBytes(State);
                ShiftRows(State);
                AddRoundKey(State, KeySchedule, Nr);
                byte[] OutputText = new byte[16];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        inputText += State[i, j].ToString() + " ";
                       
                    }
                }
          
            }
            richTextBox2.Text += inputText;
  
            using (FileStream fstream = new FileStream("CryptText.txt", FileMode.OpenOrCreate))
             {
                    byte[] array = System.Text.Encoding.Default.GetBytes(richTextBox2.Text);
                    fstream.Write(array, 0, array.Length);
             }
            using (FileStream fstream = new FileStream("DeCryptText.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(richTextBox1.Text);
                fstream.Write(array, 0, array.Length);
            }
        }
        public void AddRoundKey(byte[,] State, byte[] KeySchedule , int round)
        {
            int i, j;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < Nb; j++)
                {
                    State[i, j] ^= KeySchedule[round * Nb * 4 + i * Nb + j];
                }
            }
        }
        public void KeyExpansion(byte[] SecretKey , byte[] KeySchedule)
        {
            int i, j;
            byte k;
            byte[] temp = new byte[4];

            for (i = 0; i < Nk; i++)
            {
                KeySchedule[i * 4] = SecretKey[i * 4];
                KeySchedule[i * 4 + 1] = SecretKey[i * 4 + 1];
                KeySchedule[i * 4 + 2] = SecretKey[i * 4 + 2];
                KeySchedule[i * 4 + 3] = SecretKey[i * 4 + 3];
            }

            while (i < (Nb * (Nr + 1)))
            {
                for (j = 0; j < 4; j++)
                {
                    temp[j] = KeySchedule[(i - 1) * 4 + j];
                }

                if (i % Nk == 0)
                {
                    {
                        k = temp[0];
                        temp[0] = temp[1];
                        temp[1] = temp[2];
                        temp[2] = temp[3];
                        temp[3] = k;
                    }
                    {
                        temp[0] = SboxE[temp[0]];
                        temp[1] = SboxE[temp[1]];
                        temp[2] = SboxE[temp[2]];
                        temp[3] = SboxE[temp[3]];
                    }
                    temp[0] = (byte)(temp[0] ^ Rcon[i / Nk]);
                }
                else if (Nk > 6 && i % Nk == 4)
                {
                    {
                        temp[0] = SboxE[temp[0]];
                        temp[1] = SboxE[temp[1]];
                        temp[2] = SboxE[temp[2]];
                        temp[3] = SboxE[temp[3]];
                    }
                }

                KeySchedule[i * 4 + 0] = (byte)(KeySchedule[(i - Nk) * 4 + 0] ^ temp[0]);
                KeySchedule[i * 4 + 1] = (byte)(KeySchedule[(i - Nk) * 4 + 1] ^ temp[1]);
                KeySchedule[i * 4 + 2] = (byte)(KeySchedule[(i - Nk) * 4 + 2] ^ temp[2]);
                KeySchedule[i * 4 + 3] = (byte)(KeySchedule[(i - Nk) * 4 + 3] ^ temp[3]);
                i++;
            }
        }
        public void SubBytes(byte[,] State)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    State[i, j] = SboxE[State[i, j]];
                }
            }
        }
        public void InvSubBytes(byte[,] State)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    State[i, j] = InvSbox[State[i, j]];
                }
            }
        }
        public void MixColumns(byte[,] State)
        {
            int i;
            byte a, b, c, d;
            for (i = 0; i < 4; i++)
            {
                a = State[0, i];
                b = State[1, i];
                c = State[2, i];
                d = State[3, i];
                State[0, i] = (byte)(MultiPly(a, 0x02) ^ MultiPly(b, 0x03) ^ MultiPly(c, 0x01) ^ MultiPly(d, 0x01));
                State[1, i] = (byte)(MultiPly(a, 0x01) ^ MultiPly(b, 0x02) ^ MultiPly(c, 0x03) ^ MultiPly(d, 0x01));
                State[2, i] = (byte)(MultiPly(a, 0x01) ^ MultiPly(b, 0x01) ^ MultiPly(c, 0x02) ^ MultiPly(d, 0x03));
                State[3, i] = (byte)(MultiPly(a, 0x03) ^ MultiPly(b, 0x01) ^ MultiPly(c, 0x01) ^ MultiPly(d, 0x02));
            }
        }
        public void InvMixColumns(byte[,] State)
        {
            int i;
            byte a, b, c, d;
            for (i = 0; i < 4; i++)
            {
                a = State[0,i];
                b = State[1,i];
                c = State[2,i];
                d = State[3,i];
                State[0,i] = (byte)(MultiPly(a, 0x0e) ^ MultiPly(b, 0x0b) ^ MultiPly(c, 0x0d) ^ MultiPly(d, 0x09));
                State[1,i] = (byte)(MultiPly(a, 0x09) ^ MultiPly(b, 0x0e) ^ MultiPly(c, 0x0b) ^ MultiPly(d, 0x0d));
                State[2,i] = (byte)(MultiPly(a, 0x0d) ^ MultiPly(b, 0x09) ^ MultiPly(c, 0x0e) ^ MultiPly(d, 0x0b));
                State[3,i] = (byte)(MultiPly(a, 0x0b) ^ MultiPly(b, 0x0d) ^ MultiPly(c, 0x09) ^ MultiPly(d, 0x0e));
            }
        }
        public void ShiftRows(byte[,] State)
        {
            for (int j = 1; j < State.GetLength(0); j++)
            {
                byte[] Temp = new byte[State.GetLength(1)];
                for (int i = 0; i < State.GetLength(1); i++)
                {
                    Temp[i] = State[j, i];
                }
                for (int i = 0; i < State.GetLength(1); i++)
                {
                    int offset = (i - j) % Nb;
                    if (offset < 0) offset += Nb;
                    State[j, offset] = Temp[i];
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            string Text = richTextBox1.Text, Key = RSA_Decode(textBox5.Text);
            while (Key.Length % Convert.ToInt16(comboBox1.Text) != 0) Key += "$";
            
            byte[] KeySchedule = new byte[240];
            byte[] SecretKey = System.Text.Encoding.Default.GetBytes(Key);
            KeyExpansion(SecretKey, KeySchedule);

            string txt = richTextBox1.Text;
            string[] TempBytes = txt.Split(' ');

            byte[] TempBytes2 = new byte[TempBytes.Length];
            int Block = TempBytes.Length / 16;
            int z = 0;
            foreach (string str in TempBytes)
            {
                TempBytes2[z] = Convert.ToByte(str);
                z++;
            }
            z = 0;
            string text = "";
            for (int k = 0; k < Block; k++)
            {
                byte[,] State = new byte[4, Nb];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        State[i, j] = TempBytes2[z];
                        ++z;
                    }
                }
                AddRoundKey(State, KeySchedule, Nr);
                for (int i = Nr - 1; i >= 1; i--)
                {
                    InvShiftRows(State);
                    InvSubBytes(State);
                    AddRoundKey(State, KeySchedule, i);
                    InvMixColumns(State);
                }
                InvSubBytes(State);
                InvShiftRows(State);
                AddRoundKey(State, KeySchedule, 0);
                byte[] OutputText = new byte[16];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        OutputText[i * 4 + j] = State[i, j];
                    }
                }
                text += getString(Encoding.Default.GetString(OutputText));
            }
            richTextBox2.Text = text;
            using (FileStream fstream = new FileStream("CryptText.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(richTextBox2.Text);
                fstream.Write(array, 0, array.Length);
            }
            using (FileStream fstream = new FileStream("DeCryptText.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(richTextBox1.Text);
                fstream.Write(array, 0, array.Length);
            }
        }
        private int MultiPly(int a, int b)
        {
            int p = 0;
            int counter;
            int hi_bit_set;
            for (counter = 0; counter < 8; counter++)
            {
                if ((b & 1) == 1)
                    p ^= a;
                hi_bit_set = (a & 0x80);
                a <<= 1;
                if (hi_bit_set == 0x80)
                    a ^= 0x11b; /* x^8 + x^4 + x^3 + x + 1 */
                b >>= 1;
            }
            return p;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string ASEKEY = "", RSAKEY = "";
            ASEKEY = GenRandomString(alphabet, 5);
            RSAKEY = RSA_Encode(ASEKEY);
            using (FileStream fstream = new FileStream("RSAKEY.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(RSAKEY);
                fstream.Write(array, 0, array.Length);
            }
            textBox1.Text = ASEKEY;
            textBox5.Text = RSAKEY;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "16")
            {
                Nk = 4;
                Nr = 10;
            }
            else if (comboBox1.Text == "24")
            {
                Nk = 6;
                Nr = 12;
            }
            else if (comboBox1.Text == "32")
            {
                Nk = 8;
                Nr = 14;
            }
        }
        public string getString(string text)
        {
            string newString = ""; int remember = 0;
            for(int i = text.Length-1; i > 0; i--)
            {
                if (text[i].ToString() != "$") { remember = i; break; }
            }
            for (int i = 0; i <= remember; i++)
            {
                newString += text[i];
            }
            return newString;
        }
      public void InvShiftRows(byte[,] state)
        {
            byte temp;
            temp = state[1,3];
            state[1,3] = state[1,2];
            state[1,2] = state[1,1];
            state[1,1] = state[1,0];
            state[1,0] = temp;

            temp = state[2,0];
            state[2,0] = state[2,2];
            state[2,2] = temp;
            temp = state[2,1];
            state[2,1] = state[2,3];
            state[2,3] = temp;

            temp = state[3,0];
            state[3,0] = state[3,1];
            state[3,1] = state[3,2];
            state[3,2] = state[3,3];
            state[3,3] = temp;

        }
        string GenRandomString(string alphabet, int Length)
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder(Length - 1);
            int Position = 0;
            for (int i = 0; i < Length; i++)
            {
                Position = rnd.Next(0, alphabet.Length - 1);
                sb.Append(alphabet[Position]);
            }
            return sb.ToString();
        }
        public string RSA_Encode(string text)
        {
            string cryptText = "";
            BigInteger p = BigInteger.Parse(textBox4.Text);
            BigInteger q = BigInteger.Parse(textBox2.Text);
            BigInteger n, fi, d, E, TextKey;
            n = BigInteger.Multiply(p, q);
            fi = BigInteger.Multiply((p - 1), (q - 1));
            E = GetE(fi);
            d = Getd(fi, E, n);
            for (int i = 0; i < text.Length; i++)
            {
                if (alphabet.Contains(text[i]))
                {
                    TextKey = BigInteger.ModPow(alphabet.IndexOf(text[i]), E, n);
                    cryptText += TextKey.ToString() + " ";
                }
            }
            textBox7.Text = d.ToString();
            textBox6.Text = n.ToString();
            return cryptText;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            while (true)
            {
                BigInteger bigIntegerP = PrimeNumberGeneration();
                System.Threading.Thread.Sleep(20);
                BigInteger bigIntegerQ = PrimeNumberGeneration();
                if (bigIntegerP != 0 && bigIntegerQ != 0)
                {
                    textBox4.Text = bigIntegerP.ToString();
                    textBox2.Text = bigIntegerQ.ToString();
                    break;
                }
            }
        }

        public string RSA_Decode(string text)
        {
            BigInteger d = BigInteger.Parse(textBox7.Text);
            BigInteger n = BigInteger.Parse(textBox6.Text);
            BigInteger TextKey;
            string cryptText = "";
            string[] splitwords = text.Split(' ');
            foreach (string words in splitwords)
            {
                if (words != "")
                {
                    TextKey = BigInteger.ModPow(BigInteger.Parse(words), d, n); //Вычисляем
                    cryptText += alphabet[(int)TextKey];
                }
            }
            return cryptText;
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
        private BigInteger GetE(BigInteger n) //Получаем значение е
        {
            BigInteger e = 2, x, y;
            while (true)
            {
                if (e == n) break;
                if (Gcd(n, e, out x, out y) == 1)
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
        public BigInteger PrimeNumberGeneration()
        {
            BigInteger bigInteger = getRandomBigNumber(Convert.ToInt32(textBox3.Text));
            if (!MillerRabinTest(bigInteger, 25))
            {
                return 0;
            }
            return bigInteger;
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
    }
}