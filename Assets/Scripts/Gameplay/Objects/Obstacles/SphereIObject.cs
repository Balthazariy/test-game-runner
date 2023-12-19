using DG.Tweening;
using TestGame.Gameplay.Objects.Base;
using TestGame.Gameplay.SceneSystems;
using TestGame.ProjectSystems;
using UnityEngine;

namespace TestGame.Gameplay.Objects.Obstacles
{
    public class SphereIObject : InteractableObject
    {
        [SerializeField] private float _initialPosition = -2.0f;

        private Sequence _sequence;

        public override void Construct(SoundSystem soundSystem, GameStateSystem gameStateSystem)
        {
            base.Construct(soundSystem, gameStateSystem);
        }

        public override void Init()
        {
            base.Init();

            _selfObject.transform.localPosition = new Vector3(transform.position.x, transform.position.y, _initialPosition);

            _sequence = DOTween.Sequence();

            _sequence.AppendInterval(Random.Range(0.5f, 2f)).
                Append(_selfObject.transform.DOMoveZ(2.0f, 1.5f)).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

            //InternalTools.DoActionDelayed(() => _sequence.Append(_selfObject.transform.DOMoveZ(2.0f, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo)),
            //    Random.Range(0.5f, 2f));
        }

        private void OnDestroy()
        {
            _sequence.Kill();
            _sequence = null;
        }

        protected override void Collect()
        {
            base.Collect();
            _gameStateSystem.ChangeGameState(Settings.GameStates.Dead);
        }
    }
}