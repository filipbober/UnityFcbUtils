using UnityEngine;
using UnityEngine.SceneManagement;

namespace FcbUtils.LevelManagement
{
    public sealed class MainController : MonoBehaviour
    {
        public enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count };

        public static MainController Instance { get; private set; }

        public SceneState CurrentState { get; private set; }

        public float SceneLoadProgress => _sceneLoadTask?.progress ?? 1;

        //private enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count };
        private delegate void UpdateDelegate();

        private string _currentSceneName;
        private string _nextSceneName;
        private AsyncOperation _resourceUnloadTask;
        private AsyncOperation _sceneLoadTask;
        private UpdateDelegate[] _updateDelegates;

        public static void SwitchScene(string nextSceneName)
        {
            //// Reset time on scene change
            //Time.timeScale = 1f;

            if (Instance != null)
            {
                if (Instance._currentSceneName != nextSceneName)
                {
                    Instance._nextSceneName = nextSceneName;
                }
            }
        }

        public static void ReloadScene()
        {
            if (Instance != null)
            {
                var nextSceneName = Instance._currentSceneName;
                Instance._currentSceneName = Instance._currentSceneName + "Old";
                SwitchScene(nextSceneName);
            }
        }

        private void Awake()
        {
            // MainController should be kept alive between scene changes
            DontDestroyOnLoad(gameObject);

            // Setup the singleton instance
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            // Setup the array of updateDelegates
            _updateDelegates = new UpdateDelegate[(int)SceneState.Count];

            // Set each update delegate
            _updateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
            _updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
            _updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
            _updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
            _updateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
            _updateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
            _updateDelegates[(int)SceneState.Run] = UpdateSceneRun;

            // _currentSceneName = Application.loadedLevelName;
            _currentSceneName = SceneManager.GetActiveScene().name;

            _nextSceneName = _currentSceneName;
            //Debug.Log("Loaded scene name = " + _nextSceneName);
            CurrentState = SceneState.Run;
        }

        private void OnDestroy()
        {
            // Clean up all the updateDelegates
            if (_updateDelegates != null)           // If array itself is not null
            {
                for (int i = 0; i < (int)SceneState.Count; i++)
                {
                    _updateDelegates[i] = null;
                }

                _updateDelegates = null;            // Set the array itself to null
            }

            // Clean up the singleton instance
            if (Instance != null)
            {
                Instance = null;
            }
        }

        private void Update()
        {
            _updateDelegates[(int)CurrentState]?.Invoke();
        }

        // Reset the new scene controller to start cascade of loading
        private void UpdateSceneReset()
        {
            //Debug.Log("UpdateSceneReset");
            // Run a gc pass
            System.GC.Collect();
            CurrentState = SceneState.Preload;
        }

        // Handle anything that need to happen before loading
        private void UpdateScenePreload()
        {
            _sceneLoadTask = SceneManager.LoadSceneAsync(_nextSceneName);

            CurrentState = SceneState.Load;
        }

        // Show the loading screen until it's loaded
        private void UpdateSceneLoad()
        {
            //Debug.Log("UpdateSceneLoad");
            // Done loading?
            if (_sceneLoadTask.isDone)
            {
                CurrentState = SceneState.Unload;
                //Debug.Log("Scene is loaded!");
            }
            else
            {
                // Update scene loading progress - like scene lading progress bar one the screen.
                Debug.Log("Loading progress = " + _sceneLoadTask.progress);
            }
        }

        // Clean up unused resources by unloading them
        private void UpdateSceneUnload()
        {
            //Debug.Log("UpdateSceneUnload");
            // Cleaning up resources yet?
            if (_resourceUnloadTask == null)
            {
                _resourceUnloadTask = Resources.UnloadUnusedAssets();
            }
            else
            {
                // Done cleaning up?
                if (_resourceUnloadTask.isDone)
                {
                    _resourceUnloadTask = null;
                    CurrentState = SceneState.Postload;
                }
            }
        }

        // Handle anything that needs to happen immediately after loading
        private void UpdateScenePostload()
        {
            //Debug.Log("UpdateScenePostload");
            _currentSceneName = _nextSceneName;     // For instance both scenes are Menu when menu is postloaded
            CurrentState = SceneState.Ready;
        }

        private void UpdateSceneReady()
        {
            //Debug.Log("UpdateSceneReady");
            // Run a gc pass
            System.GC.Collect();                    // Optional step
            CurrentState = SceneState.Run;
        }

        private void UpdateSceneRun()
        {
            //Debug.Log("UpdateSceneRun -> " + _currentSceneName);
            if (_currentSceneName != _nextSceneName)
            {
                CurrentState = SceneState.Reset;
            }
        }

    }
}
