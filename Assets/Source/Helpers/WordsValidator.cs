#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EasyButtons;
using UnityEngine;

namespace Source.Helpers
{
    public class WordsValidator : MonoBehaviour
    {
        [SerializeField] private List<WordsValidationData> _readonly_wordsValidationDatas;
        [SerializeField] private int _readonly_totalWordsCount;

        #region WordsValidationAndRecreationRegion

        [Button]
        void ValidateRawFile(string wordsFileName)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(wordsFileName);
        
            if (textAsset != null)
            {
                string fileContent = textAsset.text;

                var originWordsCount = fileContent.Split("\n").Length;
                
                string processedContent = RemoveLinesWithShortQuotedWords(fileContent);
                processedContent = RemoveRepeatedWords(processedContent);

                if (originWordsCount!=processedContent.Split("\n").Length)
                {
                    Debug.LogWarning("Changes were made, words removed: "+(originWordsCount-processedContent.Split("\n").Length));
                }
             

                SaveProcessedFile(processedContent, wordsFileName);
            }
            else
            {
                Debug.LogError("File not found!");
            }
        }

        string RemoveLinesWithShortQuotedWords(string content)
        {
            return Regex.Replace(content, @"^.*?\""(\w{1,3}|\w{8,})\"".*$\n?", "", RegexOptions.Multiline);
        }

        string RemoveRepeatedWords(string content)
        {
            var lines = content.Split('\n');
            var uniqueWords = new HashSet<string>(lines);
       
            return string.Join("\n", uniqueWords);
        }
    
        void SaveProcessedFile(string content, string fileName)
        {
            string path = Path.Combine(Application.dataPath, "Resources", fileName+"Final.txt");
            File.WriteAllText(path, content);
            Debug.Log("Processed file saved to: " + path);
        }

        #endregion
        
        
        #region WordsCountRegion

        [Serializable]
        public class WordsValidationData
        {
            public int LettersCount;
            public int WordsCount;

            public WordsValidationData(int lettersCount, int wordsCount)
            {
                LettersCount = lettersCount;
                WordsCount = wordsCount;
            }
        }

        [Button]
        public void FillWordsData(string wordsFileName)
        {
            _readonly_wordsValidationDatas = new List<WordsValidationData>();
    
            TextAsset textAsset = Resources.Load<TextAsset>(wordsFileName);
    
            if (textAsset != null)
            {
                string fileContent = textAsset.text;

                var lines = fileContent.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    var words = line.Replace("\"", "").Replace(",", "").Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

                    foreach (var word in words)
                    {
                        var dataBlock = _readonly_wordsValidationDatas.FirstOrDefault(x => x.LettersCount == word.Length);
                        if (dataBlock == null)
                        {
                            _readonly_wordsValidationDatas.Add(new WordsValidationData(word.Length, 1));
                        }
                        else
                        {
                            dataBlock.WordsCount++;
                        }
                    }
                }
            }

            _readonly_totalWordsCount = _readonly_wordsValidationDatas.Sum(x => x.WordsCount);
        }
        
        #endregion
    }
}

#endif


