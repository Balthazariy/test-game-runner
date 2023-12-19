using Dreamteck.Splines;
using System.Collections.Generic;
using TestGame.Gameplay.Objects;
using TestGame.ProjectSystems;
using TestGame.Settings;
using UnityEngine;
using Zenject;

namespace TestGame.Gameplay.SceneSystems
{
    public class LevelSystem : MonoBehaviour, ISystem
    {
        [SerializeField] private List<GameObject> _levelPrefabs;

        private int _levelIndex;

        private SplineFollower _splineFollower;

        private SoundSystem _soundSystem;
        private GameStateSystem _gameStateSystem;

        private LevelIniter _currentLevel;

        [Inject]
        public void Construct(SoundSystem soundSystem, GameStateSystem gameStateSystem)
        {
            _soundSystem = soundSystem;
            _gameStateSystem = gameStateSystem;

            EventBus.OnGameStateChangedEvent += OnGameStateChangedEventHandler;
        }

        public void Init()
        {
            _levelIndex = 0;
            CreateLevel();
        }

        public void RestartLevel()
        {
            DestroyLevel();
            CreateLevel();
        }

        public void LoadNextLevel()
        {
            DestroyLevel();
            _levelIndex++;

            if (_levelIndex >= _levelPrefabs.Count)
            {
                _levelIndex = 0;
            }

            CreateLevel();
        }

        private void CreateLevel()
        {
            _currentLevel = MonoBehaviour.Instantiate(_levelPrefabs[_levelIndex]).GetComponent<LevelIniter>();
            _currentLevel.Init(_soundSystem, _gameStateSystem);
            _splineFollower = _currentLevel.transform.Find("Player").GetComponent<SplineFollower>();
            _splineFollower.follow = false;
            EventBus.OnLevelLoadedEvent?.Invoke(_levelIndex);
        }

        private void DestroyLevel()
        {
            Destroy(_currentLevel.gameObject);
            _currentLevel = null;
        }

        private void OnGameStateChangedEventHandler(GameStates gameStates)
        {
            _splineFollower.follow = gameStates == GameStates.Run;
        }
    }
}