using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.GameLogicModule.Scripts.Words.WordCells
{
    public class WordCellView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _activeColor;
        private Color _defaultColor = Color.white;
        
        public void Active()
        {
            _image.color = _activeColor;
        }

        public void DeActive()
        {
            _image.color = _defaultColor;
        }
    }
}