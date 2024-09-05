using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Player.Settings
{
    public class SettingsHandler
    {
        public void SaveMusicStatus(bool active)
        {
            PlayerPrefs.SetInt(ProgressSaveKeys.SETTINGS_MUSIC_STATUS, active?1:0);
            PlayerPrefs.Save();
        }

        public void SaveSoundStatus(bool active)
        {
            PlayerPrefs.SetInt(ProgressSaveKeys.SETTINGS_SOUND_STATUS, active?1:0);
            PlayerPrefs.Save();
        }
        
        public void GetAudioStatus(out bool musicActive, out bool soundActive)
        {
            musicActive = PlayerPrefs.HasKey(ProgressSaveKeys.SETTINGS_MUSIC_STATUS)==false 
                          || PlayerPrefs.GetInt(ProgressSaveKeys.SETTINGS_MUSIC_STATUS) == 1;
            soundActive = PlayerPrefs.HasKey(ProgressSaveKeys.SETTINGS_SOUND_STATUS)==false 
                || PlayerPrefs.GetInt(ProgressSaveKeys.SETTINGS_SOUND_STATUS) == 1;
        }
    }
}
