using Unity.Netcode;

namespace GB.NetworkEvents
{
    internal class NetworkEventManager : NetworkBehaviour
    {
        private static NetworkEventManager instance = null;
        public static NetworkEventManager Instance => instance;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #region Alert
        public void AlertEvent(NetworkEventPacket packet) => AlertEventServerRpc(packet);

        [ServerRpc(RequireOwnership = false)]
        private void AlertEventServerRpc(NetworkEventPacket packet) => CallEvent(packet);
        #endregion

        #region Broadcast
        public void BroadcastEvent(NetworkEventPacket packet) => BroadcastEventServerRpc(packet);
        
        [ServerRpc(RequireOwnership = false)]
        private void BroadcastEventServerRpc(NetworkEventPacket packet) => BroadcastEventClientRpc(packet);
        
        [ClientRpc]
        private void BroadcastEventClientRpc(NetworkEventPacket packet) => CallEvent(packet);
        #endregion

        private void CallEvent(NetworkEventPacket packet)
        {
            NetworkEventParams eventParams = NetworkEventTable.GetEventParams(packet.ParamsID, packet.Buffer);
            INetworkEvent networkEvent = NetworkEventTable.GetEvent(packet.InstanceID, packet.EventID);
            networkEvent?.Invoke(eventParams);
        }
    }
}
