#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EasyButtons;
using Source.Modules.GameLogicModule.Scripts.Levels;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Source.Modules.EditorModule
{
    public class WordsValidator : MonoBehaviour
    {
        [SerializeField] private List<WordsRequestData> _readonly_wordsValidationDatas;
        [SerializeField] private int _readonly_totalWordsCount;

        #region WordsValidationAndRecreationRegion

        [Button]
        private void ValidateWordsJson()
        {
            var wordsFetcher = new WordsFetcher();
            var allWords = wordsFetcher.GetAllWords();
            wordsFetcher.Dispose();
            
            var originWordsCount = allWords.Count;
            
            allWords = ValidateWordsLength(allWords);
            allWords = ValidateRepeats(allWords);

            if (originWordsCount!=allWords.Count)
            {
                Debug.LogWarning("Changes were made, words removed: "+(originWordsCount-allWords.Count));
            }
            SaveProcessedFile(new WordsModel(allWords));
        }

        private List<string> ValidateWordsLength(List<string> words)
        {
            return words.Where(x => x.Length is >= 4 and <= 7).ToList();
        }

        private List<string> ValidateRepeats(List<string> words)
        {
            return new HashSet<string>(words).ToList();
        }
    
        private void SaveProcessedFile(WordsModel wordsModel)
        {
            string json = JsonConvert.SerializeObject(wordsModel, Formatting.Indented);
            string path = Path.Combine(Application.dataPath, "Resources", WordsFetcher.WORDS_RESOURCE_NAME + ".json");
            File.WriteAllText(path, json);
            Debug.Log("Processed file saved to: " + path);
        }

        #endregion
        
        
        #region WordsCountRegion

        
        [Button]
        public void FillWordsData()
        {
            _readonly_wordsValidationDatas = new List<WordsRequestData>();
            
            var wordsFetcher = new WordsFetcher();
            var allWords = wordsFetcher.GetAllWords();
            wordsFetcher.Dispose();
            
            foreach (var finalWord in allWords)
            {
                var word = finalWord;
                var dataBlock = _readonly_wordsValidationDatas.FirstOrDefault(x => x.LettersCount == word.Length);
                if (dataBlock == null)
                {
                    _readonly_wordsValidationDatas.Add(new WordsRequestData(finalWord.Length, 1));
                }
                else
                {
                    dataBlock.WordsCount++;
                }
            }
            
            _readonly_totalWordsCount = _readonly_wordsValidationDatas.Sum(x => x.WordsCount);
        }
        
        #endregion
    }
}

#endif


