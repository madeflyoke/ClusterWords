using System;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    [Serializable]
    public class WordsRequestData
    {
        [SerializeField, Range(GameConstants.MIN_LETTERS_COUNT, GameConstants.MAX_LETTERS_COUNT)] public int LettersCount;
        [SerializeField, Range(1, WordsArea.MAX_WORDS_PER_BLOCK)] public int WordsCount;

        public WordsRequestData(){}
        
        public WordsRequestData(int lettersCount, int wordsCount)
        {
            LettersCount = lettersCount;
            WordsCount = wordsCount;
        }
    }
}
