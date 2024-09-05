using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Modules.ServiceModule.Scripts.Interfaces;
using Source.Modules.ServiceModule.Scripts.Player.Level;
using Source.Modules.ServiceModule.Scripts.Player.Settings;

namespace Source.Modules.ServiceModule.Scripts.Player
{
    public class PlayerDataService : IService
    {
        public LevelProgressHandler LevelProgressHandler { get; private set; }
        public SettingsHandler SettingsHandler { get; private set; }
        
        public UniTask Initialize(CancellationTokenSource cts)
        {
            LevelProgressHandler = new LevelProgressHandler();
            SettingsHandler = new SettingsHandler();
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
