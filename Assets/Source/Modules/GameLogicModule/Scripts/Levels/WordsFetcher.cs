using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class WordsFetcher : IDisposable
    {
        public const string WORDS_RESOURCE_NAME = "Words";

        private readonly Dictionary<int, List<string>> _wordsMap;
        
        public WordsFetcher()
        {
            _wordsMap = new Dictionary<int, List<string>>();
            TextAsset textAsset = Resources.Load<TextAsset>(WORDS_RESOURCE_NAME);
    
            if (textAsset != null)
            {
                WordsModel deserializedObject = JsonConvert.DeserializeObject<WordsModel>(textAsset.text);
                
                var words = deserializedObject.Words;
                foreach (var word in words)
                {
                    if (_wordsMap.ContainsKey(word.Length)==false)
                    {
                        _wordsMap.Add(word.Length, new List<string>());
                    }

                    _wordsMap[word.Length].Add(word);
                }
            }
        }

        public List<string> GetAllWords()
        {
            return _wordsMap.Values.SelectMany(x=>x).ToList();
        }
        
        public List<string> GetWords(List<WordsRequestData> requestDatas)
        {
            var finalList = new List<string>();
            var mapCopy = _wordsMap.ToDictionary(
                kvp => kvp.Key, 
                kvp => kvp.Value.ShuffledCopy()
            );
            
            foreach (var data in requestDatas)
            {
                var shuffledWords = mapCopy[data.LettersCount];
                var targetPart = shuffledWords.GetRange(0, data.WordsCount);
                mapCopy[data.LettersCount] = shuffledWords.Except(targetPart).ToList();
                finalList.AddRange(targetPart);
            }

            return finalList;
        }

        public void Dispose()
        {
        }
    }
}