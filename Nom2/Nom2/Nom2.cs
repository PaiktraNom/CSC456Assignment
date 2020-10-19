/******************************************************
 * NAME         :   PAIKTRA NOM
 * CLASS        :   CSC 456
 * ASSIGNMENT   :   2
 * DUE DATE     :   10/21/2020
 * PROFESSOR    :   PROFESSOR WON
 * OBJECTIVE    :   IMPLEMENT A THREADED BURGER ID SYSTEM USING SEMAPHORES
 *                  TO PRODUCE SYNCHRONIZATION
 * ****************************************************/

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
        private static Semaphore milk_full;
        private static Semaphore milk_empty;
        private static Semaphore cheese_full;
        private static Semaphore cheese_empty;
        private static Mutex milk_mut;
        private static Mutex cheese_mut;
        private int milk_count;
        private int cheese_count;

        /**********************
         * FUNCTION NAME: Nom2 
         * INPUT: None
         * OUTPUT: None
         * DESCRIPTION: Constructor for Nom2 
         **********************/
        public Nom2()
        {
            buffer_milk = new string[MILK_BUFFER_SIZE] {"0", "0", "0", "0", "0", "0", "0", "0", "0" };
            Array.Clear(buffer_milk, 0, buffer_milk.Length);
            buffer_cheese = new string[CHEESE_BUFFER_SIZE] { "0", "0", "0", "0" };
            Array.Clear(buffer_cheese, 0, buffer_cheese.Length);

            milk_count = 0;
            cheese_count = 0;

            milk_full = new Semaphore(0, 9);
            milk_empty = new Semaphore(9, 9);
            cheese_full = new Semaphore(0, 4);
            cheese_empty = new Semaphore(4, 4);

            milk_mut = new Mutex();
            cheese_mut = new Mutex();
        }
        /**********************
         * FUNCTION NAME: Input 
         * INPUT: None
         * OUTPUT: int
         * DESCRIPTION: Obtain number of burgers from user 
         **********************/
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

        /**********************
        * FUNCTION NAME: Milk_Production 
        * INPUT: Nom2, string, int
        * OUTPUT: void
        * DESCRIPTION: Producer of milk, used by milk threads 
        **********************/
        public static void Milk_Production(Nom2 obj, string value, int burgers)
        {
            for(int i = 0; i < burgers*2; i++)
            {
                milk_empty.WaitOne();
                milk_mut.WaitOne(); //Wait until it is safe to enter

                //Critical Section Start****************************
                buffer_milk[obj.milk_count] = value;        
                obj.milk_count++;
                //Critical Section End******************************

                milk_mut.ReleaseMutex();
                if(obj.milk_count % 3 == 0 && obj.milk_count > 0)
                    milk_full.Release(); //Notify procs waiting in milk_full
            }
        }

        /**********************
        * FUNCTION NAME: 0 
        * INPUT: Nom2. string, int
        * OUTPUT: void
        * DESCRIPTION:  Producer of Cheese,
        *               Consumer of milk,
        *               used by cheese threads
        **********************/
        public static void Milk_Consumption(Nom2 obj, string value, int burgers)
        {
            for (int k = 0; k < burgers; k++)
            {
                cheese_empty.WaitOne();   //Producer for cheese
                milk_full.WaitOne();    //Consumer for milk

                cheese_mut.WaitOne();   //Wait for both cheese and milk buffers to be open 
                milk_mut.WaitOne();

                string cheeseID = string.Empty;

                //Critical Section Start****************************
                for (int i = obj.milk_count - 3; i < obj.milk_count; i++)
                {
                    cheeseID += buffer_milk[i];
                    milk_empty.Release();   //Consumer for milk
                    buffer_milk[i] = "0";
                }
                obj.milk_count -= 3;
                cheeseID += value;
                buffer_cheese[obj.cheese_count] = cheeseID;
                obj.cheese_count++;
                Console.WriteLine("Created Cheese, ID: {0}", cheeseID);
                //Critical Section End******************************


                cheese_mut.ReleaseMutex();
                milk_mut.ReleaseMutex();

                if(obj.cheese_count % 2 == 0 && obj.cheese_count > 0)
                    cheese_full.Release();  //Producer for cheese
            }
        }
        /**********************
        * FUNCTION NAME: Cheese_Consumption 
        * INPUT: Nom2, int
        * OUTPUT: void
        * DESCRIPTION:  Producer of Burgers,
        *               Consumer of cheese,
        *               Prints out Burger ID,
        *               Used by burger thread
        **********************/
        public static void Cheese_Consumption(Nom2 obj, int burgers)
        {
            for(int i = 0; i < burgers; i++)
            {
                string burgerID = string.Empty;
                cheese_full.WaitOne();
                cheese_mut.WaitOne();
                //Critical Section Start****************************
                for (int j = obj.cheese_count - 2; j < obj.cheese_count; j++)
                {
                    burgerID += buffer_cheese[j];
                    buffer_cheese[j] = "0";
                    cheese_empty.Release();
                }
                obj.cheese_count -= 2;
                Console.WriteLine("Burger {0}: {1}", i+1, burgerID);
                //Critical Section End******************************
                cheese_mut.ReleaseMutex();
            }
        }

        /**********************
        * FUNCTION NAME: Main 
        * INPUT: string args[]
        * OUTPUT: void
        * DESCRIPTION:  Driver Function for program
        *               Initializes and starts threads
        **********************/
        static void Main(string[] args)
        {
            int burgers;
            Nom2 n = new Nom2();
            burgers = n.Input();

            Thread milk1 = new Thread(() => Milk_Production(n, "1", burgers));
            milk1.Name = "milk1";
            Thread milk2 = new Thread(() => Milk_Production(n, "2", burgers));
            milk2.Name = "milk2";
            Thread milk3 = new Thread(() => Milk_Production(n, "3", burgers));
            milk3.Name = "milk3";

            Thread cheese1 = new Thread(() => Milk_Consumption(n, "4", burgers));
            cheese1.Name = "cheese1";
            Thread cheese2 = new Thread(() => Milk_Consumption(n, "5", burgers));
            cheese2.Name = "cheese2";

            Thread burger1 = new Thread(() => Cheese_Consumption(n, burgers));

            milk1.Start();
            milk2.Start();
            milk3.Start();

            cheese1.Start();
            cheese2.Start();

            burger1.Start();

            milk1.Join();
            milk2.Join();
            milk3.Join();
            cheese1.Join();
            cheese2.Join();
            burger1.Join();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
