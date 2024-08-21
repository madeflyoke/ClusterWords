namespace Source.Modules.GameLogicModule.Scripts.Utils
{
    public static class GameConstants //calibrating depends on preferences of screen size, gameplay field, difficulty etc.
    {
        public const int MIN_CLUSTER_SIZE = 2;
        public const int MAX_CLUSTER_SIZE = 4;
        
        public const int MIN_LETTERS_COUNT = 4;
        public const int MAX_LETTERS_COUNT = 7;

        public const int MAX_WORD_FIELD_SIZE_LETTERS = 22; //count of chars cells to be fit into words area
        public const int MAX_CLUSTERS_FIELD_SIZE_LETTERS = 60; //approx count of chars cells to be fit into clusters area
    }
}
