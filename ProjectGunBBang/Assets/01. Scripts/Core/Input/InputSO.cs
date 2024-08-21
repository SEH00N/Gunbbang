using UnityEngine;

namespace GB.Inputs
{
    public class InputSO : ScriptableObject
    {
        public InputType inputType;

        protected virtual void OnEnable()
        {
            Debug.Log($"Set InputSO : {inputType}");
        }
    }
}
