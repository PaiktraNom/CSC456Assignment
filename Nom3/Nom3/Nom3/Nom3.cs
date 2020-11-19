/******************************************************
 * NAME         :   PAIKTRA NOM
 * CLASS        :   CSC 456
 * ASSIGNMENT   :   3
 * DUE DATE     :   11/22/2020
 * PROFESSOR    :   PROFESSOR WON
 * OBJECTIVE    :   BUILD A PAGING-BASED MEMORY MANAGEMENT
 *                  SYSTEM
 * ****************************************************/

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
            /**********************
             * FUNC NAME:   Proc
             * INPUT:       None
             * OUTPUT:      None
             * DESCRIPTION: Constructor for Proc class
             *              Initializes attributes
             **********************/
            public Proc()
            {
                size = 0;
                pid = 0;
            }
        }
        private List<int> pageTable;        //Keeps track of values
        private List<int> frames;           //for storing frame values
        private List<int> freeFrameList;    //List of Current Free Frames
        private List<Proc> processList;     //List of processes
        private int frameTotal;             //Total number of frames
        private int frameCount;             //Number of used frames (frameSize - FrameCount = total number of free frames)


        /**********************
         * FUNC NAME:   Nom3
         * INPUT:       None
         * OUTPUT:      None
         * DESCRIPTION: Constructor for Nom3 class
         *              Initializes attributes
         **********************/
        public Nom3()
        {
            frames = new List<int>();
            processList = new List<Proc>();
            pageTable = new List<int>();
            frameTotal = 0;
            frameCount = 0;
        }

        /**********************
         * FUNC NAME:   Parse    
         * INPUT:       string, ref string, ref string, ref string
         * OUTPUT:      void
         * DESCRIPTION: Separate input string to
         *              command, parameter 1, and parameter 2
         **********************/
        public void Parse(string input, ref string command, ref string para1, ref string para2)
        {
            int i = 0;
            while (i < input.Length && input[i] != ' ')
            {
                command += input[i];
                i++;
            }
            i++;
            while (i < input.Length && input[i] != ' ')
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
        /**********************
         * FUNC NAME:   Input
         * INPUT:       string, string, string
         * OUTPUT:      void
         * DESCRIPTION: Execute functions from command
         *              and para1 and para2
         **********************/
        public void Input(string command, string para1, string para2)
        {
            int para1Int = 0;
            int para2Int = 0;
            if(freeFrameList == null)
            {
                if (command == "M" || command == "m")
                {
                    if(para1 != "" && para2 != "")
                    {
                        para1Int = Int32.Parse(para1);
                        para2Int = Int32.Parse(para2);
                        MemoryManager(para1Int, para2Int);
                        Console.WriteLine("Created Memory");
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Incorrect parameters for memory creation");
                    }
 
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
                    PrintMemory();
                }
                else if (command == "D" || command == "d")
                {
                    if(para2 == "" && para1 != "")
                    {
                        para1Int = Int32.Parse(para1);
                        if (Deallocate(para1Int) == -1)
                        {
                            Console.WriteLine("Deallocation Unsuccessful");
                        }
                        else
                        {
                            Console.WriteLine("Deallocation Successful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Incorrect parameters for Deallocate");
                    }

                    
                }
                else if(command == "R" || command == "r")
                {
                    if(para1 != "" && para2 != "")
                    {
                        para1Int = Int32.Parse(para1);
                        para2Int = Int32.Parse(para2);
                        if (Read(para1Int, para2Int) == -1)
                        {
                            Console.WriteLine("Read Unsuccessful: Frame index does not match pid");
                        }
                        else
                        {
                            Console.WriteLine("Read Successful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Incorrect parameters for Read");
                    }
                }
                else if (command == "W" || command == "w")
                {
                    if(para1 != "" && para2 != "")
                    {
                        para1Int = Int32.Parse(para1);
                        para2Int = Int32.Parse(para2);
                        if (Write(para1Int, para2Int) == -1)
                        {
                            Console.WriteLine("Write Unsuccessful: Frame index does not match pid");
                        }
                        else
                        {
                            Console.WriteLine("Write Successful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Incorrect parameters for Write");
                    }

                }
                else if (command == "A" || command == "a")
                {
                    if (para1 != "" && para2 != "")
                    {
                        para1Int = Int32.Parse(para1);
                        para2Int = Int32.Parse(para2);
                        if (Allocate(para1Int, para2Int) == -1)
                        {
                            Console.WriteLine("Allocation Unsuccessful: Frame List is full");
                        }
                        else
                        {
                            Console.WriteLine("Allocation Successful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Incorrect parameters for Allocation");
                    }
                }
                else if (command == "M" || command == "m")
                {
                    if (para1 != "" && para2 != "")
                    {
                        para1Int = Int32.Parse(para1);
                        para2Int = Int32.Parse(para2);
                        MemoryManager(para1Int, para2Int);
                        Console.WriteLine("Created Memory");
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Incorrect parameters for memory creation");
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Command not found...");
                }
            }
            
        }
        /**********************
         * FUNC NAME:   MemoryManager 
         * INPUT:       int, int
         * OUTPUT:      void
         * DESCRIPTION: Creates memory space for allocation
         **********************/
        private void MemoryManager(int memSize, int frameSize)
        {
            freeFrameList = new List<int>();
            frameTotal = memSize*frameSize;
            for(int i = 0; i < memSize*frameSize; i++)
            {
                freeFrameList.Add(i);
                pageTable.Add(0);
                frames.Add(0);
            }
        }
        /**********************
         * FUNC NAME:   Allocate
         * INPUT:       int, int
         * OUTPUT:      int
         * DESCRIPTION: allocates space for process, returns 1
         *              if allocation is successful and -1 for
         *              unsuccessful allocation
         **********************/
        private int Allocate(int allocSize, int pid)
        {
            if(frameCount + allocSize <= frameTotal)
            {
                Random r = new Random();
                Proc p = new Proc();
                p.pid = pid;
                p.size = allocSize;
                int counter = 0;
                processList.Add(p);

                while(counter < allocSize)
                {
                    int randValue = r.Next(freeFrameList.Count);
                    pageTable[freeFrameList[randValue]] = pid;
                    freeFrameList.RemoveAt(randValue);
                    frameCount++;
                    counter++;
                }
                return 1;
            }
            else
            {
                return -1;
            }

        }
        /**********************
         * FUNC NAME:   Deallocate  
         * INPUT:       int
         * OUTPUT:      int
         * DESCRIPTION: Deallocates process, returns 1
         *              if deallocation is successful and
         *              -1 if deallocation is unsuccessful
         **********************/
        private int Deallocate(int pid)
        {
            if(frameCount > 0)
            {
                Proc p = new Proc();
                bool found = false;
                for(int i = 0; i < frameTotal; i++)
                {
                    if(pageTable[i] == pid)
                    {
                        freeFrameList.Add(i);
                        pageTable[i] = 0;
                        found = true;
                        frameCount--;
                        
                    }
                }
                if(found == true)
                {
                    for(int j = 0; j < processList.Count; j++)
                    {
                        if(processList[j].pid == pid)
                        {
                            processList.RemoveAt(j);
                        }    
                    }
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
        /**********************
         * FUNC NAME:   Write  
         * INPUT:       int, int 
         * OUTPUT:      int
         * DESCRIPTION: Writes a value at pid
         *              with logical address
         **********************/
        private int Write(int pid, int logicAdd)
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
        /**********************
         * FUNC NAME:   Read   
         * INPUT:       int, int
         * OUTPUT:      int
         * DESCRIPTION: Reads a value
         *              at pid with logical
         *              address
         **********************/
        private int Read(int pid, int logicAdd)
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

        /**********************
         * FUNC NAME:   PrintMemory()  
         * INPUT:       none
         * OUTPUT:      void
         * DESCRIPTION: Outputs Memory, Current Free frames,
         *              And current active processes
         **********************/

        private void PrintMemory()
        {

            Console.Write("Frame Values:    ");
            for(int i = 0; i < frames.Count; i++)
            {
                Console.Write("{0} ", frames[i]);
            }
            Console.Write("\npid:             ");
            for (int i = 0; i < pageTable.Count; i++)
            {
                Console.Write("{0} ", pageTable[i]);
            }
            Console.Write("\nPage Numbers:    ");
            for (int i = 0; i < pageTable.Count; i++)
            {
                Console.Write("{0} ", i);
            }
            Console.WriteLine("\n");

            Console.WriteLine("freeFrameList");
            List<int> sortedList = new List<int>();
            freeFrameList.Sort();
            sortedList = freeFrameList;
            for (int i = 0; i < freeFrameList.Count; i++)
            {
                Console.Write("{0} ", sortedList[i]);
            }

            Console.WriteLine("\nprocessList");
            Console.WriteLine("Pid Size");
            foreach(Proc p in processList)
            {
                Console.WriteLine("{0}   {1}", p.pid, p.size);
            }
        }
        /**********************
         * FUNC NAME:   displayCommands    
         * INPUT:       none
         * OUTPUT:      void
         * DESCRIPTION: Give the user commands and syntax
         **********************/
        public void displayCommands()
        {
            Console.WriteLine("Welcome to a simulation of a paging based memory management system");
            Console.WriteLine("Commands:");
            Console.WriteLine("Create Memory:   M <memorySize> <frameSize>");
            Console.WriteLine("Allocate:        A <size> <pid>");
            Console.WriteLine("Write:           W <pid> <logicalAddress>");
            Console.WriteLine("Read:            R <pid> <logicalAddress>");
            Console.WriteLine("Deallocate:      D <pid>");
            Console.WriteLine("Print Memory:    P\n");

        }
        static void Main(string[] args)
        {
            Nom3 n = new Nom3();
           

            string input = string.Empty;
            n.displayCommands();
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
