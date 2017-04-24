namespace AI_Assignment
{
    public class Command
    {
        private string _mapName;
        private string _searchMethod;


        /// <summary>
        /// Takes in and stores appropriate program input arguments and stores them with appropriately named properties
        /// </summary>
        /// <param name="input"></param>
        public Command(string[] input)
        {
            MapName = input[0];
            SearchMethod = input[1];
        }

        public string MapName
        {
            get
            {
                return _mapName;
            }

            set
            {
                _mapName = value;
            }
        }

        public string SearchMethod
        {
            get
            {
                return _searchMethod;
            }

            set
            {
                _searchMethod = value;
            }
        }
    }
}