using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string local = Directory.GetCurrentDirectory();
            Map m1 = new Map(@local + "\\map1.txt");

            TestEverything(m1, local);
            */

            
            Command command = new Command(args);

            

            string local = Directory.GetCurrentDirectory();

            string mapName = command.MapName;

            if (mapName.Remove(0, command.MapName.Length - 4)==".txt")
            {
                command.MapName = command.MapName.Remove(command.MapName.Length-4,4);
            }
            try
            {
                Map m = new Map(@local + "\\" + command.MapName + ".txt");


                Console.WriteLine("Filename: " + command.MapName);

                switch (command.SearchMethod)
                {
                    case "DFS":
                        Console.WriteLine("Depth First Search");
                        m.AI_DepthSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        foreach (Cell c in m.Path)
                        {
                            Console.WriteLine("x: " + c.X + " | y: " + c.Y);
                        }
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "BFS":
                        Console.WriteLine("Breadth First Search");
                        m.AI_BreadthSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        foreach (Cell c in m.Path)
                        {
                            Console.WriteLine("x: " + c.X + " | y: " + c.Y);
                        }
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "GBFS":
                        Console.WriteLine("Greedy Best First Search");
                        m.AI_GreedyFirstSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        foreach (Cell c in m.Path)
                        {
                            Console.WriteLine("x: " + c.X + " | y: " + c.Y);
                        }
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "AS":
                        Console.WriteLine("A* Search");
                        m.AI_AStarSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        foreach (Cell c in m.Path)
                        {
                            Console.WriteLine("x: " + c.X + " | y: " + c.Y);
                        }
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "CUS1": //uninformed
                        Console.WriteLine("");

                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        foreach (Cell c in m.Path)
                        {
                            Console.WriteLine("x: " + c.X + " | y: " + c.Y);
                        }
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "CUS2": //informed
                        Console.WriteLine("");

                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        foreach (Cell c in m.Path)
                        {
                            Console.WriteLine("x: " + c.X + " | y: " + c.Y);
                        }
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    default:
                        Console.WriteLine("Invalid search method input!\nPlease input:");
                        Console.WriteLine("DFS, BFS, GBFS, AS, CUS1, CUS2");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void TestEverything(Map m, string path)
        {
            StreamWriter toFile = new StreamWriter(@path + "\\infoDump.txt",false);
            
            

            Stack<Cell> testPath = new Stack<Cell>();
            Direction[] dirTest = new Direction[4] { Direction.Up, Direction.Down, Direction.Right, Direction.Left };

            List<Direction> pGen = new List<Direction>();
            

            Direction one, two, three;

            pGen.Add(Direction.Up);
            pGen.Add(Direction.Down);
            pGen.Add(Direction.Right);
            pGen.Add(Direction.Left);

            List<Direction[]> allPermutations = new List<Direction[]>();
            while (allPermutations.Count < 24)
            {
                for (int i = 0; i < 4; i++)
                {

                    one = pGen.ElementAt(i);
                    pGen.RemoveAt(i);
                    for (int j = 0; j < 3; j++)
                    {
                        two = pGen.ElementAt(j);
                        pGen.RemoveAt(j);
                        for (int k = 0; k < 2; k++)
                        {
                            three = pGen.ElementAt(k);
                            pGen.RemoveAt(k);
                            allPermutations.Add(new Direction[] { one, two, three, pGen[0] });
                            pGen.Insert(k, three);
                        }
                        pGen.Insert(j, two);
                    }
                    pGen.Insert(i, one);

                }
            }
            toFile.WriteLine("\n");
            toFile.WriteLine("Testing Everything");
            toFile.WriteLine("Current Map");
            m.ConsoleASCIIPrintout(toFile);
            toFile.WriteLine("\n");
            toFile.WriteLine("A Star Function Map");
            m.ConsoleASCIIPrintoutAStarFunction(toFile);
            toFile.WriteLine("\n");
            toFile.WriteLine("Greedy Function Map");
            m.ConsoleASCIIPrintoutGreedyFunction(toFile);

            m.Reset();

            toFile.WriteLine("\n");
            toFile.WriteLine("Depth Search Algorithm:");
            testPath = m.AI_DepthSearch();
            m.ConsoleASCIIPrintoutFromStack(toFile);
            toFile.Write("Path Length: ");
            toFile.WriteLine(testPath.Count);
            toFile.Write("Node Expansions: ");
            toFile.WriteLine(m.NodeExpansions);
            m.Reset();

            toFile.WriteLine("\n");
            toFile.WriteLine("Breadth Search Algorithm:");
            testPath = m.AI_BreadthSearch();
            m.ConsoleASCIIPrintoutFromStack(toFile);
            toFile.Write("Path Length: ");
            toFile.WriteLine(testPath.Count);
            toFile.Write("Node Expansions: ");
            toFile.WriteLine(m.NodeExpansions);
            m.Reset();

            toFile.WriteLine("\n");
            toFile.WriteLine("A* Search Algorithm");
            testPath = m.AI_AStarSearch();
            m.ConsoleASCIIPrintoutFromStack(toFile);
            toFile.Write("Path Length: ");
            toFile.WriteLine(testPath.Count);
            toFile.Write("Node Expansions: ");
            toFile.WriteLine(m.NodeExpansions);
            m.Reset();

            toFile.WriteLine("\n");
            toFile.WriteLine("Greedy Search Algorithm");
            testPath = m.AI_GreedyFirstSearch();
            m.ConsoleASCIIPrintoutFromStack(toFile);
            toFile.Write("Path Length: ");
            toFile.WriteLine(testPath.Count);
            toFile.Write("Node Expansions: ");
            toFile.WriteLine(m.NodeExpansions);
            m.Reset();


            toFile.WriteLine("\n");
            toFile.WriteLine("Uninformed search data testing...");
            toFile.WriteLine("\n");
            DepthSearchAllPermutations(allPermutations, m, toFile);
            m.Reset();
            toFile.WriteLine("\n");
            BreadthSearchAllPermutations(allPermutations, m, toFile);
            m.Reset();
            toFile.Close();
        }

        static void DepthSearchAllPermutations(List<Direction[]> allDir, Map m, StreamWriter toFile)
        {
            toFile.WriteLine("Breadth Search for every preferential order of direction: ");
            Dictionary<int, int> path = new Dictionary<int, int>();
            Dictionary<int, int> node = new Dictionary<int, int>();
            int size = m.Height * m.Width; //set upper limit as every cell to ensure full coverage
            for (int i = 0; i < size; i++)
            {
                node.Add(i, 0);
                path.Add(i, 0);
            }
            foreach (Direction[] directionOrder in allDir)
            {
                m.Reset();
                path[m.AI_DepthSearch(directionOrder).Count]++;
                node[m.NodeExpansions]++;
                //function returns a stack, stack count is the amount of cells in the path, index of dictionary is the path count.
                //Value of dictionary is incremented to represent the number of iterations with the same path count
            }
            toFile.WriteLine("Path Count : Iterations");
            for (int i = 0; i < size; i++)
            {
                toFile.WriteLine(i + " : " + path[i]);
            }
            toFile.WriteLine("Node Expansions : Iterations");
            for (int i = 0; i < size; i++)
            {
                toFile.WriteLine(i + " : " + node[i]);
            }
        }


        static void BreadthSearchAllPermutations(List<Direction[]> allDir, Map m, StreamWriter toFile)
        {
            toFile.WriteLine("Breadth Search for every preferential order of direction: ");
            Dictionary<int, int> path = new Dictionary<int, int>();
            Dictionary<int, int> node = new Dictionary<int, int>();
            int size = m.Height * m.Width; //set upper limit as every cell to ensure full coverage
            for (int i = 0; i < size; i++)
            {
                node.Add(i, 0);
                path.Add(i, 0);
            }
            foreach (Direction[] directionOrder in allDir)
            {
                m.Reset();
                path[m.AI_BreadthSearch(directionOrder).Count]++;
                node[m.NodeExpansions]++;
                //function returns a stack, stack count is the amount of cells in the path, index of dictionary is the path count.
                //Value of dictionary is incremented to represent the number of iterations with the same path count
            }
            toFile.WriteLine("Path Count : Iterations");
            for (int i = 0; i < size; i++)
            {
                toFile.WriteLine(i + " : " + path[i]);
            }
            toFile.WriteLine("Node Expansions : Iterations");
            for(int i = 0; i < size;i++)
            {
                toFile.WriteLine(i + " : " + node[i]);
            }
        }
    }
}
