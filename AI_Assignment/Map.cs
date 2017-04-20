using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AI_Assignment
{
    class Map
    {
        //Width and Height of entire envirenment of Cells
        private int _width;
        private int _height;
        //Cell tracking tools
        private Stack<Cell> _path;
        private List<Cell> _list;

        //Saves the number of node expansions neccessary in the last algorithm that was run
        private int _nodeExpansions;
        
        //Reference to starting and goal cell in order to easily set up environment
        private Cell _start;
        private Cell _goal;

        //Contains all the Cells in the environment
        private List<Cell> _cells;

        //public access to the main program in order to view the generated path of an algorithm
        public Stack<Cell> Path { get => _path; set => _path = value; }

        /// <summary>
        /// Saves the number of node expansions executed in the last algorithm that was run
        /// </summary>
        public int NodeExpansions { get => _nodeExpansions; set => _nodeExpansions = value; }
        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }

        //Reference to the starting Cell for the benefit of the search algorithms

        /// <summary>
        /// Constructor takes in a path to a valid map file in order to read and process that data into
        /// an appropriate testing map for various searching algorithms
        /// </summar
        public Map(string path)
        {
            //Read data from file
            
            string str;
            string[] processed;
            StreamReader fRead = new StreamReader(path);

            
            //retrieve cell details
             
            str = fRead.ReadLine();
            str = str.Trim('[', ']');
            processed = str.Split(new Char[] { ',' });

            Height = Int32.Parse(processed[0]);
            Width = Int32.Parse(processed[1]);


            //list which stores every cell for future processing and setup
            _cells = new List<Cell>();
            //generate cells with regards to map size
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _cells.Add(new Cell(i + 1, j + 1));
                }
            }


            foreach (Cell c in _cells) //link adjacent cells
            {
                c.Up = FindCell(c.X, c.Y - 1);
                c.Right = FindCell(c.X + 1, c.Y);
                c.Down = FindCell(c.X, c.Y + 1);
                c.Left = FindCell(c.X - 1, c.Y);
            }

            //initialises the stack path and list objects to be utilised in various searching algorithms
            _path = new Stack<Cell>();
            _list = new List<Cell>();

            //retrieve position data for start cell
            str = fRead.ReadLine();
            str = str.Trim('(', ')');
            processed = str.Split(new Char[] { ',' });
            _start = FindCell(Int32.Parse(processed[0]) + 1, Int32.Parse(processed[1]) + 1);
            _start.Status = CellStatus.Start;
            _start.Parent = null;

            //retrieve position data for goal cell
            str = fRead.ReadLine();
            str = str.Trim('(', ')');
            processed = str.Split(new Char[] { ',' });
            _goal = FindCell(Int32.Parse(processed[0]) + 1, Int32.Parse(processed[1]) + 1);
            _goal.Status = CellStatus.Goal;

            /// reads and processes the rest of the file into an array, each line is processed into an array
            /// representing a rectangle, which then is used to in an embedded for loop to find the appropriate
            /// cells and change the status of each to filled
            while (!fRead.EndOfStream)
            {
                str = fRead.ReadLine();
                str = str.Trim('(', ')');
                processed = str.Split(new Char[] { ',' }); // x0 y1 w2 h3
                Cell wall;
                for(int i = Int32.Parse(processed[0]); i < Int32.Parse(processed[0]) + Int32.Parse(processed[2]); i++)
                {
                    for(int j = Int32.Parse(processed[1]); j < Int32.Parse(processed[1]) + Int32.Parse(processed[3]); j++)
                    {
                        wall = FindCell(i + 1, j + 1);
                        wall.Status = CellStatus.Filled;
                    }
                }
            }

            //set cell cost based on starting cell
            foreach(Cell c in _cells)
            {
                int x = Math.Abs(c.X - _start.X);
                int y = Math.Abs(c.Y - _start.Y);
                c.Cost = x + y;
                x = Math.Abs(c.X - _goal.X);
                y = Math.Abs(c.Y - _goal.Y);
                c.DTG = x + y;
            }

        }

        

        //public Map()
        //use c# project directory & default map name

        //Function used to search for adjacent cells, only called during initialisations
        Cell FindCell(int x, int y)
        {
            foreach(Cell c in _cells)
            {
                if (x == c.X && y == c.Y)
                    return c;
            }
            return null;
        }

        



        /// <summary>
        /// Prints out the map of the AI search challenge, with start, goal and obstructions
        /// </summary>
        public void ConsoleASCIIPrintout(TextWriter writer)
        {
            Cell c;
            Cell[,] arr = new Cell[Width,Height];
            for(int i = 0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    arr[i, j] = FindCell(i + 1, j + 1);
                }
            }
            for(int j = 0; j < Height; j++)
            {
                for(int i = 0; i < Width; i++)
                {
                    c = arr[i, j];
                    switch (c.Status)
                    {
                        case CellStatus.Start:
                            writer.Write("|S|");
                            break;
                        case CellStatus.Goal:
                            writer.Write("|G|");
                            break;
                        case CellStatus.Filled:
                            writer.Write("|x|");
                            break;
                        case CellStatus.Empty:
                            writer.Write("| |");
                            break;
                    }
                }
                writer.WriteLine();
            }
            writer.WriteLine();
            writer.WriteLine();
        }


        /// <summary>
        /// Prints out the map of the AI search challenge, with start, goal, obstructions and the path of the last run algorithm
        /// </summary>
        public void ConsoleASCIIPrintoutFromStack(TextWriter writer)
        {
            foreach(Cell c in _path) //set all valid cells in the path stack to path status for the purpose of printing
            {
                if (c.Status != CellStatus.Goal && c.Status != CellStatus.Start)
                    c.Status = CellStatus.Path;
            }

            if (_path.Count != 0)
            {
                Cell c;
                Cell[,] arr = new Cell[Width, Height];
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        arr[i, j] = FindCell(i + 1, j + 1);
                    }
                }
                for (int j = 0; j < Height; j++)
                {
                    for (int i = 0; i < Width; i++)
                    {
                        c = arr[i, j];
                        switch (c.Status)
                        {
                            case CellStatus.Start:
                                writer.Write("|S|");
                                break;
                            case CellStatus.Goal:
                                writer.Write("|G|");
                                break;
                            case CellStatus.Filled:
                                writer.Write("|x|");
                                break;
                            case CellStatus.Empty:
                                writer.Write("| |");
                                break;
                            case CellStatus.Path:
                                writer.Write("|*|");
                                break;
                        }
                    }
                    writer.WriteLine();
                }
            }
            else
                writer.WriteLine("Error: Run a search algorithm before you try to print a path!");
            writer.WriteLine();
            writer.WriteLine();
        }


        /// <summary>
        /// Prints out the map of the AI search challenge, with start, goal and obstructions
        /// </summary>
        public void ConsoleASCIIPrintoutAStarFunction(TextWriter writer)
        {
            Cell c;
            Cell[,] arr = new Cell[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    arr[i, j] = FindCell(i + 1, j + 1);
                }
            }
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    c = arr[i, j];
                    writer.Write("|");
                    int star = c.DTG + c.Cost;
                    if(star < 10)
                        writer.Write(" ");
                    writer.Write(star);
                    writer.Write("|");
                }
                writer.WriteLine();
            }
            writer.WriteLine();
            writer.WriteLine();
        }


        public void ConsoleASCIIPrintoutGreedyFunction(TextWriter writer)
        {
            Cell c;
            Cell[,] arr = new Cell[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    arr[i, j] = FindCell(i + 1, j + 1);
                }
            }
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    c = arr[i, j];
                    writer.Write("|");
                    if (c.DTG < 10)
                        writer.Write(" ");
                    writer.Write(c.DTG);
                    writer.Write("|");
                }
                writer.WriteLine();
            }
            writer.WriteLine();
            writer.WriteLine();
        }

        /// <summary>
        /// Uses the default preference order.
        /// Depth search algorithm, uses a stack to continue expanding child nodes until either the goal is found or
        /// there are no valid nodes left. If the latter is the case, pops the stack and checks for the next valid option
        /// in the parent Cell and continues accordingly.
        /// 
        /// This is an uninformed search, meaning no clear preferential treatment is given to the child nodes for expansion.
        /// </summary>
        /// <returns>A stack of cell objects which make up the final path after search algorithm finishes</returns>
        public Stack<Cell> AI_DepthSearch()
        {
            _nodeExpansions = 0; //for tracking node expansions
            int thisSize;
            int nextSize;
            _path.Push(_start); //sets the beginning node
            while (_path.Peek().Status != CellStatus.Goal) //runs algorithm until goal cell is discovered
            {
                thisSize = _path.Count;
                Cell c = _path.Peek(); //set Cell to work with from top of stack
                        //If conditions met, expand this node, set this node to visited and add node to the stack, end the direction loop
                if (c.Up != null && !c.Up.Visited && c.Up.Status != CellStatus.Filled)
                {
                    c = c.Up;
                    c.Visited = true;
                    _path.Push(c);
                    _nodeExpansions++; //track expansion
                }
                else if (c.Left != null && !c.Left.Visited && c.Left.Status != CellStatus.Filled)
                {
                    c = c.Left;
                    c.Visited = true;
                    _path.Push(c);
                    _nodeExpansions++; //track expansion
                }
                else if (c.Down != null && !c.Down.Visited && c.Down.Status != CellStatus.Filled)
                {
                    c = c.Down;
                    c.Visited = true;
                    _path.Push(c);
                    _nodeExpansions++; //track expansion
                }
                else if (c.Right != null && !c.Right.Visited && c.Right.Status != CellStatus.Filled)
                {
                    c = c.Right;
                    c.Visited = true;
                    _path.Push(c);
                    _nodeExpansions++; //track expansion
                }
                nextSize = _path.Count;
                if (nextSize - thisSize == 0)
                    _path.Pop(); //if direction loop ends with no node expanded, pop the top node and search the previous one for more alternate paths
            }
            return _path;
        }



        /// <summary>
        /// User input direction order.
        /// Depth search algorithm, uses a stack to continue expanding child nodes until either the goal is found or
        /// there are no valid nodes left. If the latter is the case, pops the stack and checks for the next valid option
        /// in the parent Cell and continues accordingly.
        /// 
        /// This is an uninformed search, meaning no clear preferential treatment is given to the child nodes for expansion
        /// </summary>
        /// <returns>A stack of cell objects which make up the final path after search algorithm finishes</returns>
        public Stack<Cell> AI_DepthSearch(Direction[] order)
        {
            _nodeExpansions = 0; //for tracking node expansions
            int thisSize;
            int nextSize;
            _path.Push(_start); //sets the beginning node
            while (_path.Peek().Status != CellStatus.Goal) //runs algorithm until goal cell is discovered
            {
                thisSize = _path.Count;
                for (int i = 0; i < 4; i++) //each search iteration will test 4 directions in the order specified by 'order[]'
                {
                    Cell c = _path.Peek(); //set Cell to work with from top of stack
                    switch (order[i]) //check the current direction
                    {
                        case Direction.Up:
                            //If conditions met, expand this node, set this node to visited and add node to the stack, end the direction loop
                            if (c.Up != null && !c.Up.Visited && c.Up.Status != CellStatus.Filled)
                            {
                                c = c.Up;
                                c.Visited = true;
                                _path.Push(c);
                                _nodeExpansions++; //track expansion
                                i = 4;
                            }
                            break;
                        case Direction.Left:
                            if (c.Left != null && !c.Left.Visited && c.Left.Status != CellStatus.Filled)
                            {
                                c = c.Left;
                                c.Visited = true;
                                _path.Push(c);
                                _nodeExpansions++; //track expansion
                                i = 4;
                            }
                            break;
                        case Direction.Down:
                            if (c.Down != null && !c.Down.Visited && c.Down.Status != CellStatus.Filled)
                            {
                                c = c.Down;
                                c.Visited = true;
                                _path.Push(c);
                                _nodeExpansions++; //track expansion
                                i = 4;
                            }
                            break;
                        case Direction.Right:
                            if (c.Right != null && !c.Right.Visited && c.Right.Status != CellStatus.Filled)
                            {
                                c = c.Right;
                                c.Visited = true;
                                _path.Push(c);
                                _nodeExpansions++; //track expansion
                                i = 4;
                            }
                            break;
                    }
                }
                nextSize = _path.Count;
                if(nextSize-thisSize==0)
                    _path.Pop(); //if direction loop ends with no node expanded, pop the top node and search the previous one for more alternate paths
            }
            return _path;
        }



        /// <summary>
        /// Uses the default preference order.
        /// Breadth first search algorithm, uses a FIFO list to search for the goal Cell, then uses the linked parent of each
        /// cell in a sequence from the goal cell to generate a stack for the path
        /// 
        /// This is an uninformed search, meaning no clear preferential treatment is given to the next target for expansion
        /// </summary>
        /// <returns>A stack of cell objects which make up the final path after search algorithm finishes</returns>
        public Stack<Cell> AI_BreadthSearch() //
        {
            _nodeExpansions = 0; //for tracking node expansions
            bool finished = false;
            Cell c;
            _list.Add(_start); //initialise list
            while (!finished) //runs until goal is found
            {
                c = _list[0]; //expand the node at the front of the list
                if (c.Up != null && c.Up.Status != CellStatus.Filled && c.Up.Visited == false) //check if sub-node valid
                {
                    c.Up.Visited = true; //set visited flag to prevent future exploration of this node
                    c.Up.Parent = c; //set the parent of this node in order to track a path back to the start
                    _list.Add(c.Up); //add this node onto the END of the list to be expanded when it arrives at the top
                    _nodeExpansions++; //track expansion
                    if (c.Up.Status == CellStatus.Goal) //if this is the goal cell, cancel the search and the last element of the list will be the goal cell
                    {
                        finished = true;
                        break;
                    }
                }
                if (c.Left != null && c.Left.Status != CellStatus.Filled && c.Left.Visited == false)
                {
                    //see top if statement
                    c.Left.Visited = true;
                    c.Left.Parent = c;
                    _list.Add(c.Left);
                    _nodeExpansions++; //track expansion
                    if (c.Left.Status == CellStatus.Goal)
                    {
                        finished = true;
                        break;
                    }
                }
                if (c.Down != null && c.Down.Status != CellStatus.Filled && c.Down.Visited == false)
                {
                    //see top if statement
                    c.Down.Visited = true;
                    c.Down.Parent = c;
                    _list.Add(c.Down);
                    _nodeExpansions++; //track expansion
                    if (c.Down.Status == CellStatus.Goal)
                    {
                        finished = true;
                        break;
                    }
                }
                if (c.Right != null && c.Right.Status != CellStatus.Filled && c.Right.Visited == false)
                {
                    //see top if statement
                    c.Right.Visited = true;
                    c.Right.Parent = c;
                    _list.Add(c.Right);
                    _nodeExpansions++; //track expansion
                    if (c.Right.Status == CellStatus.Goal)
                    {
                        finished = true;
                        break;
                    }
                }
                //remove the current expanded node, as it has been explored and has not found the goal cell
                _list.RemoveAt(0);
            }

            //Start generating the path by pushing the goal cell (last element of the list) into the stack
            _path.Push(_list[_list.Count - 1]);

            //initialise c to work with the goal cell
            c = _path.Peek();

            //iterate through the parents of each cell to get to the starting cell, pushing each cell into the stack
            //stop when start cell is detected
            while (c != _start)
            {
                c = c.Parent;
                _path.Push(c);
            }

            //Reverse the order of the stack to match expected path order of other algorithms
            Cell[] arr = _path.ToArray();
            _path.Clear();
            arr.Reverse();
            _path = new Stack<Cell>(arr);
            //return the amount of cells which make up the path
            return _path;
        }



        /// <summary>
        /// User input direction order.
        /// Breadth first search algorithm, uses a FIFO list to search for the goal Cell, then uses the linked parent of each
        /// cell in a sequence from the goal cell to generate a stack for the path.
        /// 
        /// This is an uninformed search, meaning no clear preferential treatment is given to the next target for expansion
        /// </summary>
        /// <returns>A stack of cell objects which make up the final path after search algorithm finishes</returns>
        public Stack<Cell> AI_BreadthSearch(Direction[] order) //
        {
            _nodeExpansions = 0; //for tracking node expansions
            bool finished = false;
            Cell c;
            _list.Add(_start); //initialise list
            while(!finished) //runs until goal is found
            {
                for (int i = 0; i < 4 && !finished; i++)
                {
                    c = _list[0]; //expand the node at the front of the list
                    switch (order[i])
                    {

                        case Direction.Up:
                            if (c.Up != null && c.Up.Status != CellStatus.Filled && c.Up.Visited == false) //check if sub-node valid
                            {
                                c.Up.Visited = true; //set visited flag to prevent future exploration of this node
                                c.Up.Parent = c; //set the parent of this node in order to track a path back to the start
                                _list.Add(c.Up); //add this node onto the END of the list to be expanded when it arrives at the top
                                _nodeExpansions++; //track expansion
                                if (c.Up.Status == CellStatus.Goal) //if this is the goal cell, cancel the search and the last element of the list will be the goal cell
                                    finished = true;
                            }
                            break;
                        case Direction.Right:
                            if (c.Right != null && c.Right.Status != CellStatus.Filled && c.Right.Visited == false)
                            {
                                //see top if statement
                                c.Right.Visited = true;
                                c.Right.Parent = c;
                                _list.Add(c.Right);
                                _nodeExpansions++; //track expansion
                                if (c.Right.Status == CellStatus.Goal)
                                    finished = true;
                            }
                            break;
                        case Direction.Down:

                            if (c.Down != null && c.Down.Status != CellStatus.Filled && c.Down.Visited == false)
                            {
                                //see top if statement
                                c.Down.Visited = true;
                                c.Down.Parent = c;
                                _list.Add(c.Down);
                                _nodeExpansions++; //track expansion
                                if (c.Down.Status == CellStatus.Goal)
                                    finished = true;
                            }
                            break;
                        case Direction.Left:

                            if (c.Left != null && c.Left.Status != CellStatus.Filled && c.Left.Visited == false)
                            {
                                //see top if statement
                                c.Left.Visited = true;
                                c.Left.Parent = c;
                                _list.Add(c.Left);
                                _nodeExpansions++; //track expansion
                                if (c.Left.Status == CellStatus.Goal)
                                    finished = true;
                            }
                            break;
                    }
                }
                //remove the current expanded node, as it has been explored and has not found the goal cell
                _list.RemoveAt(0);
            }
            
            //Start generating the path by pushing the goal cell (last element of the list) into the stack
            _path.Push(_list[_list.Count-1]);

            //initialise c to work with the goal cell
            c = _path.Peek();

            //iterate through the parents of each cell to get to the starting cell, pushing each cell into the stack
            //stop when start cell is detected
            while (c!=_start)
            {
                c = c.Parent;
                _path.Push(c);
            }

            //Reverse the order of the stack to match expected path order of other algorithms
            Cell[] arr = _path.ToArray();
            _path.Clear();
            arr.Reverse();
            _path = new Stack<Cell>(arr);
            //return the amount of cells which make up the path
            return _path;
        }



        /// <summary>
        /// A* algorith, uses a combination of distance cost from starting cell and straight line distance from goal cell
        /// to choose an appropriate path which aims to save time by guessing the best options
        /// </summary>
        /// <returns>A stack of cell objects which make up the final path after search algorithm finishes</returns>
        public Stack<Cell> AI_AStarSearch()
        {
            _nodeExpansions = 0; //for tracking node expansions
            List<Cell> routeOptions = new List<Cell>(); //list used to store cells that are valid for node expansion
            _path.Push(_start); //push the start cell to allow node expansion based on this cell
            Cell c; //a cell reference to work on the current node
            while (_path.Peek().Status != CellStatus.Goal) //runs algorithm until goal cell is discovered
            {
                c = _path.Peek(); //change the current active node
                routeOptions.Clear(); //clears list for new check
                if(c.Up != null && c.Up.Status != CellStatus.Filled && c.Up.Visited == false) //only add an option if there is one
                {
                    routeOptions.Add(c.Up);
                }
                if (c.Left != null && c.Left.Status != CellStatus.Filled && c.Left.Visited == false) //see first if for details
                {
                    routeOptions.Add(c.Left);
                }
                if (c.Down != null && c.Down.Status != CellStatus.Filled && c.Down.Visited == false) //see first if for details   
                {
                    routeOptions.Add(c.Down);
                }
                if (c.Right != null && c.Right.Status != CellStatus.Filled && c.Right.Visited == false) //see first if for details
                {
                    routeOptions.Add(c.Right);
                }
                if (routeOptions.Count > 0)
                {
                    int aStarPrice;
                    int currentMin = this.Height+this.Width; //this value is set high enough to always let the first option win
                    Cell best = null; //need to find the best route in order to push the right cell onto the path stack
                    foreach (Cell r in routeOptions) //use to find best route for A* based on both distance to goal and cost from start
                    {
                        aStarPrice = r.DTG + r.Cost;
                        if (aStarPrice < currentMin) //evalute best A* option based on combined cost and distance
                        {
                            currentMin = aStarPrice; //set the new minimum
                            best = r; //hold onto the best Route so far
                        }
                    }
                    best.Visited = true;
                    _path.Push(best); //push the destination cell of the best Route into the path stack
                    _nodeExpansions++; //track expansion
                }
                else
                {
                    _path.Pop(); //No options available for expansion, pop current cell off the path in order to re-evaluate the previous one
                }
            }
            return _path;
        }



        /// <summary>
        /// Greedy first algorithm attempts to reach the target quickly by expanding only nodes closest to the goal value.
        /// Note: only difference between this and AI_AStarSearch is that Greedy does not evaluate path cost
        /// </summary>
        /// <returns>A stack of cell objects which make up the final path after search algorithm finishes</returns>
        public Stack<Cell> AI_GreedyFirstSearch()
        {
            _nodeExpansions = 0; //for tracking node expansions
            List<Cell> routeOptions = new List<Cell>();
            _path.Push(_start); //push the start cell to allow node expansion based on this cell
            Cell c; //a cell reference to work on the current node
            while (_path.Peek().Status != CellStatus.Goal) //runs algorithm until goal cell is discovered
            {
                c = _path.Peek(); //change the current active node
                routeOptions.Clear(); //clears list for new check
                if (c.Up != null && c.Up.Status != CellStatus.Filled && c.Up.Visited == false) //only add an option if there is one
                {
                    routeOptions.Add(c.Up); //store direction taken with destination cell
                }
                if (c.Left != null && c.Left.Status != CellStatus.Filled && c.Left.Visited == false) //see first if for details
                {
                    routeOptions.Add(c.Left);
                }
                if (c.Down != null && c.Down.Status != CellStatus.Filled && c.Down.Visited == false) //see first if for details   
                {
                    routeOptions.Add(c.Down);
                }
                if (c.Right != null && c.Right.Status != CellStatus.Filled && c.Right.Visited == false) //see first if for details
                {
                    routeOptions.Add(c.Right);
                }
                if (routeOptions.Count > 0)
                {
                    int currentMin = this.Height + this.Width; //this value is set high enough to always let the first option win
                    Cell best = null;
                    foreach (Cell r in routeOptions) //use to find best route for A* based on both distance to goal and cost from start
                    {
                        if (r.DTG < currentMin) //evalute best A* option based on combined cost and distance
                        {
                            currentMin = r.DTG; //set the new ma
                            best = r; //assign current test passing Route as the best
                        }
                    }
                    best.Visited = true;
                    _path.Push(best); //push the destination cell of the best Route into the path stack
                    _nodeExpansions++; //track expansion
                }
                else
                {
                    _path.Pop(); //No options available for expansion, pop current cell off the path in order to re-evaluate the previous one
                }
            }
            return _path;
        }



        public void Reset() //Wipes the slate clean for the next Algorithm
        {
            _list.Clear();
            _path.Clear();
            foreach(Cell c in _cells)
            {
                c.Visited = false;
                if (c.Status == CellStatus.Path)
                    c.Status = CellStatus.Empty;
            }
        }


    }
}

