using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordsSpawner : MonoBehaviour
    {
        [SerializeField] private WordCompositeRoot _wordCompositeRootPrefab;
        [SerializeField] private WordsArea _wordsSpawnArea;
        
        public List<WordCompositeRoot> SpawnWords(IEnumerable<Word> words)
        {
            List<WordCompositeRoot> wordCompositeRoots = new(); 
            foreach (Word levelDataWord in words)
            {
                var spawnParent = _wordsSpawnArea.GetCorrespondingWordParent(levelDataWord.CellsCount);
                WordCompositeRoot wordCompositeRoot = Instantiate(_wordCompositeRootPrefab, spawnParent);
                wordCompositeRoot.Composite(levelDataWord.CellsCount);
                wordCompositeRoots.Add(wordCompositeRoot);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_wordsSpawnArea.transform as RectTransform);
            return wordCompositeRoots;
        }
        
    }
}
