using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_4
{
    class Mnogochlen
    {
        int[] koef; //массив коэф-ов
        int step; //значение степени полинома

        public Mnogochlen(int[] k, int s)
        {
            koef = k;
            step = s;
        }

        //сложение полиномов
        public static Mnogochlen operator +(Mnogochlen A, Mnogochlen B)
        {
            int D1 = A.step;
            int[] M1 = new int[D1 + 1];
            Mnogochlen C = new Mnogochlen(M1, D1);
            for (int i = 0; i < A.step + 1; i++)
            {
                C.koef[i] = A.koef[i] + B.koef[i];
            }
            return C;
        }


        //вычитание полиномов
        public static Mnogochlen operator -(Mnogochlen A, Mnogochlen B)
        {
            int D1 = A.step;
            int[] M1 = new int[D1 + 1];
            Mnogochlen C = new Mnogochlen(M1, D1);
            for (int i = 0; i < A.step + 1; i++)
            {
                C.koef[i] = A.koef[i] - B.koef[i];
            }
            return C;
        }


        //умножение полиномов
        public static Mnogochlen operator *(Mnogochlen A, Mnogochlen B)
        {
            int D1 = A.step;
            int[] M1 = new int[D1 + 1];
            Mnogochlen C = new Mnogochlen(M1, D1);
            for (int i = 0; i < A.step + 1; i++)
            {
                C.koef[i] = A.koef[i] * B.koef[i];
            }
            return C;
        }



        //вывод полинома
        public void show()
        {
            for (int i = 0; i < step; i++)
            {
                Console.Write("+" + koef[i] + "x^" + (step - i));
            }
            //return 0;
        }
    }
}
