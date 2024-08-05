using System;
using System.Collections.Generic;
using UnityEngine;

namespace GB.Characters
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] List<CharacterComponent> moduleList = new List<CharacterComponent>();
        [SerializeField] List<CharacterComponent> componentList = new List<CharacterComponent>();

        private Dictionary<Type, CharacterComponent> components = new Dictionary<Type, CharacterComponent>();

        private void Awake()
        {
            moduleList.ForEach(RegisterComponent);
            componentList.ForEach(RegisterComponent);
        }

        private void RegisterComponent(CharacterComponent component)
        {
            component.Init(this);
            
            Type type = component.ComponentType;
            while (true)
            {
                components.Add(type, component);
                type = type.BaseType;

                if (type == typeof(CharacterComponent))
                    break;
            }
        }
    }
}
