using System.Collections.Generic;
using TestGame.Gameplay.SceneSystems;
using TestGame.ProjectSystems;
using TestGame.Scenes.Base;
using TestGame.Settings;
using TestGame.UI;
using Zenject;

namespace TestGame.Scenes
{
    public class GameScene : SceneView
    {
        private List<ISystem> _systems;

        [Inject]
        public void Construct(SoundSystem soundSystem, UISystem uiSystem, LevelSystem levelSystem, GameStateSystem gameStateSystem)
        {
            base.Construct(soundSystem, uiSystem);

            _uiSystem = uiSystem;

            _systems = new List<ISystem>()
            {
                levelSystem,
                gameStateSystem,
            };

            Init();

            soundSystem.PlaySound(Sounds.GameBackground);

            EventBus.OnGameStateChangedEvent += OnGameStateChangedEventHandler;
        }

        public void Init()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].Init();
            }
        }

        private void OnGameStateChangedEventHandler(GameStates gameStates)
        {
            switch (gameStates)
            {
                case GameStates.Idle:
                    break;

                case GameStates.Run:
                    break;

                case GameStates.Dead:
                    _uiSystem.ShowView<ViewGameOver>();
                    break;

                case GameStates.Complete:
                    _uiSystem.ShowView<ViewLevelCompletePage>();
                    break;
            }
        }
    }
}