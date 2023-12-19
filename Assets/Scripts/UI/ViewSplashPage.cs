using DG.Tweening;
using TestGame.ProjectSystems;
using TestGame.UI.Views.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestGame.UI
{
    public class ViewSplashPage : View
    {
        [SerializeField] private Image _iconImage;
        private SceneSystem _sceneSystem;

        private Sequence _iconSequence;

        [Inject]
        public void Construct(SceneSystem sceneSystem)
        {
            _sceneSystem = sceneSystem;
        }

        public override void Show()
        {
            base.Show();

            _sceneSystem.LoadSceneByName(Settings.SceneNames.Game);

            _iconSequence = DOTween.Sequence();

            _iconSequence.Append(_iconImage.DOFade(1.0f, 2.0f).From(0.0f)).
                Append(_iconImage.transform.DOScale(1.4f, 3.0f)).
                OnComplete(() => _sceneSystem.OpenLoadedScene());
        }

        public override void Hide()
        {
            base.Hide();

            _iconSequence = null;
        }
    }
}