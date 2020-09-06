using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Nom1
{
    public class Class1
    {

        static void Main(string [] args)
        {
            Shell s = new Shell();
            s.Run();

        }
    }
    public class Shell
    {
        private Dictionary<string, string> Aliases = new Dictionary<string, string>();
        public void Run() 
        {
            string input = string.Empty;
            do
            {
                Console.Write("Nom>");
                input = Console.ReadLine();
                Execute(input);
            } while (input != "exit");
        }
        public int Execute(string input)
        {
            if (Aliases.Keys.Contains(input))
            {
                System.Diagnostics.Process process = new Process();
                process.StartInfo = new ProcessStartInfo(Aliases[input])
                {
                    UseShellExecute = false
                };

                process.Start();
                process.WaitForExit();

                return 0;
            }

            Console.WriteLine($"{input} not found");
            return 1;
        }
    }
}
