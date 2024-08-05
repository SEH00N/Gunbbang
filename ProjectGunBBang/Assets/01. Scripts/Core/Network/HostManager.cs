using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace GB.Networks
{
    public class HostManager
    {
        public static HostManager Instance = null;

        public event Action OnHostStartedEvent = null;
        public event Action OnHostClosedEvent = null;
        public event Action<ulong> OnClientConnectedEvent = null;
        public event Action<ulong> OnClientDisconnectEvent = null;

        public const string JOIN_CODE_KEY = "JoinCode";
        public string JoinCode { get; private set; } = "";
        public string LobbyID { get; private set; } = "";

        private Allocation room = null;

        public HostManager()
        {

        }

        public async void StartHostAsync(int maxConnections)
        {
            JoinCode = await CreateRoomAsync(maxConnections);
            LobbyID = await CreateLobbyAsync(JoinCode, maxConnections);
            StartHost();
                
            OnHostStartedEvent?.Invoke();
        }

        public void ClostHost()
        {
            NetworkManager.Singleton.Shutdown();
        }

        private async Task<string> CreateRoomAsync(int maxConnections)
        {
            try {
                room = await Relay.Instance.CreateAllocationAsync(maxConnections);
            }
            catch(Exception err) {
                Debug.LogError(err.Message);
            }

            string joinCode = await Relay.Instance.GetJoinCodeAsync(room.AllocationId);
            GUIUtility.systemCopyBuffer = joinCode;

            Debug.Log($"Room Created | Code : {joinCode}");

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            RelayServerData relayServer = new RelayServerData(room, "dtls");
            transport.SetRelayServerData(relayServer);

            return joinCode;
        }

        private async Task<string> CreateLobbyAsync(string joinCode, int maxConnections)
        {
            string lobbyID = "";
            try {
                CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
                lobbyOptions.IsPrivate = false;
                lobbyOptions.Data = new Dictionary<string, DataObject>() {
                    [JOIN_CODE_KEY] = new DataObject(DataObject.VisibilityOptions.Public, joinCode)
                };

                Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("Test", maxConnections, lobbyOptions);
                lobbyID = lobby.Id;
            }
            catch(Exception err) {
                Debug.LogError(err.Message);
            }

            return lobbyID;
        }

        private void StartHost()
        {
            bool result = NetworkManager.Singleton.StartHost();
            if(result == false)
                return;

            NetworkManager.Singleton.ConnectionApprovalCallback += HandleConnectionApproval;

            NetworkManager.Singleton.OnClientStopped += HandleClientStopped;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        private void HandleConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            response.CreatePlayerObject = false;
        }

        private void HandleClientStopped(bool isHosted)
        {
            if(isHosted == false)
                return;

            OnHostClosedEvent?.Invoke();
        }

        private void HandleClientConnected(ulong clientID)
        {
            Debug.Log($"Client Connected | Client ID : {clientID}");
            OnClientConnectedEvent?.Invoke(clientID);
        }

        private void HandleClientDisconnect(ulong clientID)
        {
            Debug.Log($"Client Disconnected | Client ID : {clientID}");
            OnClientDisconnectEvent?.Invoke(clientID);
        }
    }
}
