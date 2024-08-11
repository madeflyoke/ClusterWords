using System;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    [Serializable]
    public class WordsRequestData
    {
        public int LettersCount;
        public int WordsCount;

        public WordsRequestData(int lettersCount, int wordsCount)
        {
            LettersCount = lettersCount;
            WordsCount = wordsCount;
        }
    }
}
