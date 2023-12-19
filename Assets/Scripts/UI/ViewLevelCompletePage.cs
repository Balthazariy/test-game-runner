using TestGame.Gameplay.SceneSystems;
using TestGame.UI.Views.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestGame.UI
{
    public class ViewLevelCompletePage : View
    {
        [SerializeField] private Button _nextButton;

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

            _nextButton.onClick.AddListener(NextButtonOnClickHandler);
        }

        public override void Hide()
        {
            base.Hide();

            _nextButton.onClick.RemoveListener(NextButtonOnClickHandler);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public void NextButtonOnClickHandler()
        {
            _gameStateSystem.ChangeGameState(Settings.GameStates.Idle);
            _levelSystem.LoadNextLevel();
            Hide();
        }
    }
}