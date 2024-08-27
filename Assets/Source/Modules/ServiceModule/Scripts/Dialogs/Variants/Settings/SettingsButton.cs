using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.Settings
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private DialogService _dialogService;
        
        [Inject]
        public void Construct(ServicesHolder servicesHolder)
        {
            _dialogService = servicesHolder.GetService<DialogService>();
        }
        
        private void Start()
        {
            _button.onClick.AddListener(OnSettingButtonClick);
        }
        
        private void OnSettingButtonClick()
        {
            _dialogService.ShowDialog<SettingsDialog>(false);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnSettingButtonClick);
        }
    }
}
