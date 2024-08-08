using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordsArea : MonoBehaviour
    {
        [SerializeField] private List<WordsBlock> _wordsBlocks;
        
        public Transform GetCorrespondingWordParent(int cellsCount)
        {
            var block = _wordsBlocks.FirstOrDefault(x => x.CharsCapacity == cellsCount);
            if (block==null)
            {
                Debug.LogError("Incorrect cells count in word!");
                return null;
            }
            block.WordsParent.gameObject.SetActive(true);
            return block.WordsParent;
        }

        [Serializable]
        public class WordsBlock
        {
            public int CharsCapacity;
            public Transform WordsParent;
        }
    }
}
