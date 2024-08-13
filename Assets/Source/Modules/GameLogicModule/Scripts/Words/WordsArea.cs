using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordsArea : MonoBehaviour
    {
        public const int MAX_WORDS_PER_BLOCK = 6;
        
        [SerializeField] private UIElementsBlock _blockPrefab;
        private List<UIElementsBlock> _currentBlocksMap;

        private void Awake()
        {
            _currentBlocksMap = new List<UIElementsBlock>();
        }
        
        public Transform GetCorrespondingWordParent(int charsCount)
        {
            UIElementsBlock block = _currentBlocksMap.FirstOrDefault(x => x.Capacity == charsCount
                                                                          && x.transform.childCount < MAX_WORDS_PER_BLOCK);
            if (block==null)
            {
                block = Instantiate(_blockPrefab, transform).Initialize(charsCount);
                _currentBlocksMap.Add(block);
            }
            return block.transform;
        }
    }
}
