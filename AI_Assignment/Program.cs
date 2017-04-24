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

        /// <summary>
        /// Main Function which runs the default program, taking in arguments passed through a batch file
        /// 
        /// Runs one algorithm per program execution and returns useful information
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            /* USE FOR TESTING TO COLLECT DATA AND DEBUG ALGORITHMS
            string local = Directory.GetCurrentDirectory();
            Map m1 = new Map(@local + "\\map3.txt");

            TestEverything(m1, local);
            */
            
            Command command = new Command(args);

            

            string local = Directory.GetCurrentDirectory();

            //take a copy of variable to use in testing condition
            string mapName = command.MapName;

            //clean up map parameter input
            if (mapName.Remove(0, command.MapName.Length - 4)!=".txt") //if string does not contain '.txt' at the end
            {
                command.MapName = command.MapName + ".txt"; //add .txt onto the end of the string
            }
            try //failsafe for opening file
            {
                Map m = new Map(@local + "\\" + command.MapName); //open up file with clean map name


                Console.WriteLine("Filename: " + command.MapName);

                switch (command.SearchMethod) //Run the input Algorithm argument
                {
                    case "DFS":
                        Console.WriteLine("Depth First Search");
                        m.AI_DepthSearch(); //algorithm runtime
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);  //info
                        Console.Write("Path Count: ");
                        Console.WriteLine(m.Path.Count);
                        m.ConsoleASCIIPrintoutFromStack(Console.Out); //print a ASCII representation of the map and path
                        break;
                    case "BFS":
                        Console.WriteLine("Breadth First Search");
                        m.AI_BreadthSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        Console.Write("Path Count: ");
                        Console.WriteLine(m.Path.Count);
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "GBFS":
                        Console.WriteLine("Greedy Best First Search");
                        m.AI_GreedyFirstSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        Console.Write("Path Count: ");
                        Console.WriteLine(m.Path.Count);
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "AS":
                        Console.WriteLine("A* Search");
                        m.AI_AStarSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        Console.Write("Path Count: ");
                        Console.WriteLine(m.Path.Count);
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "CUS1": //uninformed custom algorithm - lowest cost first search
                        Console.WriteLine("Lowest Cost First Search");
                        Console.WriteLine("Expands nodes tier by tier like BFS, but prioritises the lowest cost cells to be searched first");
                        m.AI_LowestFirstSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        Console.Write("Path Count: ");
                        Console.WriteLine(m.Path.Count);
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "LCFS": //uninformed custom algorithm - lowest cost first search
                        Console.WriteLine("Lowest Cost First Search");
                        Console.WriteLine("Expands nodes tier by tier like BFS, but prioritises the lowest cost cells to be searched first");
                        m.AI_LowestFirstSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        Console.Write("Path Count: ");
                        Console.WriteLine(m.Path.Count);
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "AAS": //informed custom algorithm - Advanced A*
                        Console.WriteLine("Advanced Iteration of A* Search");
                        Console.WriteLine("Uses the straight line hypotenuse distance for calculating both cost and the distance to goal of each node");
                        m.AI_AdvancedAStarSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        Console.Write("Path Count: ");
                        Console.WriteLine(m.Path.Count);
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    case "CUS2": //informed custom algorithm - Advanced A*
                        Console.WriteLine("Advanced Iteration of A* Search");
                        Console.WriteLine("Uses the straight line hypotenuse distance for calculating both cost and the distance to goal of each node");
                        m.AI_AdvancedAStarSearch();
                        Console.Write("Nodes expanded: ");
                        Console.WriteLine(m.NodeExpansions);
                        Console.Write("Path Count: ");
                        Console.WriteLine(m.Path.Count);
                        m.ConsoleASCIIPrintoutFromStack(Console.Out);
                        break;
                    default:
                        Console.WriteLine("Invalid search method input!\nPlease input:");
                        Console.WriteLine("DFS, BFS, GBFS, AS, CUS1 (or LCFS), CUS2 (or AAS)");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Tests All Algorithms along with special advanced iteration tests, prints to the infoDump.txt file
        /// </summary>
        /// <param name="m">Map object to test</param>
        /// <param name="path">path string for saving to infoDump.txt in the program directory</param>
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

        /// <summary>
        /// Runs the DFS algorithm through every possible preferential order of direction
        /// </summary>
        /// <param name="allDir"></param>
        /// <param name="m"></param>
        /// <param name="toFile"></param>
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
                //function returns a stack, stack count is the amount of cells in the path, index of dictionary is the path count.
                //Value of dictionary is incremented to represent the number of iterations with the same path count
                node[m.NodeExpansions]++;
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

        /// <summary>
        /// Runs the BFS algorithm through every possible preferential order of direction
        /// </summary>
        /// <param name="allDir"></param>
        /// <param name="m"></param>
        /// <param name="toFile"></param>
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
