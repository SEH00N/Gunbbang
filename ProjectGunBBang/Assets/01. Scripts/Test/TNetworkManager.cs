using GB.Networks;
using TMPro;
using UnityEngine;

namespace GB.Tests
{
    public class TNetworkManager : MonoBehaviour
    {
        [SerializeField] TMP_InputField joinCodeInput = null;

        private async void Start()
        {
            ClientManager.Instance.InitNetworkAsync();
            await Authenticator.DoAuthAsync();
        }

        public void StartHost()
        {
            HostManager.Instance.StartHostAsync(2);
        }

        public void StartGuest()
        {
            GuestManager.Instnace.StartGuestAsync(joinCodeInput.text);
        }
    }
}
