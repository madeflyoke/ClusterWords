using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts
{
    public class UIElementsBlock : MonoBehaviour
    {
        public int Capacity { get; private set; }
        
        public UIElementsBlock Initialize(int capacity)
        {
           Capacity = capacity;
           return this;
        }
    }
}
