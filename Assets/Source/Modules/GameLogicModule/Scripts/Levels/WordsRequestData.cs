using System;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    [Serializable]
    public class WordsRequestData
    {
        [SerializeField, Range(GameConstants.MIN_LETTERS_COUNT, GameConstants.MAX_LETTERS_COUNT)] public int LettersCount;
        [SerializeField, Range(1, GameConstants.MAX_WORDS_COUNT_PER_BLOCK)] public int WordsCount;

        public WordsRequestData(){}
        
        public WordsRequestData(int lettersCount, int wordsCount)
        {
            LettersCount = lettersCount;
            WordsCount = wordsCount;
        }
    }
}
