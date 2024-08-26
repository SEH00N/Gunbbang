using UnityEngine;
using UnityEngine.Events;

namespace GB.FSM
{
    public class FSMAction : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onStateEnterEvent = null;
        [SerializeField] protected UnityEvent onStateExitEvent = null;

        protected FSMBrain brain;
        protected FSMState state;

        public virtual void Init(FSMBrain brain, FSMState state)
        {
            this.brain = brain;
            this.state = state;
        }

        public virtual void EnterState() { onStateEnterEvent?.Invoke(); }
        public virtual void UpdateState() {}
        public virtual void ExitState() { onStateEnterEvent?.Invoke(); }
    }
}