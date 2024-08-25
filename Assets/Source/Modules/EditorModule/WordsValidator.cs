#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Source.Modules.GameLogicModule.Scripts.Levels;
using UnityEngine;
using UnityEngine.Networking;

namespace Source.Modules.EditorModule
{
    public class WordsValidator : MonoBehaviour
    {
        [SerializeField] private List<WordsRequestData> _readonly_wordsValidationDatas;
        [SerializeField] private int _readonly_totalWordsCount;

        #region WordsValidationAndRecreationRegion
        
        [Button]
        private async void ValidateWordsJson() //Long waiting required (probable some minutes)
        {
            Debug.Log("Validating started...");
            
            var wordsFetcher = new WordsFetcher();
            var allWords = wordsFetcher.GetAllWords();
            wordsFetcher.Dispose();
            
            var originWordsCount = allWords.Count;
            
            allWords = ValidateWordsLength(allWords);
            allWords = ValidateRepeats(allWords);
            
            allWords = await ValidateExisting(allWords);

            if (originWordsCount!=allWords.Count)
            {
                Debug.Log("Changes were made, words removed: "+(originWordsCount-allWords.Count));
            }
            SaveProcessedFile(new WordsModel(allWords));
            
            DebugLogWithColor("Validating completed!", Color.green);
        }

        private List<string> ValidateWordsLength(List<string> words)
        {
            Debug.Log("Length validating started...");

            var finalWords = words.Where(x => x.Length is >= 4 and <= 7).ToList();
            
            DebugLogWithColor("Length validating completed!", Color.green);

            return finalWords;
        }

        private List<string> ValidateRepeats(List<string> words)
        {
            Debug.Log("Repeats validating started...");

            var finalWords = new HashSet<string>(words).ToList();
            
            DebugLogWithColor("Repeats validating completed!", Color.green);

            return finalWords;
        }

        //HEAVY API OPERATION
        private async UniTask<List<string>> ValidateExisting(List<string> words)
        {
            Debug.Log("Repeats validating started, be patient...");

            async UniTask<bool> CheckWord(string word, string languageCode)
            {
                var struckTimerSeconds = TimeSpan.TicksPerSecond * 30;
                var startTime = DateTime.UtcNow.Ticks;
                
                string url =
                    $"https://{languageCode}.wiktionary.org/w/api.php?action=query&titles={UnityWebRequest.EscapeURL(word)}" +
                    $"&format=json&prop=extracts&explaintext&exsectionformat=plain";

                using (UnityWebRequest request = UnityWebRequest.Get(url))
                {
                    var operation = request.SendWebRequest();
                    
                    while (operation.isDone == false)
                    {
                        if (startTime+struckTimerSeconds<=DateTime.UtcNow.Ticks)
                        {
                            Debug.LogError($"Timed out at word {word}!");
                            return false;
                        }
                        await UniTask.Yield();
                    }

                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                        request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogError(request.error);
                    }
                    else
                    {
                        var json = request.downloadHandler.text;
                        if (json.Contains("\"-1\""))
                        {
                            return false;
                        }
                        return true;
                    }
                }

                return false;
            }

            List<string> finalWords = new List<string>(words);
            
            foreach (var word in words)
            {
                Debug.Log($"Checking the word: {word}...");
                bool result = await CheckWord(word, "ru");
                if (result==false)
                {
                    finalWords.Remove(word);
                    DebugLogWithColor($"Removed word: {word}", Color.yellow);
                }
            }
            
            DebugLogWithColor("Repeats validating completed!", Color.green);

            return finalWords;
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
            Debug.Log("Filling words data started...");
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
            DebugLogWithColor("Filling words data completed!", Color.green);
        }
        
        #endregion
        
        private void DebugLogWithColor(string message, Color color)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(color);
            Debug.Log($"<color=#{colorHex}>{message}</color>");
        }
    }
}

#endif


