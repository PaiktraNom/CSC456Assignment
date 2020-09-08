using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nom1
{
    public class Class1 //Driver Class
    {
        static void Main(string [] args)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());

            Shell s = new Shell();
            s.Run();
        }
    }
    public class Shell
    {
        private Dictionary<string, string> Aliases = new Dictionary<string, string>()
        {
            {"ls", @"ListDirectories.exe" }
        };

        private List<string> args = new List<string>();
        private string command = string.Empty;
        public void Parse(string input)
        {
            char[] delimiterChars = { ' ', '\t' };
            string[] words = input.Split(delimiterChars);
            command = words[0];
            for(int i = 1; i < words.Length; i++)
            {
                args.Append(words[i]);
            }
        }
        public void Run() 
        {
            string input = string.Empty;
            do
            {
                Console.Write("Nom>");
                input = Console.ReadLine();
                Parse(input);
                Execute(input);
            } while (input != "exit");
        }
        public void Execute(string input)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("bash.exe", "-c " + input);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.RedirectStandardError = true;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            String result = proc.StandardOutput.ReadToEnd();
            Console.Write(result);
        }
    }
}
