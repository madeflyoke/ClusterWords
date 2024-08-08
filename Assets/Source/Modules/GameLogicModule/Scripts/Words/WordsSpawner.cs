using System.Collections.Generic;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordsSpawner : MonoBehaviour
    {
        [SerializeField] private WordCompositeRoot _wordCompositeRoot;
        [SerializeField] private WordsArea _wordsSpawnArea;
        
        public List<WordCompositeRoot> SpawnWords(IEnumerable<Word> words)
        {
            List<WordCompositeRoot> wordCompositeRoots = new(); 
            foreach (Word levelDataWord in words)
            {
                WordCompositeRoot wordCompositeRoot = Instantiate(_wordCompositeRoot, _wordsSpawnArea.GetCorrespondingWordParent(levelDataWord.CellsCount));
                wordCompositeRoot.Composite(levelDataWord.CellsCount);
                wordCompositeRoots.Add(wordCompositeRoot);
            }

            return wordCompositeRoots;
        }
    }
}
