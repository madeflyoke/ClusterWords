using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Levels/LevelsConfig")]
    public class LevelsConfig : SerializedScriptableObject
    {
        [SerializeField] private List<LevelData> _levelDatas;

        public LevelData GetLevelData(int id)
        {
            return _levelDatas.FirstOrDefault(x => x.Id == id);
        }
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            var index = 0;
            foreach (var levelData in _levelDatas)
            {
                if (levelData.WordsRequests.Sum(x=>x.LettersCount)>GameConstants.MAX_WORD_FIELD_SIZE_LETTERS)
                {
                    Debug.LogError("Level data has too much letters to fit screen size, id: "+levelData.Id);
                }

                levelData.Id = index;
                index++;
            }
        }

#endif
    }
}
