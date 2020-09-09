/***************************************************
 *  NAME:       PAIKTRA NOM
 *  CLASS:      CSC 456
 *  ASSIGNMENT: 1
 *  DUE DATE:   9/11/2020
 *  PROFESSOR:  PROFESSOR WON
 *  OBJECTIVE:  IMPLEMENT A SIMPLE LINUX SHELL
***************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Hosting;

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
    public class Shell  //Shell Class
    {

        private List<string> args = new List<string>(); //Arguments
        private string command = string.Empty;          //Commmand
        

        /**********************
         * FUNCTION NAME: Parse
         * INPUT: string input
         * OUTPUT: void
         * DESCRIPTION: Parse command line input
         **********************/
        public void Parse(string input) //Get arguments
        {
            args.Clear();
            char[] delimiterChars = { ' ', '\t' };
            string[] words = input.Split(delimiterChars);
            command = words[0];
            for(int i = 1; i < words.Length; i++)
            {
                args.Add(words[i]);
            }
        }

        /**********************
         * FUNCTION NAME: Run
         * INPUT: None
         * OUTPUT: void
         * DESCRIPTION: Get command from user 
         **********************/
        public void Run() 
        {
            string input;
            do
            {
                Console.Write("Nom>");
                input = Console.ReadLine();
                Parse(input);
                Execute(command);
            } while (input != "exit");
        }
        public void Execute(string input)
        {
            string argumentString = "-c " + "'" + input;
            
            if(args.Count > 0)
            {
                foreach(string arguments in args)
                {
                    argumentString += " " + arguments;
                }
                argumentString += "'";
            }
            else
            {
                argumentString += "'";
            }
            ProcessStartInfo procStartInfo = new ProcessStartInfo("bash.exe", argumentString);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.RedirectStandardError = true;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            String result = proc.StandardOutput.ReadToEnd();
            if (result == "")
                result = proc.StandardError.ReadToEnd();
            Console.Write(result);
        }
    }
}
