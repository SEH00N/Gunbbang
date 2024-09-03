using GB.Networks;
using GB.Scenes;
using UnityEngine;

namespace GunBBang
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            ClientManager.Instance = gameObject.AddComponent<ClientManager>();
            SceneManager.Instance = gameObject.AddComponent<SceneManager>();
        }
    }
}
