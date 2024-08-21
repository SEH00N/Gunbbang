using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace GB.Characters
{
    public class CharacterController : NetworkBehaviour
    {
        [SerializeField] List<CharacterComponent> moduleList = new List<CharacterComponent>();
        [SerializeField] List<CharacterComponent> componentList = new List<CharacterComponent>();

        private Dictionary<Type, CharacterComponent> components = new Dictionary<Type, CharacterComponent>();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Init();
        }

        protected virtual void Init()
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

        public T GetCharacterComponent<T>() where T : CharacterComponent => components[typeof(T)] as T;
    }
}
