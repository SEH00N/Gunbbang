using GB.Scenes;
using UnityEngine;

namespace GB
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] SceneType sceneType = SceneType.Intro;

        private void Start()
        {
            SceneManager.Instance.LoadScene(sceneType);
        }
    }
}
