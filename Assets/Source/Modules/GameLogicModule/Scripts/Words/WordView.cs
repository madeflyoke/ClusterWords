using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordView : MonoBehaviour
    {
        [SerializeField] private GameObject _completedWordBg;

        public void SetViewState(bool isWordCompleted)
        {
            _completedWordBg.SetActive(isWordCompleted);
        }
    }
    
}
