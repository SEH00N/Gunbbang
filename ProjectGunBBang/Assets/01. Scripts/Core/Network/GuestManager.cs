using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace GB.Networks
{
    public class GuestManager
    {
        public static GuestManager Instnace = null;

        public event Action OnClientStartedEvent = null;
        public event Action OnClientClosedEvent = null;

        private JoinAllocation room = null;

        public GuestManager()
        {
            
        }
        
        public async void StartGuestAsync(string roomCode)
        {
            await JoinRoomAsync(roomCode);
            StartGuest();

            OnClientStartedEvent?.Invoke();
        }

        private async Task JoinRoomAsync(string roomCode)
        {
            try {
                room = await Relay.Instance.JoinAllocationAsync(roomCode);
            }
            catch(Exception err) {
                Debug.LogError(err.Message);
                return;
            }

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            RelayServerData relayServer = new RelayServerData(room, "dtls");
            transport.SetRelayServerData(relayServer);
        }

        private void StartGuest()
        {
            NetworkManager.Singleton.StartClient();
            
            NetworkManager.Singleton.OnClientStopped += HandleClientStopped;
        }

        private void HandleClientStopped(bool isHosted)
        {
            if(isHosted)
                return;

            OnClientClosedEvent?.Invoke();
        }
    }
}
