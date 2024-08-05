using GB.Networks;
using UnityEngine;

namespace GunBBang
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            ClientManager.Instance = gameObject.AddComponent<ClientManager>();
        }
    }
}
