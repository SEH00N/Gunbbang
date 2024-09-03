using System;
using System.Collections.Generic;
using GB.Extensions;
using Unity.Netcode;
using UnityEngine;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace GB.Scenes
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance = null;

        private bool callbackPostpone = false;
        private Action loadSceneCallbackBuffer = null;
        
        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += SubscribeSceneCallback;
        }

        public void LoadScene(SceneType sceneType, bool postponeOneFrame = false, Action callback = null)
            => LoadScene(sceneType.ToString(), postponeOneFrame, callback);

        public void LoadScene(string sceneName, bool postponeOneFrame = false, Action callback = null)
        {
            callbackPostpone = postponeOneFrame;
            loadSceneCallbackBuffer = callback;

            if(NetworkManager.Singleton == null || NetworkManager.Singleton.IsClient == false)
            {
                AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
                if(oper != null)
                    oper.completed += i => HandleLoadEventCompleted(sceneName, LoadSceneMode.Single, null, null);
            }
            else
            {
                if(NetworkManager.Singleton.IsHost == false)
                    return;

                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }

        private void HandleLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if(loadSceneCallbackBuffer == null)
                return;

            if(callbackPostpone)
                StartCoroutine(this.PostponeFrameCoroutine(InvokeDisposableCallback));
            else
                InvokeDisposableCallback();
        }

        private void InvokeDisposableCallback()
        {
            loadSceneCallbackBuffer?.Invoke();
            loadSceneCallbackBuffer = null;

            callbackPostpone = false;
        }
        
        private void SubscribeSceneCallback()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += HandleLoadEventCompleted;
        }
    }
}
