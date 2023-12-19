using TestGame.Gameplay.SceneSystems;
using TestGame.ProjectSystems;
using TestGame.Settings;
using TestGame.UI.Views.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestGame.UI
{
    public class ViewGamePage : View
    {
        [SerializeField] private TextMeshProUGUI _levelCounterText;

        [SerializeField] private Button _restartButton;

        [SerializeField] private Button _playButton;

        private LevelSystem _levelSystem;
        private UISystem _uiSystem;
        private GameStateSystem _gameStateSystem;

        [Inject]
        public void Construct(LevelSystem levelSystem, UISystem uiSystem, GameStateSystem gameStateSystem)
        {
            _levelSystem = levelSystem;
            _uiSystem = uiSystem;
            _gameStateSystem = gameStateSystem;
        }

        public override void Show()
        {
            base.Show();

            _restartButton.onClick.AddListener(RestartButtonOnClickHandler);
            _playButton.onClick.AddListener(PlayButtonOnClickHandler);

            EventBus.OnGameStateChangedEvent += OnGameStateChangedEventHandler;
            EventBus.OnLevelLoadedEvent += OnLevelLoadedEventHandler;
        }

        public override void Hide()
        {
            base.Hide();

            _restartButton.onClick.RemoveListener(RestartButtonOnClickHandler);
            _playButton.onClick.RemoveListener(PlayButtonOnClickHandler);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public void UpdateLevelCounter(int value)
        {
            _levelCounterText.text = $"Level: {value.ToString()}";
        }

        public void RestartButtonOnClickHandler()
        {
            _gameStateSystem.ChangeGameState(GameStates.Idle);
            _levelSystem.RestartLevel();
        }

        public void PlayButtonOnClickHandler()
        {
            _gameStateSystem.ChangeGameState(GameStates.Run);
            _playButton.gameObject.SetActive(false);
        }

        private void OnGameStateChangedEventHandler(GameStates gameStates)
        {
            _playButton.gameObject.SetActive(gameStates == GameStates.Idle);
        }

        private void OnLevelLoadedEventHandler(int value)
        {
            UpdateLevelCounter(value);
        }
    }
}