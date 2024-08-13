using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    [Serializable]
    public class LevelData
    {
        public int Id;
        
        #if UNITY_EDITOR
        [GUIColor(nameof(ValidatedColor))]
        #endif
        public List<WordsRequestData> WordsRequests;
        
        #if UNITY_EDITOR

        [ReadOnly, ShowInInspector] public int EDITOR_TotalWordsCount => WordsRequests.Sum(x => x.WordsCount);

        private Color ValidatedColor()
        {
            return (WordsRequests.Sum(x => x.LettersCount) > GameConstants.MAX_GAME_FIELD_SIZE_LETTERS)
                ? Color.red
                : Color.white;
        }
#endif
    }
}
