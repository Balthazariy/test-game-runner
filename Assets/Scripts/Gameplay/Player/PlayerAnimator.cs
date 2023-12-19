using TestGame.Settings;
using UnityEngine;

namespace TestGame.Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            EventBus.OnGameStateChangedEvent += OnGameStateChangedEventHandler;
        }

        private void OnDisable()
        {
            EventBus.OnGameStateChangedEvent -= OnGameStateChangedEventHandler;
        }

        private void OnGameStateChangedEventHandler(GameStates gameStates)
        {
            switch (gameStates)
            {
                case GameStates.Idle:
                case GameStates.Dead:
                    _animator.Play("Idle", -1, 0);
                    break;

                case GameStates.Run:
                    _animator.Play("Run", -1, 0);
                    break;

                case GameStates.Complete:
                    _animator.Play("Complete", -1, 0);
                    break;
            }
        }
    }
}