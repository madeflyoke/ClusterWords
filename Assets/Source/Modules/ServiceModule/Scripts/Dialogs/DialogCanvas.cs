using Source.Modules.ServiceModule.Scripts.Dialogs.Visual;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Dialogs
{
    public class DialogCanvas : MonoBehaviour
    {
        [field: SerializeField] public TransitionAnimation TransitionAnimationComponent { get; private set; }
        
        public void Initialize()
        {
            TransitionAnimationComponent.Initialize();
            TransitionAnimationComponent.InstantOpen();
        }
    }
}
