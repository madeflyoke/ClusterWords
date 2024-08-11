using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class WordsFetcher
    {
        private const string WORDS_RESOURCE_NAME = "WordsFinal";

        private Dictionary<int, List<string>> _wordsMap;

        public WordsFetcher()
        {
            _wordsMap = new Dictionary<int, List<string>>();
            TextAsset textAsset = Resources.Load<TextAsset>(WORDS_RESOURCE_NAME);
    
            if (textAsset != null)
            {
                string fileContent = textAsset.text;
                
                var words = fileContent.Split(Environment.NewLine, System.StringSplitOptions.RemoveEmptyEntries);

                for (var index = 0; index < words.Length; index++)
                {
                    var word = words[index];

                    if (_wordsMap.ContainsKey(word.Length)==false)
                    {
                        _wordsMap.Add(word.Length, new List<string>());
                    }

                    _wordsMap[word.Length].Add(word);
                }
            }
        }
        
        public List<string> FetchWords(List<WordsRequestData> requestDatas)
        {
            var finalList = new List<string>();
            foreach (var data in requestDatas)
            {
                var shuffledWords = _wordsMap[data.LettersCount].ShuffledCopy();
                finalList.AddRange(shuffledWords.GetRange(0, data.WordsCount));
            }

            return finalList;
        }
    }
}