using TestGame.ProjectSystems;
using TestGame.Scenes.Base;
using Zenject;

namespace TestGame.Scenes
{
    public class SplashScene : SceneView
    {
        [Inject]
        public void Construct(SoundSystem soundSystem, UISystem uiSystem)
        {
            base.Construct(soundSystem, uiSystem);
        }
    }
}