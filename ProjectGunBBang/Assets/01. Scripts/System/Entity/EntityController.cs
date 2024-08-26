using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace GB.Entities
{
    public class EntityController : NetworkBehaviour
    {
        [SerializeField] List<EntityComponent> moduleList = new List<EntityComponent>();
        [SerializeField] List<EntityComponent> componentList = new List<EntityComponent>();

        private Dictionary<Type, EntityComponent> components = new Dictionary<Type, EntityComponent>();

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

        private void RegisterComponent(EntityComponent component)
        {
            component.Init(this);
            
            Type type = component.ComponentType;
            while (true)
            {
                components.Add(type, component);
                type = type.BaseType;

                if (type == typeof(EntityComponent))
                    break;
            }
        }

        public T GetEntityComponent<T>() where T : EntityComponent => components[typeof(T)] as T;
    }
}
