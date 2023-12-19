using TestGame.ProjectSystems;
using TestGame.Settings;
using UnityEngine;
using Zenject;

namespace TestGame.Gameplay.SceneSystems
{
    public class GameStateSystem : MonoBehaviour, ISystem
    {
        private GameStates _currentGameState;

        [Inject]
        public void Construct()
        {
        }

        public void Init()
        {
            ChangeGameState(GameStates.Idle);
        }

        public void ChangeGameState(GameStates state)
        {
            if (_currentGameState == state)
            {
                return;
            }

            _currentGameState = state;
            EventBus.OnGameStateChangedEvent?.Invoke(_currentGameState);

            //if (_currentGameState == GameStates.Dead || _currentGameState == GameStates.Complete)
            //{
            //    EventBus.OnGamePausedEvent?.Invoke(true);
            //}
        }
    }
}