using TestGame.Gameplay.SceneSystems;
using Zenject;

namespace TestGame.SceneEntries
{
    public class GameEntry : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelSystem>().FromComponentInHierarchy().AsCached();
            Container.Bind<GameStateSystem>().FromComponentInHierarchy().AsCached();
        }
    }
}