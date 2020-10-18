using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Nom2
{
    class Nom2
    {
        private const int MILK_BUFFER_SIZE = 9;
        private const int CHEESE_BUFFER_SIZE = 4;
        private static string[] buffer_cheese;
        private static string[] buffer_milk;
        private static Semaphore milk_sem;
        private static Semaphore cheese_sem;
        private static Mutex milk_mut;
        private static Mutex cheese_mut;
        private int milk_count;
        private int cheese_count;

        public Nom2()
        {
            buffer_milk = new string[MILK_BUFFER_SIZE];
            Array.Clear(buffer_milk, 0, buffer_milk.Length);
            buffer_cheese = new string[CHEESE_BUFFER_SIZE];
            Array.Clear(buffer_cheese, 0, buffer_cheese.Length);

            milk_count = 0;
            cheese_count = 0;

            milk_sem = new Semaphore(1, 9);
            cheese_sem = new Semaphore(1, 4);

            milk_mut = new Mutex();
            cheese_mut = new Mutex();
        }
        public int Input()
        {
            string input = string.Empty;
            do
            {
                Console.Write("How many burgers do you want?");
                input = Console.ReadLine();
                return Int32.Parse(input);
            } while (Int32.Parse(input) >= 0);

        }

        public static void Milk_Production(Nom2 obj, string value, int burgers)
        {
            for(int i = 0; i < burgers*2; i++)
            {
                Console.WriteLine("{0} is requesting the mutex",
                          Thread.CurrentThread.Name);

                milk_mut.WaitOne();

                Console.WriteLine("{0} has entered the protected area",
                             Thread.CurrentThread.Name);

                if (obj.milk_count < MILK_BUFFER_SIZE)
                {
                    obj.milk_count++;
                    buffer_milk[obj.milk_count] = value;
                }


                Console.WriteLine("{0} is leaving the protected area",
                    Thread.CurrentThread.Name);

                milk_mut.ReleaseMutex();

                Console.WriteLine("{0} has released the mutex",
                    Thread.CurrentThread.Name);
            }
        }
        public static void Milk_Consumption(Nom2 obj, string value, int burgers)
        {
            for(int k = 0, i < burgers; i++)
            {
                cheese_mut.WaitOne();
                milk_mut.WaitOne();
                string cheeseID = string.Empty;
                if (obj.milk_count >= 3)
                {
                    for (int i = obj.milk_count - 3; i < obj.milk_count; i++)
                    {
                        cheeseID += buffer_milk[i];
                    }
                    obj.milk_count -= 3;
                    cheeseID += value;
                    buffer_cheese[obj.cheese_count] = cheeseID;
                }
                cheese_mut.ReleaseMutex();
                milk_mut.ReleaseMutex();
            }
        }

        static void Main(string[] args)
        {
            int burgers;
            Nom2 n = new Nom2();
            burgers = n.Input();

            Thread milk1 = new Thread(() => Milk_Production(n, "1"));
            milk1.Name = "milk1";
            //Thread milk2 = new Thread();
            //Thread milk3 = new Thread();

            Thread cheese1 = new Thread(() => Milk_Consumption(n, "4"));
            cheese1.Name = "cheese1";
            //Thread cheese2 = new Thread();

            //Thread burger1 = new Thread();

            milk1.Start();
            cheese1.Start();

            Console.ReadKey();

        }
    }
}
