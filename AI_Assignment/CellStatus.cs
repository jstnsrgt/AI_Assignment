namespace AI_Assignment
{
    //Cells default to Empty, but some are changed to Filled, Start and Goal during initialisation of a map object
    public enum CellStatus
    {
        Start,  //Specifies a starting cell to begin AI search from
        Goal,   //Specifies a cell which successfully finishes the AI search
        Filled, //Specifies an unusable cell which cannot be expanded in a search
        Empty,   //Specifies a cell ready to be expanded for additional searching
        Path
    }

}