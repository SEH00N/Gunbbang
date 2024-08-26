using System.Collections.Generic;
using UnityEngine;

namespace GB.FSM
{
    public class FSMTransition : MonoBehaviour
    {
        [SerializeField] FSMState targetState = null;
        public FSMState TargetState => targetState;

        private List<FSMDecision> decisions = null;

        public void Init(FSMBrain brain, FSMState state)
        {
            decisions = new List<FSMDecision>();
            GetComponents<FSMDecision>(decisions);
            decisions.ForEach(i => i.Init(brain, state));
        }

        public void EnterState()
        {
            decisions.ForEach(i => i.EnterState());
        }

        public void ExitState()
        {
            decisions.ForEach(i => i.ExitState());
        }

        public bool CheckDecisions()
        {
            bool condition = false;

            foreach(FSMDecision decision in decisions)
            {
                condition = decision.MakeDecision();
                if(decision.IsReverse)
                    condition = !condition;
                if(condition == false)
                    break;
            }

            return condition;
        }
    }
}
