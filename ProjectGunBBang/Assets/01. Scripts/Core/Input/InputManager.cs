using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GB.Inputs
{
    public static class InputManager
    {
        private static Dictionary<InputType, InputActionMap> inputMapDic;
        private static InputType currentInputType;
        public static InputType CurrentInputType => currentInputType;

        private static bool enable = true;

        static InputManager()
        {
            inputMapDic = new Dictionary<InputType, InputActionMap>();
        }

        public static void RegistInputMap(InputSO inputSO, InputActionMap actionMap)
        {
            inputMapDic[inputSO.inputType] = actionMap;
            actionMap.Disable();
        }

        public static void ChangeInputMap(InputType inputType)
        {
            if (inputMapDic.ContainsKey(currentInputType))
                inputMapDic[currentInputType]?.Disable();

            currentInputType = inputType;

            if (inputMapDic.ContainsKey(currentInputType))
                inputMapDic[currentInputType]?.Enable();

            SetInputEnable(enable);

            Debug.Log($"change input map : {currentInputType}");
        }

        public static void SetInputEnable(bool value)
        {
            if(value)
                inputMapDic[currentInputType]?.Enable();
            else
                inputMapDic[currentInputType]?.Disable();

            enable = value;
        }
    }
}
