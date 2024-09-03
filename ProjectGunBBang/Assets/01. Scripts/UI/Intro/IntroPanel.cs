using GB.Scenes;
using UnityEngine;

namespace GB.UI.Intros
{
    public class IntroPanel : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.Instance.LoadScene(SceneType.Lobby);
        }
    }
}
