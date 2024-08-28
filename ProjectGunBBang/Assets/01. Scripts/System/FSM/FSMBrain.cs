using System;
using System.Collections.Generic;
using UnityEngine;

namespace GB.FSM
{
    public class FSMBrain : MonoBehaviour
    {
        [SerializeField] List<FSMParamSO> fsmParams = null;

        private FSMState currentState;
        private Dictionary<Type, FSMParamSO> fsmParamDictionary = null;

        public void Init()
        {
            fsmParamDictionary = new Dictionary<Type, FSMParamSO>();
            fsmParams.ForEach(i => {
                Type type = i.GetType();
                if (fsmParamDictionary.ContainsKey(type))
                    return;
                fsmParamDictionary.Add(type, ScriptableObject.Instantiate(i));
            });

            Transform stateContainer = transform.Find("States");
            foreach (Transform stateTrm in stateContainer)
            {
                if (stateTrm.TryGetComponent<FSMState>(out FSMState state))
                    state.Init(this);
            }
        }

        public void UpdateFSM()
        {
            currentState?.UpdateState();
        }

        public T GetFSMParam<T>() where T : FSMParamSO
        {
            return fsmParamDictionary[typeof(T)] as T;
        }

        public void ChangeState(FSMState state)
        {
            currentState?.ExitState();
            currentState = state;
            currentState.EnterState();
        }
    }
}
