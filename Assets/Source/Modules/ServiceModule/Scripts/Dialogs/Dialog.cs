using System;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Dialogs
{
    public abstract class Dialog : MonoBehaviour
    {
        public event Action<Dialog> Hidden;
        protected DialogCanvas DialogCanvas;
        
        public void Initialize(DialogCanvas dialogCanvas)
        {
            DialogCanvas = dialogCanvas;
        }

        protected virtual void Start()
        {
          
        }

        protected virtual void OnDestroy()
        {

        }

        public virtual void Show(Action onComplete = null)
        {
           gameObject.SetActive(true);
           onComplete?.Invoke();
        }

        public virtual void Hide()
        {
            Hidden?.Invoke(this);
            Destroy(gameObject);
        }
    }
}