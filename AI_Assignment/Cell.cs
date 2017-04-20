namespace AI_Assignment
{
    public class Cell
    {
        //Cell Location X and Y
        private int _x;
        private int _y;

        private int _cost;
        private int _dtg;

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
            _status = CellStatus.Empty; //default to empty cell
            _visited = false;
            _x = x;
            _y = y;
        }

        //Once location is set it cannot be changed
        public int X { get => _x; }
        public int Y { get => _y; }

        //Adjacent Cells are set after all cells have been generated
        public Cell Up { get => _up; set => _up = value; }
        public Cell Down { get => _down; set => _down = value; }
        public Cell Right { get => _right; set => _right = value; }
        public Cell Left { get => _left; set => _left = value; }

        //Algorithms must have access to the following data about cells
        public bool Visited { get => _visited; set => _visited = value; }
        public CellStatus Status { get => _status; set => _status = value; }
        public Cell Parent { get => _parent; set => _parent = value; }
        public int Cost { get => _cost; set => _cost = value; }
        public int DTG { get => _dtg; set => _dtg = value; }
    }
}