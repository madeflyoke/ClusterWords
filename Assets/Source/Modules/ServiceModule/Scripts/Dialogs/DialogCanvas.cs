using System;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Dialogs
{
    public class DialogCanvas : MonoBehaviour
    {
        private static DialogCanvas _instance;
        
        private void Awake()
        {
            if (_instance!=null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
        }
    }
}
