using TestGame.Settings;
using UnityEngine;

namespace TestGame.Gameplay.Objects
{
    public class Finish : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            EventBus.OnGameStateChangedEvent?.Invoke(GameStates.Complete);
        }
    }
}