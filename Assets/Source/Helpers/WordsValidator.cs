#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EasyButtons;
using ModestTree;
using Source.Modules.GameLogicModule.Scripts.Levels;
using UnityEngine;

namespace Source.Helpers
{
    public class WordsValidator : MonoBehaviour
    {
        [SerializeField] private List<WordsRequestData> _readonly_wordsValidationDatas;
        [SerializeField] private int _readonly_totalWordsCount;

        #region WordsValidationAndRecreationRegion

        [Button]
        void ValidateRawFile(string rawFileName)
        {
            string cleanFileContent = GetCleanWordsContent(rawFileName);

            var originWordsCount = cleanFileContent.Split(Environment.NewLine, System.StringSplitOptions.RemoveEmptyEntries).Length;
                
            string processedContent = ValidateWordsLength(cleanFileContent);
            processedContent = ValidateRepeats(processedContent);

            if (originWordsCount!=processedContent.Split(Environment.NewLine, System.StringSplitOptions.RemoveEmptyEntries).Length)
            {
                Debug.LogWarning("Changes were made, words removed: "+(originWordsCount-processedContent.Split(Environment.NewLine).Length));
            }
            
            SaveProcessedFile(processedContent, rawFileName);
        }

        string ValidateWordsLength(string content)
        {
            var lines = content.Split(Environment.NewLine, System.StringSplitOptions.RemoveEmptyEntries);
            return Array.FindAll(lines, line => Regex.IsMatch(line, @"^\w{4,7}$")).Join(Environment.NewLine);
        }

        string ValidateRepeats(string content)
        {
            var lines = content.Split(Environment.NewLine, System.StringSplitOptions.RemoveEmptyEntries);
            var uniqueWords = new HashSet<string>(lines);
       
            return string.Join(Environment.NewLine, uniqueWords);
        }
    
        void SaveProcessedFile(string content, string fileName)
        {
            string path = Path.Combine(Application.dataPath, "Resources", fileName+"Final.txt");
            File.WriteAllText(path, content);
            Debug.Log("Processed file saved to: " + path);
        }

        #endregion
        
        
        #region WordsCountRegion

        
        [Button]
        public void FillWordsData(string wordsFileName)
        {
            _readonly_wordsValidationDatas = new List<WordsRequestData>();
            
            var cleanWords = GetCleanWordsContent(wordsFileName).Split(Environment.NewLine, System.StringSplitOptions.RemoveEmptyEntries);
                
            for (var index = 0; index < cleanWords.Length; index++)
            {
                var finalWord = cleanWords[index];
                    
                var dataBlock = _readonly_wordsValidationDatas.FirstOrDefault(x => x.LettersCount == finalWord.Length);
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
        
        private string GetCleanWordsContent(string wordsFileName)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(wordsFileName);
            if (textAsset!=null)
            {
                var rawContent = textAsset.text;

                var lines = rawContent.Split(Environment.NewLine, System.StringSplitOptions.RemoveEmptyEntries);
                for (var index = 0; index < lines.Length; index++)
                {
                    lines[index] = lines[index].Replace("\"", "").Replace(",", "");
                    if (Regex.IsMatch(lines[index], @"[^a-zA-Z]") == false)
                    {
                        lines[index] = string.Empty;
                        continue;
                    }
                }

                lines = lines.Where(x => string.IsNullOrEmpty(x) == false).ToArray();
                return string.Join(Environment.NewLine, lines);
            }

            return null;
        }
    }
}

#endif


