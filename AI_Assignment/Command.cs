namespace AI_Assignment
{
    public class Command
    {
        private string _mapName;
        private string _searchMethod;



        public Command(string[] input)
        {
            MapName = input[0];
            SearchMethod = input[1];
        }

        public string MapName { get => _mapName; set => _mapName = value; }
        public string SearchMethod { get => _searchMethod; set => _searchMethod = value; }
    }
}