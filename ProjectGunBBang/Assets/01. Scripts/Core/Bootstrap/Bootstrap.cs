using UnityEngine;
using UnityEngine.SceneManagement;

namespace GB
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] string sceneName;

        private void Start()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
