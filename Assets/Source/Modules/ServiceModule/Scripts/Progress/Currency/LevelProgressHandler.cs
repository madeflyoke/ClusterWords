using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Progress.Currency
{
    public class LevelProgressHandler
    {
        public void SaveLastCompletedLevel(int id)
        {
            PlayerPrefs.SetInt(ProgressSaveKeys.SAVED_LEVEL, id);
        }
        
        public int GetLastCompletedLevel()
        {
            return PlayerPrefs.GetInt(ProgressSaveKeys.SAVED_LEVEL);
        }

        public bool IsLevelCompleted(int id)
        {
           return id <= GetLastCompletedLevel();
        }
    }
}
