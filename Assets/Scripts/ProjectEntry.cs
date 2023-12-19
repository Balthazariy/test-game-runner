using TestGame.ProjectSystems;
using UnityEngine.SceneManagement;
using Zenject;

namespace TestGame.ProjectEntry
{
    public class ProjectEntry : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadObjectsSystem>().FromComponentInHierarchy().AsCached();
            Container.Bind<SoundSystem>().FromComponentInHierarchy().AsCached();
            Container.Bind<UISystem>().FromComponentInHierarchy().AsCached();
            Container.Bind<SceneSystem>().FromComponentInHierarchy().AsCached();

            SceneManager.LoadScene("Init");
        }
    }
}