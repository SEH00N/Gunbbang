using System;
using UnityEngine;

namespace GB.Entities
{
    public abstract class EntityComponent : MonoBehaviour
    {
        public virtual Type ComponentType => GetType();
        protected EntityController controller = null;

        public bool Active { get; protected set; } = false;
        public bool IsOwner => controller.IsOwner;

        public virtual void Init(EntityController controller)
        {
            this.controller = controller;
            Active = true;
        }

        public virtual void Release()
        {
            
        }
    }
}
