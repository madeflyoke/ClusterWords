using Source.Modules.GameLogicModule.Scripts.Levels;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants
{
    public class GameplayDialog : Dialog
    {
        [SerializeField] private LevelLauncher _levelLauncher;

        protected override void Start()
        {
            base.Start();
            _levelLauncher.LaunchLevel(0);
        }
    }
}
