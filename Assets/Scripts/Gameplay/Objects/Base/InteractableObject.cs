using TestGame.Gameplay.SceneSystems;
using TestGame.ProjectSystems;
using UnityEngine;

namespace TestGame.Gameplay.Objects.Base
{
    public class InteractableObject : MonoBehaviour
    {
        protected GameObject _selfObject;
        protected GameObject _modelObject;

        protected GameStateSystem _gameStateSystem;
        protected SoundSystem _soundSystem;

        public virtual void Construct(SoundSystem soundSystem, GameStateSystem gameStateSystem)
        {
            _soundSystem = soundSystem;
            _gameStateSystem = gameStateSystem;
        }

        public virtual void Init()
        {
            _selfObject = gameObject;
            _modelObject = _selfObject.transform.Find("Model").gameObject;
        }

        protected virtual void Collect()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            Collect();
        }
    }
}