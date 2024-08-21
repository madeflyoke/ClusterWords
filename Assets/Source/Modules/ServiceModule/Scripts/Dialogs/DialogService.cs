using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Modules.ServiceModule.Scripts.S.Interfaces;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Source.Modules.ServiceModule.Scripts.Dialogs
{
    public class DialogService: IService
    {
        private const string DIALOG_RESOURCES_PATH = "Dialogs";
        private const string DIALOG_CANVAS_PATH = "Dialogs/DialogCanvas";
        
        private DialogCanvas _dialogParent;
        private List<Dialog> _activeDialogs;

        public UniTask Initialize(CancellationTokenSource cts)
        {
            _dialogParent = Object.Instantiate(Resources.Load<DialogCanvas>(DIALOG_CANVAS_PATH));
            _activeDialogs = new List<Dialog>();
            return UniTask.CompletedTask;
        }
        
        public void ShowDialog<T>() where T : Dialog
        {
            Dialog dialog = _activeDialogs.FirstOrDefault(x => x.GetType() == typeof(T));
            if (dialog==null)
            {
                Dialog dialogPrefab = Resources.Load<T>(DIALOG_RESOURCES_PATH +"/"+typeof(T).Name);
                dialog = Object.FindObjectOfType<SceneContext>().Container
                    .InstantiatePrefabForComponent<T>(dialogPrefab, _dialogParent.transform);
            }
       
            ShowDialogInternal(dialog);
        }
        
        public void HideDialog<T>() where T : Dialog
        {
            var dialog = _activeDialogs.FirstOrDefault(x=>x.GetType()==typeof(T));
            if (dialog!=null)
            {
                HideDialogInternal(dialog);
            }
        }
        
        public void ShowSingleDialog<T>() where T : Dialog
        {
            for (var i =  _activeDialogs.Count-1; i >=0; i--)
            {
                HideDialogInternal(_activeDialogs[i]);
            }

            ShowDialog<T>();
        }

        private void ShowDialogInternal(Dialog dialog)
        {
            dialog.Show();
            dialog.Hidden += OnDialogHidden;
            _activeDialogs.Add(dialog);
        }

        private void OnDialogHidden(Dialog dialog)
        {
            dialog.Hidden -= OnDialogHidden;
            _activeDialogs.Remove(dialog);
        }

        private void HideDialogInternal(Dialog dialog)
        {
            dialog.Hide();
        }
        
        public void Dispose()
        {
            
        }
    }
}