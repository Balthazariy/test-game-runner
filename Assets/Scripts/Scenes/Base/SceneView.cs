using System.Collections.Generic;
using TestGame.ProjectSystems;
using TestGame.UI.Views.Base;
using UnityEngine;

namespace TestGame.Scenes.Base
{
    public class SceneView : MonoBehaviour
    {
        [Header("Root view that never been hiden by ViewStacks")]
        [SerializeField] private View _rootView;

        [Header("View for exiting from scene show when no one View in stack")]
        [SerializeField] private View _exitView;

        [Space(4)]
        [Header("List of all view in current scene exept root view and exit view")]
        [SerializeField] private List<View> _views;

        protected UISystem _uiSystem;
        protected SoundSystem _soundSystem;

        public virtual void Construct(SoundSystem soundSystem, UISystem uiSystem)
        {
            Utilities.Logger.Log("Base SceneView Construct", Settings.LogTypes.Info);

            _uiSystem = uiSystem;
            _soundSystem = soundSystem;

            Init();
        }

        public virtual void Init()
        {
            if (_rootView != null && _exitView != null)
            {
                _views = _uiSystem.CheckIfInListOfViewsHasRootViewOrExitView(_views, _rootView, _exitView);
            }

            _views = _uiSystem.CheckIfInListOfViewsHasDuplicatesOfViews(_views);

            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Construct(_soundSystem);

                _views[i].Hide();
            }

            if (_rootView != null)
            {
                _rootView.Construct(_soundSystem);
                _rootView.Show();
            }

            if (_exitView != null)
            {
                _exitView.Construct(_soundSystem);
                _exitView.Hide();
            }

            _uiSystem.SetupViewsInCurrentScene(_views, _rootView, _exitView, this);
        }

        public virtual void ShowView(View view)
        {
        }

        public virtual void HideView()
        {
        }

        public virtual void Dispose()
        {
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Dispose();
            }

            _rootView.Dispose();
            _rootView = null;

            _exitView.Dispose();
            _exitView = null;

            _uiSystem.ResetViewsBeforeSceneChange();

            _soundSystem = null;
            _uiSystem = null;
        }
    }
}