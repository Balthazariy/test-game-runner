using TestGame.ProjectSystems;
using UnityEngine;
using UnityEngine.UI;

namespace TestGame.UI.Views.Base
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class View : MonoBehaviour
    {
        private SoundSystem _soundSystem;

        private GameObject _selfObject;
        private Canvas _canvas;

        public void ForceActiveGameObject() => _selfObject.SetActive(true);

        public virtual void Construct(SoundSystem soundSystem)
        {
            _soundSystem = soundSystem;

            Init();
        }

        private void Init()
        {
            _selfObject = this.gameObject;

            if (!_selfObject.activeInHierarchy)
            {
                ForceActiveGameObject();
            }

            _canvas = GetComponent<Canvas>();
            _canvas.vertexColorAlwaysGammaSpace = true;
        }

        public virtual void Show()
        {
            if (_canvas.enabled)
            {
                return;
            }

            // TODO - maybe bad way to show views, use instead _canvas.enable
            //if (_selfObject.activeInHierarchy)
            //{
            //    return;
            //}

            if (_canvas == null)
            {
                return;
            }

            _canvas.enabled = true;
            //_selfObject.gameObject.SetActive(true);

            _soundSystem.PlaySound(Settings.Sounds.ShowView);
        }

        public virtual void Hide()
        {
            if (!_canvas.enabled)
            {
                return;
            }

            // TODO - maybe bad way to hide views, use instead _canvas.enable
            //if (!_selfObject.activeInHierarchy)
            //{
            //    return;
            //}

            if (_canvas == null)
            {
                return;
            }

            _canvas.enabled = false;
            //_selfObject.gameObject.SetActive(false);

            _soundSystem.PlaySound(Settings.Sounds.HideView);
        }

        public virtual void Dispose()
        {
            _canvas = null;
            _soundSystem = null;
        }
    }
}