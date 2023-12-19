using System.Collections.Generic;
using TestGame.Gameplay.Objects.Base;
using TestGame.Gameplay.SceneSystems;
using TestGame.ProjectSystems;
using UnityEngine;

namespace TestGame.Gameplay.Objects
{
    public class LevelIniter : MonoBehaviour
    {
        [SerializeField] private List<InteractableObject> _collectableIObjects;
        [SerializeField] private List<InteractableObject> _obstacleIObjects;

        public void Init(SoundSystem soundSystem, GameStateSystem gameStateSystem)
        {
            InitIObjects(soundSystem, gameStateSystem);
        }

        private void InitIObjects(SoundSystem soundSystem, GameStateSystem gameStateSystem)
        {
            for (int i = 0; i < _obstacleIObjects.Count; i++)
            {
                _obstacleIObjects[i].Construct(soundSystem, gameStateSystem);
            }

            for (int i = 0; i < _obstacleIObjects.Count; i++)
            {
                _obstacleIObjects[i].Init();
            }

            for (int i = 0; i < _collectableIObjects.Count; i++)
            {
                _collectableIObjects[i].Construct(soundSystem, gameStateSystem);
            }

            for (int i = 0; i < _collectableIObjects.Count; i++)
            {
                _collectableIObjects[i].Init();
            }
        }
    }
}