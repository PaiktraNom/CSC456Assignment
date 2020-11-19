using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nom3
{
    class Nom3
    {
        class Proc
        {
            public int size;
            public int pid;

            public Proc()
            {
                size = 0;
                pid = 0;
            }
        }
        private List<int> freeFrameList;
        private List<Proc> processList;
        private int frameTotal;         //Total number of frames
        private int frameCount;         //Number of used frames (frameSize - FrameCount = total number of free frames)
        private List<int> pageTable;    //Keeps track of values
        private List<int> frames;       //for storing frame values

        public Nom3()
        {
            frames = new List<int>();
            processList = new List<Proc>();
            pageTable = new List<int>();
            frameTotal = 0;
            frameCount = 0;
        }

        public void Parse(string input, ref string command, ref string para1, ref string para2)
        {
            int i = 0;
            command = input[i].ToString();
            i++;
            i++;
            while(i < input.Length && input[i] != ' ')
            {
                para1 += input[i];
                i++;
            }
            i++;
            while(i < input.Length && input[i] != ' ') 
            {
                para2 += input[i];
                i++;
            }
        }
        public void Input(string command, string para1, string para2)
        {
            int para1Int = 0;
            int para2Int = 0;
            if(freeFrameList == null)
            {
                if (command == "M" || command == "m")
                {
                    para1Int = Int32.Parse(para1);
                    para2Int = Int32.Parse(para2);
                    MemoryManager(para1Int, para2Int);
                    Console.WriteLine("Created Memory");
                }
                else
                {
                    Console.WriteLine("ERROR: Memory not created");
                }
            }
            else
            {
                if (command == "P" || command == "p")
                {
                    printMemory();
                }
                else if (command == "D" || command == "d")
                {
                    para1Int = Int32.Parse(para1);
                    if(deallocate(para1Int) == -1)
                    {
                        Console.WriteLine("Deallocation Unsuccessful");
                    }
                    else
                    {
                        Console.WriteLine("Deallocation Successful");
                    }
                    
                }
                else if(command == "R" || command == "r")
                {
                    para1Int = Int32.Parse(para1);
                    para2Int = Int32.Parse(para2);
                    if (read(para1Int, para2Int) == -1)
                    {
                        Console.WriteLine("Read Unsuccessful");
                    }
                    else
                    {
                        Console.WriteLine("Read Successful");
                    }
                }
                else if (command == "W" || command == "w")
                {
                    para1Int = Int32.Parse(para1);
                    para2Int = Int32.Parse(para2);
                    if (write(para1Int, para2Int) == -1)
                    {
                        Console.WriteLine("Write Unsuccessful");
                    }
                    else
                    {
                        Console.WriteLine("Write Successful");
                    }
                }
                else if (command == "A" || command == "a")
                {
                    para1Int = Int32.Parse(para1);
                    para2Int = Int32.Parse(para2);
                    if(allocate(para1Int, para2Int) == -1)
                    {
                        Console.WriteLine("Allocation Unsuccessful: Frame List is full");
                    }
                    else
                    {
                        Console.WriteLine("Allocation Successful");
                    }
                }
                else if (command == "M" || command == "m")
                {
                    para1Int = Int32.Parse(para1);
                    para2Int = Int32.Parse(para2);
                    MemoryManager(para1Int, para2Int);
                }
                else
                {
                    Console.WriteLine("ERROR: Command not found...");
                }
            }
            
        }
        private void MemoryManager(int memSize, int frameSize)
        {
            freeFrameList = new List<int>();
            frameTotal = memSize;
            for(int i = 0; i < memSize; i++)
            {
                freeFrameList.Add(i);
                pageTable.Add(0);
            }
        }
        private int allocate(int allocSize, int pid)
        {
            if(frameCount + allocSize < frameTotal)
            {
                Random rnd = new Random();
                Proc p = new Proc();
                p.pid = pid;
                p.size = allocSize;
                processList.Add(p);

                for(int i = 0; i < allocSize; i++)
                {
                    int randValue = rnd.Next(freeFrameList.Count);
                    if(pageTable[randValue] == 0)
                    {
                        pageTable[randValue] = pid;
                        freeFrameList.Remove(randValue);
                    }
                }
                return 1;
            }
            else
            {
                return -1;
            }

        }
        private int deallocate(int pid)
        {
            if(frameCount > 0)
            {
                Proc p = new Proc();
                bool found = false;
                for(int i = 0; i < frameTotal; i++)
                {
                    if(pageTable[i] == pid)
                    {
                        pageTable.Add(i);
                        pageTable[i] = 0;
                        found = true;
                    }
                }
                if(found == true)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        private int write(int pid, int logicAdd)
        {
            if(pid == pageTable[logicAdd])  //Check if pid exists at logicalAddress
            {
                frames[logicAdd] = 1;
                return 1;
            }
            else
            {
                return -1;
            }
        }
        private int read(int pid, int logicAdd)
        {
            if (pid == pageTable[logicAdd])
            {
                Console.WriteLine("pid: {0} address: {1} frame value: {2}", pid, logicAdd, frames[logicAdd]);
                return 1;
            }
            else
            {
                return -1;
            }    
            
        }
        private void printMemory()
        {
            Console.WriteLine("freeFrameList");
            for (int i = 0; i < freeFrameList.Count; i++)
            {
                Console.Write("{0} ", i);
            }

            Console.WriteLine("\nprocessList");

            foreach(Proc p in processList)
            {
                Console.WriteLine("{0} {1}", p.pid, p.size);
            }
        }

        static void Main(string[] args)
        {
            Nom3 n = new Nom3();
           

            string input = string.Empty;
            do
            {
                Console.Write("Nom>> ");
                input = Console.ReadLine();

                string command = string.Empty;
                string para1 = string.Empty;
                string para2 = string.Empty;

                if(input != "exit" && input != "Exit")
                {
                    n.Parse(input, ref command, ref para1, ref para2);
                    n.Input(command, para1, para2);
                }
                
            } while (input != "exit" && input != "Exit");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
