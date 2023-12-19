using TestGame.Gameplay.Objects.Base;
using TestGame.Gameplay.SceneSystems;
using TestGame.ProjectSystems;

namespace TestGame.Gameplay.Objects.Obstacles
{
    public class WallIObject : InteractableObject
    {
        public override void Construct(SoundSystem soundSystem, GameStateSystem gameStateSystem)
        {
            base.Construct(soundSystem, gameStateSystem);
        }

        public override void Init()
        {
            base.Init();
        }

        protected override void Collect()
        {
            base.Collect();
            _gameStateSystem.ChangeGameState(Settings.GameStates.Dead);
        }
    }
}