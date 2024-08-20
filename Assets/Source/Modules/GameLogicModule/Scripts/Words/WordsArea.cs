using System.Collections.Generic;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.UI;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordsArea : MonoBehaviour
    {
        public const int MAX_WORDS_PER_BLOCK = 6;
        
        [SerializeField] private UIElementsBlock _blockPrefab;
        private List<UIElementsBlock> _currentBlocks;

        private void Awake()
        {
            _currentBlocks = new List<UIElementsBlock>();
        }
        
        public Transform GetCorrespondingWordParent(int charsCount)
        {
            UIElementsBlock block = _currentBlocks.FirstOrDefault(x => x.Capacity == charsCount
                                                                          && x.transform.childCount < MAX_WORDS_PER_BLOCK);
            if (block==null)
            {
                block = Instantiate(_blockPrefab, transform).Initialize(charsCount);
                _currentBlocks.Add(block);
            }
            return block.transform;
        }
    }
}
