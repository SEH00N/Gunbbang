using Unity.Services.Core;
using UnityEngine;

namespace GB.Networks
{
    /// <summary>
    /// this is GB.NetworkManager
    /// </summary>
    public class ClientManager : MonoBehaviour
    {
        public static ClientManager Instance = null;

        private void Awake()
        {
            HostManager.Instance = new HostManager();
            GuestManager.Instnace = new GuestManager();
        }

        // Call this method when game starts
        public async void InitNetworkAsync()
        {
            await UnityServices.InitializeAsync();
        }
    }
}
