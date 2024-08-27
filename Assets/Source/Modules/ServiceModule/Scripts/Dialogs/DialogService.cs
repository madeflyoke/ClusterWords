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
        
        private DialogCanvas _dialogCanvas;
        private List<Dialog> _activeDialogs;

        public UniTask Initialize(CancellationTokenSource cts)
        {
            _dialogCanvas = Object.Instantiate(Resources.Load<DialogCanvas>(DIALOG_CANVAS_PATH));
            _activeDialogs = new List<Dialog>();
            return UniTask.CompletedTask;
        }
        
        public Dialog ShowDialog<T>(bool asSingle=true, Action onComplete = null, bool asFirstChild =false) where T : Dialog
        {
            Dialog dialog = _activeDialogs.FirstOrDefault(x => x.GetType() == typeof(T));
            if (dialog==null)
            {
                Dialog dialogPrefab = Resources.Load<T>(DIALOG_RESOURCES_PATH +"/"+typeof(T).Name);
                dialog = Object.FindObjectOfType<SceneContext>().Container
                    .InstantiatePrefabForComponent<T>(dialogPrefab, _dialogCanvas.transform);
                if (asFirstChild)
                {
                    dialog.transform.SetAsFirstSibling();
                }
            }
       
            ShowDialogInternal(dialog, onComplete);

            if (asSingle)
            {
                HideDialogs(_activeDialogs.Where(x => x != dialog).ToList());
            }
            
            return dialog;
        }

        public void HideAllExcept<T>() where T: Dialog
        {
            var filteredDialogs = _activeDialogs.Where(x => x.GetType() != typeof(T)).ToList();
            HideDialogs(filteredDialogs);
        } 
        
        public void HideDialog<T>() where T : Dialog
        {
            var dialog = _activeDialogs.FirstOrDefault(x=>x.GetType()==typeof(T));
            if (dialog!=null)
            {
                HideDialogInternal(dialog);
            }
        }
        
        private void HideDialogs(List<Dialog> dialogsToHide)
        {
            for (var i = dialogsToHide.Count-1; i >=0; i--)
            {
                HideDialogInternal(_activeDialogs[i]);
            }
        }

        private void ShowDialogInternal(Dialog dialog, Action onComplete =null)
        {
            if (_activeDialogs.Contains(dialog))
            {
                return;
            }
            dialog.Show(onComplete);
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