using System;
using GB.NetworkEvents;
using System.Collections.Generic;
using Unity.Netcode;

namespace GB.NetworkStates
{
    [Serializable]
    public class NetworkState<TEnum> where TEnum : struct, Enum
    {
        public NetworkEvent<IntParams, int> OnStateSyncedEvent = new NetworkEvent<IntParams, int>("StateSynced");
        public NetworkEvent<StateParams> OnStateChangedEvent = new NetworkEvent<StateParams>("StateChanged");
        
        private TEnum state;
        public TEnum State => state;

        private Dictionary<ulong, TEnum> stateTable = new Dictionary<ulong, TEnum>();
        private int stateSyncedClientCount = 0;

        private NetworkObject owner = null;

        public void Init(NetworkObject stateHolder)
        {
            owner = stateHolder;

            OnStateSyncedEvent.Register(owner);

            OnStateChangedEvent.AddListener(HandleStateChanged);
            OnStateChangedEvent.Register(owner);

            if(NetworkManager.Singleton.IsHost == false)
                return;

            foreach(ulong id in NetworkManager.Singleton.ConnectedClientsIds)
                stateTable.Add(id, default);

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        public void Release()
        {
            OnStateSyncedEvent.Unregister();
            OnStateChangedEvent.Unregister();

            if(NetworkManager.Singleton.IsHost == false)
                return;
            
            stateTable.Clear();

            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        public void ChangeState(TEnum state)
        {
            if (State.Equals(state))
                return;

            this.state = state;

            if (NetworkManager.Singleton.IsHost)
                stateSyncedClientCount = 0;

            StateParams stateParams = new StateParams(ToInt(State), owner.OwnerClientId);
            OnStateChangedEvent?.Broadcast(stateParams, false);
        }

        private void HandleStateChanged(StateParams stateParams)
        {
            if(NetworkManager.Singleton.IsHost == false)
                return;

            TEnum state = ToEnum(stateParams.State);
            ulong clientID = stateParams.ClientID;

            if (State.Equals(state)) // 호스트와 같은 스테이트로 업데이트 했다면
            {
                stateSyncedClientCount++;
            }
            else
            {
                if (State.Equals(stateTable[clientID])) // 호스트와 같은 스테이트였다가 다른 스테이트로 업데이트 했다면
                {
                    stateSyncedClientCount--;
                }
            }

            stateTable[clientID] = state;
            CheckSynced();
        }

        private void HandleClientConnect(ulong clientID)
        {
            stateTable.Add(clientID, default);
            if(stateTable[clientID].Equals(State))
                stateSyncedClientCount++;

            CheckSynced();
        }

        private void HandleClientDisconnect(ulong clientID)
        {
            if(stateTable[clientID].Equals(State)) // 접속 해제한 클라이언트가 호스트와 같은 스테이트였다면
                stateSyncedClientCount--;

            stateTable.Remove(clientID);
            CheckSynced();
        }

        private void CheckSynced()
        {
            if(stateSyncedClientCount >= stateTable.Count)
                OnStateSyncedEvent?.Broadcast(ToInt(State));
        }

        private TEnum ToEnum(int value) => (TEnum)Enum.ToObject(typeof(TEnum), value);
        private int ToInt(TEnum value) => Convert.ToInt32(value);
    }
}
