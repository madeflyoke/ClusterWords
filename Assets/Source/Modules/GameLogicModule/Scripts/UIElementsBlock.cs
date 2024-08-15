using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.GameLogicModule.Scripts
{
    public class UIElementsBlock : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup _layoutGroup;
        
        public int Capacity { get; private set; }
        
        public UIElementsBlock Initialize(int capacity, float customSpacing = 12f)
        {
           Capacity = capacity;
           _layoutGroup.spacing = customSpacing;
           return this;
        }
    }
}
