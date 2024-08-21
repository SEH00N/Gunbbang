using System;
using UnityEngine;

namespace GB.Characters
{
    public abstract class CharacterComponent : MonoBehaviour
    {
        public virtual Type ComponentType => GetType();
        protected CharacterController controller = null;

        public bool Active { get; protected set; } = false;
        public bool IsOwner => controller.IsOwner;

        public virtual void Init(CharacterController controller)
        {
            this.controller = controller;
            Active = true;
        }

        public virtual void Release()
        {
            
        }
    }
}
