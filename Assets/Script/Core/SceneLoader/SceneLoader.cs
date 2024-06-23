using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Utility;
using System.Collections.Generic;

namespace Core
{
    public static class SceneLoader
    {
        static int fadeIn = Animator.StringToHash("FadeIn");
        static int fadeOut = Animator.StringToHash("FadeOut");

        // Start the loading of a new scene
        public static event Action OnLoadingStarted;
        // Loading of the scene complete
        public static event Action OnLoadingCompleted;
        // The old scene is unloaded
        public static event Action<int> OnSceneUnloaded;

        public static async UniTask LoadScene(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            OnLoadingStarted?.Invoke();
            await LoadLoadingSceneAndAnimate();

            var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                await UniTask.Yield();
            }
            await UnloadOtherScenesAsync(sceneIndex);

            await FadeOutAndUnloadLoadingScene();
            OnLoadingCompleted?.Invoke();
        }

        /// <summary>
        /// Load two scenes together
        /// </summary>
        /// <param name="mainScene"></param>
        /// <param name="additiveScene"></param>
        /// <returns></returns>
        public static async UniTask LoadScenes(int mainScene, int additiveScene)
        {
            OnLoadingStarted?.Invoke();
            await LoadLoadingSceneAndAnimate();

            var asyncLoadMain = SceneManager.LoadSceneAsync(mainScene, LoadSceneMode.Additive);
            while (!asyncLoadMain.isDone)
            {
                await UniTask.Yield();
            }

            await UnloadOtherScenesAsync(mainScene);

            var asyncLoadAdditive = SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);
            while (!asyncLoadAdditive.isDone)
            {
                await UniTask.Yield();
            }

            await FadeOutAndUnloadLoadingScene();
            OnLoadingCompleted?.Invoke();
        }

        /// <summary>
        /// Swap current level with another
        /// </summary>
        /// <param name="currentLevelName"></param>
        /// <param name="newLevelName"></param>
        /// <returns></returns>
        public static async UniTask SwapLevel(int currentLevelName, int newLevelName)
        {
            OnLoadingStarted?.Invoke();
            await LoadLoadingSceneAndAnimate();

            var asyncLoad = SceneManager.LoadSceneAsync(newLevelName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                await UniTask.Yield();
            }

            await SceneManager.UnloadSceneAsync(currentLevelName);
            OnSceneUnloaded?.Invoke(currentLevelName);

            await FadeOutAndUnloadLoadingScene();
            OnLoadingCompleted?.Invoke();
        }

        /// <summary>
        /// Handle fade in loading scene
        /// </summary>
        /// <returns></returns>
        private static async UniTask LoadLoadingSceneAndAnimate()
        {
            await SceneManager.LoadSceneAsync((int)SceneName.Loading, LoadSceneMode.Additive);
            GameObject loadingScreen = GameObject.FindWithTag("LoadingScreen");
            Debug.Log(loadingScreen);
            Animator loadingScreenAnimator;
            if (loadingScreen.TryGetComponent<Animator>(out loadingScreenAnimator))
            {
                loadingScreenAnimator.SetTrigger(fadeIn);
                await UniTask.Delay((int)(loadingScreenAnimator.GetCurrentAnimatorStateInfo(0).length * 1000));
            }
            else
            {
                Debug.LogError("Loading screen missing animator");
            }
        }

        /// <summary>
        /// Handle fade out
        /// </summary>
        /// <returns></returns>
        private static async UniTask FadeOutAndUnloadLoadingScene()
        {
            GameObject loadingScreen = GameObject.FindWithTag("LoadingScreen");
            Animator loadingScreenAnimator;
            Debug.Log(loadingScreen);
            if (loadingScreen.TryGetComponent<Animator>(out loadingScreenAnimator))
            {
                loadingScreenAnimator.SetTrigger(fadeOut);
                await UniTask.Delay((int)(loadingScreenAnimator.GetCurrentAnimatorStateInfo(0).length * 1000));
                await SceneManager.UnloadSceneAsync((int)SceneName.Loading);
            }
            else
            {
                Debug.LogError("Loading screen missing animator");
            }
        }

        /// <summary>
        /// Unloads all scenes except the loading scene and the specified scene.
        /// </summary>
        /// <param name="keepSceneIndex">The index of the scene to keep.</param>
        /// <returns></returns>
        private static async UniTask UnloadOtherScenesAsync(int keepSceneIndex)
        {
            List<int> scenesToUnload = new List<int>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex != 0 && scene.buildIndex != keepSceneIndex)
                {
                    scenesToUnload.Add(scene.buildIndex);
                }
            }

            foreach (int sceneIndex in scenesToUnload)
            {
                await SceneManager.UnloadSceneAsync(sceneIndex);
            }
        }
    }
}

