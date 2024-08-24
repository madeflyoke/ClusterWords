using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants
{
    public class LoadingScreenDialog : Dialog
    {
        [SerializeField] private Image _floatingImage;
        [SerializeField] private float _xOffsetSpeed = 0.04f;
        
        private void Update()
        {
           _floatingImage.material.mainTextureOffset += Vector2.left * Time.deltaTime * _xOffsetSpeed;
        }
    }
}
