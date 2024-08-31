namespace Source.Modules.SignalsModule.Scripts
{
    public struct LevelStartSignal
    {
        public readonly int LevelId;

        public LevelStartSignal(int levelId)
        {
            LevelId = levelId;
        }
    }
}