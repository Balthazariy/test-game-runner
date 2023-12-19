using System.Collections;
using TestGame.Settings;
using TestGame.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace TestGame.ProjectSystems
{
    public class SceneSystem : MonoBehaviour, ISystem
    {
        private AsyncOperation _currentAsycnOperation;

        private SceneNames _currentSceneName = SceneNames.Unknown;
        private SceneNames _aimedAfterLoadingSceneName = SceneNames.Unknown;

        private bool _isAutoOpen;
        private float _delayToOpenScene;

        [Inject]
        public void Construct()
        {
            Utilities.Logger.Log("SceneSystem Construct", LogTypes.Info);
        }

        public void Init()
        {
            InternalTools.DoActionDelayed(() => LoadSceneByNameWithAutoOpen(SceneNames.Splash), 0.5f);
        }

        public void OpenLoadedScene()
        {
            if (_currentAsycnOperation == null)
            {
                Utilities.Logger.Log("Try to open scene that don't loaded", LogTypes.Error);
                return;
            }

            _currentAsycnOperation.allowSceneActivation = true;

            _currentAsycnOperation = null;
        }

        public void LoadSceneByName(SceneNames sceneName, SceneNames aimedSceneNameAfterLoading = SceneNames.Unknown)
        {
            _isAutoOpen = false;
            _delayToOpenScene = 0;

            SetupSceneSettings(sceneName, aimedSceneNameAfterLoading);
        }

        public void LoadSceneByNameWithAutoOpen(SceneNames sceneName, SceneNames aimedSceneNameAfterLoading = SceneNames.Unknown,
                                                float delayToOpenScene = 0.3f)
        {
            _isAutoOpen = true;
            _delayToOpenScene = delayToOpenScene;

            SetupSceneSettings(sceneName, aimedSceneNameAfterLoading);
        }

        private void SetupSceneSettings(SceneNames sceneName, SceneNames aimedSceneNameAfterLoading = SceneNames.Unknown)
        {
            _currentSceneName = sceneName;

            _aimedAfterLoadingSceneName = aimedSceneNameAfterLoading;

            StartCoroutine(LoadScene(sceneName.ToString()));
        }

        public void LoadAimedAfterLoadingScene()
        {
            _isAutoOpen = false;
            if (_aimedAfterLoadingSceneName == SceneNames.Unknown)
            {
                Utilities.Logger.Log("Aimed scene name after loading can't be Unknown!!!", LogTypes.Warning);
                return;
            }

            StartCoroutine(LoadScene(_aimedAfterLoadingSceneName.ToString()));

            _aimedAfterLoadingSceneName = SceneNames.Unknown;
        }

        private IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            _currentAsycnOperation = asyncOperation;
            _currentAsycnOperation.allowSceneActivation = false;

            while (!_currentAsycnOperation.isDone)
            {
                // for present loading like 1-100%
                EventBus.OnSceneLoadingProgressEvent?.Invoke(_currentAsycnOperation.progress * 100f);

                if (_currentAsycnOperation.progress >= 0.9f)
                {
                    if (_isAutoOpen)
                    {
                        InternalTools.DoActionDelayed(() => _currentAsycnOperation.allowSceneActivation = true, _delayToOpenScene);
                    }

                    EventBus.OnSceneLoadedEvent?.Invoke(InternalTools.EnumFromString<SceneNames>(sceneName));
                    break;
                }

                yield return null;
            }
        }

        public SceneNames GetCurrentScene() => _currentSceneName;
    }
}