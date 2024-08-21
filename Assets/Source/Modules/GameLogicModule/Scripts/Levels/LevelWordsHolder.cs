using System.Collections.Generic;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class LevelWordsHolder
    {
        private readonly List<Word> _words;
        public IReadOnlyCollection<Word> Words => _words.AsReadOnly();

        public LevelWordsHolder(List<Word> words)
        {
            _words = words;
        }
    }
}