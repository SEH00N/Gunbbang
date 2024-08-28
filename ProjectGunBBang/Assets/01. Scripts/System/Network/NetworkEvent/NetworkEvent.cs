using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace GB.NetworkEvents
{
    [System.Serializable]
    public class NetworkEvent : NetworkEvent<NoneParams>
    {
        public NetworkEvent() : base() 
        {
            AddListener(CallWrapper);
        }

        public NetworkEvent(string key) : base(key) 
        {
            AddListener(CallWrapper);
        }

        private UnityAction callWrapper;

        public void Alert(bool requireOwnership = true)
        {
            NoneParams eventParams = new NoneParams();
            Alert(eventParams, requireOwnership);
        }

        public void Broadcast(bool requireOwnership = true)
        {
            NoneParams eventParams = new NoneParams();
            Broadcast(eventParams, requireOwnership);
        }

        public void AddListener(UnityAction call)
        {
            callWrapper += call;
        }

        public void RemoveListener(UnityAction call)
        {
            callWrapper -= call;
        }

        private void CallWrapper(NoneParams ignore)
        {
            callWrapper?.Invoke();
        }
    }

    [System.Serializable]
    public class NetworkEvent<T> : NetworkEvent<T, T> where T : NetworkEventParams, IConvertible<T>
    {
        public NetworkEvent() : base() { }
        public NetworkEvent(string key) : base(key) { }
    }

    [System.Serializable]
    public class NetworkEvent<T, U> : UnityEvent<U>, INetworkEvent where T : NetworkEventParams, IConvertible<U>
    {
        private NetworkObject instance = null;

        private string eventName = "";
        private ulong eventID = 0;
        ulong INetworkEvent.EventID => eventID;

        protected bool alive = false;

        public NetworkEvent() : base() { }

        public NetworkEvent(string key) : base()
        {
            Init(key);
        }

        ~NetworkEvent()
        {
            Unregister();
        }

        public void Init(string key)
        { 
            eventName = key;
            eventID = NetworkEventTable.StringToHash(key);
        }

        public void Register(NetworkObject instance)
        {
            if (alive)
                return;
            alive = true;

            this.instance = instance;
            NetworkEventTable.RegisterEvent(instance.NetworkObjectId, this);
        }

        public void Unregister()
        {
            if (alive == false)
                return;
            alive = false;

            NetworkEventTable.UnregisterEvent(instance.NetworkObjectId, this);
        }

        public virtual void Alert(T eventParams, bool requireOwnership = true)
        {
            if (Middleware(requireOwnership) == false)
                return;

            NetworkEventPacket packet = CreatePacket(eventParams);
            NetworkEventManager.Instance.AlertEvent(packet);
        }

        public virtual void Broadcast(T eventParams, bool requireOwnership = true)
        {
            if (Middleware(requireOwnership) == false)
                return;

            //Debug.Log($"Instance ID : {instance.NetworkObjectId}, Event ID : {eventID}, Event Name : {eventName}");
            NetworkEventPacket packet = CreatePacket(eventParams);
            NetworkEventManager.Instance.BroadcastEvent(packet);
        }

        private bool Middleware(bool requireOwnership)
        {
            if (instance.IsSpawned == false)
            {
                Debug.LogError("Network Object Instance Does Not Spawned");
                return false;
            }

            if (requireOwnership && instance.IsOwner == false)
            {
                Debug.LogError("Only Owner Can Broadcast Network Event");
                return false;
            }

            return true;
        }

        private NetworkEventPacket CreatePacket(T eventParams)
        {
            NetworkEventPacket packet = new NetworkEventPacket(
                instance.NetworkObjectId,
                (this as INetworkEvent).EventID,
                eventParams.GetType().ToString(),
                eventParams.Serialize()
            );

            return packet;
        }

        void INetworkEvent.Invoke(NetworkEventParams eventParams)
        {
            U convertedParams = (eventParams as T).Convert();
            Invoke(convertedParams);
        }
    }

}