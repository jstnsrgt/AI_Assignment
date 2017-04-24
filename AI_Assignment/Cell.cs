namespace AI_Assignment
{
    public class Cell
    {
        //Cell Location X and Y
        private int _x;
        private int _y;

        private int _cost;
        private int _dtg;
        private double _advDtg;
        private double _advCost;

        //Referenced adjacent cells
        private Cell _up;
        private Cell _down;
        private Cell _right;
        private Cell _left;

        //Used to allocate a parent cell in order to backtrack through cells when generating a path
        private Cell _parent;

        //Flag for search algorithm visit
        private bool _visited;

        //Cellstatus flag, enum options are: Empty,Filled,Goal,Start
        private CellStatus _status;

        




        //Cell constructs with location and initialised to not visited 
        public Cell(int x, int y)
        {
            Status = CellStatus.Empty; //default to empty cell
            Visited = false;
            _x = x;
            _y = y;
        }

        //Once location is set it cannot be changed
        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }


        //Adjacent Cells are set after all cells have been generated
        public Cell Up
        {
            get
            {
                return _up;
            }

            set
            {
                _up = value;
            }
        }

        public Cell Down
        {
            get
            {
                return _down;
            }

            set
            {
                _down = value;
            }
        }

        public Cell Right
        {
            get
            {
                return _right;
            }

            set
            {
                _right = value;
            }
        }

        public Cell Left
        {
            get
            {
                return _left;
            }

            set
            {
                _left = value;
            }
        }


        //Algorithms must have access to the following data about cells
        public int Cost
        {
            get
            {
                return _cost;
            }

            set
            {
                _cost = value;
            }
        } //Cost from start

        public double AdvCost
        {
            get
            {
                return _advCost;
            }

            set
            {
                _advCost = value;
            }
        }

        public int DTG
        {
            get
            {
                return _dtg;
            }

            set
            {
                _dtg = value;
            }
        } //Distance to Goal

        public double AdvDTG
        {
            get
            {
                return _advDtg;
            }

            set
            {
                _advDtg = value;
            }
        }

        public Cell Parent
        {
            get
            {
                return _parent;
            }

            set
            {
                _parent = value;
            }
        } //track a parent node for BFS

        public bool Visited
        {
            get
            {
                return _visited;
            }

            set
            {
                _visited = value;
            }
        }

        public CellStatus Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        
    }
}