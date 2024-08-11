using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    [Serializable]
    public class WordsModel
    {
        public WordsModel(List<string> words)
        {
            Words = words;
        }

        [field: SerializeField] public List<string> Words { get; private set; }
    }
}