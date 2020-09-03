using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1
{
    class Class1
    {
        public void Display()
        {
            string input = string.Empty;
            do
            {
                Console.Write("Nom>");
                input = Console.ReadLine();
            } while (input != "exit");
        }

        static void Main(string [] args)
        {
            Class1 c = new Class1();
            c.Display();
        }
    }
}
