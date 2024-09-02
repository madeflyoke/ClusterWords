using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Progress.Currency
{
    public class LevelProgressHandler
    {
        public void SaveLastCompletedLevel(int id)
        {
            if (id>GetLastCompletedLevel())
            {
                PlayerPrefs.SetInt(ProgressSaveKeys.SAVED_LEVEL, id);
                PlayerPrefs.Save();
            }
        }
        
        public int GetLastCompletedLevel()
        {
            return PlayerPrefs.HasKey(ProgressSaveKeys.SAVED_LEVEL)? PlayerPrefs.GetInt(ProgressSaveKeys.SAVED_LEVEL) : -1;
        }

        public bool IsLevelCompleted(int id)
        {
           return id <= GetLastCompletedLevel();
        }
    }
}
