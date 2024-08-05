using System;
using UnityEngine;

namespace GB.Characters
{
    public abstract class CharacterComponent : MonoBehaviour
    {
        public virtual Type ComponentType => GetType();
        protected CharacterController controller = null;

        public virtual void Init(CharacterController controller)
        {
            this.controller = controller;
        }
    }
}
