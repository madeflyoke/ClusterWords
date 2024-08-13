using System;
using System.Collections.Generic;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    [Serializable]
    public class LevelData
    {
        public int Id;
        public List<WordsRequestData> WordsRequests;
    }
}
