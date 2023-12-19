using TestGame.Gameplay.SceneSystems;
using TestGame.Settings;
using TestGame.UI.Views.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestGame.UI
{
    public class ViewGameOver : View
    {
        [SerializeField] private Button _restartButton;

        private LevelSystem _levelSystem;
        private GameStateSystem _gameStateSystem;

        [Inject]
        public void Construct(LevelSystem levelSystem, GameStateSystem gameStateSystem)
        {
            _levelSystem = levelSystem;
            _gameStateSystem = gameStateSystem;
        }

        public override void Show()
        {
            base.Show();

            _restartButton.onClick.AddListener(RestartButtonOnClickHandler);
        }

        public override void Hide()
        {
            base.Hide();

            _restartButton.onClick.RemoveListener(RestartButtonOnClickHandler);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public void RestartButtonOnClickHandler()
        {
            _gameStateSystem.ChangeGameState(GameStates.Idle);
            _levelSystem.RestartLevel();
            Hide();
        }
    }
}