using System.Collections.Generic;
using TestGame.ProjectSystems;
using UnityEngine;
using Zenject;

namespace TestGame.SceneEntries
{
    public class InitEntry : MonoBehaviour
    {
        private List<ISystem> _projectSystem;

        [Inject]
        public void Construct(LoadObjectsSystem loadObjectsSystem,
            SceneSystem sceneSystem,
            UISystem uiSystem,
            SoundSystem soundSystem)
        {
            _projectSystem = new List<ISystem>()
            {
                loadObjectsSystem,
                soundSystem,
                uiSystem,
                sceneSystem,
            };

            Init();
        }

        public void Init()
        {
            for (int i = 0; i < _projectSystem.Count; i++)
            {
                _projectSystem[i].Init();
            }
        }
    }
}