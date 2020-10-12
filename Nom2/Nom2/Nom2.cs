using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Nom2
{
    class Nom2
    {
        private const int BUFFER_SIZE = 9;
        private static int[] buffer_milk;
        private static Semaphore s;
        private int count;

        public Nom2()
        {
            buffer_milk = new int[BUFFER_SIZE];
            Array.Clear(buffer_milk, 0, buffer_milk.Length);
            count = 0;
            s = new Semaphore(1, 2);
        }
        public int Input()
        {
            string input = string.Empty;
            Console.Write("How burger ");
            input = Console.ReadLine();
            return Int32.Parse(input);
        }
        static void Main(string [] args)
        {
            int burgers;
            Nom2 n = new Nom2();
            burgers = n.Input();
        }

        public void Milk_Production()
        {
            if(count < BUFFER_SIZE)
            {
                count++;
                buffer_milk[count] = s.Release();
            }
            return;
        }
        public string Milk_Consumption()
        {
            string cheeseID = string.Empty;
            if(count >= 3)
            {
                for(int i = count - 3; i < count; i++ )
                {
                    cheeseID += buffer_milk[i];
                }
                count -= 3;
            }
            return cheeseID;
        }
    }
}
