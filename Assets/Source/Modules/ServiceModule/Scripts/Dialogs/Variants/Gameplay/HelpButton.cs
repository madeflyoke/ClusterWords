using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.Gameplay
{
    public class HelpButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private DialogService _dialogService;
        
        [Inject]
        public void Construct(ServicesHolder servicesHolder)
        {
            _dialogService = servicesHolder.GetService<DialogService>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(() =>
            {
                _dialogService.ShowDialog<HelpDialog>(false);
            });  
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
