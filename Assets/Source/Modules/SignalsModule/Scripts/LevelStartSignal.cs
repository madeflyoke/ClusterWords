namespace Source.Modules.SignalsModule.Scripts
{
    public struct LevelStartSignal
    {
        public int LevelIndex;

        public LevelStartSignal(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}