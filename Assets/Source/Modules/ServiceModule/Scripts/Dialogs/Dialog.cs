using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs
{
    public abstract class Dialog : MonoBehaviour
    {
        public event Action<Dialog> Hidden; 
        
        protected virtual void Start()
        {
          
        }

        protected virtual void OnDestroy()
        {

        }

        public virtual void Show()
        {
           gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            Hidden?.Invoke(this);
            Destroy(gameObject);
        }
    }
}