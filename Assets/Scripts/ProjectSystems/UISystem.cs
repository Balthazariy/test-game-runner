using System.Collections.Generic;
using System.Linq;
using TestGame.Scenes.Base;
using TestGame.UI.Views.Base;
using UnityEngine;
using Zenject;

namespace TestGame.ProjectSystems
{
    public class UISystem : MonoBehaviour, ISystem
    {
        /// <summary>
        /// Root view in current scene that never hide by ViewStacks
        /// </summary>
        private View _rootView;

        /// <summary>
        /// All views in current scene
        /// </summary>
        private List<View> _views;

        private View _currentView;
        private View _previousView;

        private SceneView _currentSceneView;

        public View CurrentView
        {
            get
            {
                return _currentView;
            }
            private set
            {
                _currentView = value;
            }
        }

        public SceneView CurrentSceneView
        {
            get
            {
                return _currentSceneView;
            }
            private set
            {
                _currentSceneView = value;
            }
        }

        [Inject]
        public void Construct()
        {
            Utilities.Logger.Log("UISystem Construct", Settings.LogTypes.Info);
        }

        public void Init()
        {
        }

        public void SetupViewsInCurrentScene(List<View> views, View rootView, View exitFromSceneView, Scenes.Base.SceneView sceneView)
        {
            _currentSceneView = sceneView;

            _rootView = rootView;

            _views = views;
        }

        public List<View> CheckIfInListOfViewsHasRootViewOrExitView(List<View> targetListOfViews, View rootView, View exitFromSceneView)
        {
            for (int i = 0; i < targetListOfViews.Count; i++)
            {
                if (targetListOfViews[i] == rootView || targetListOfViews[i] == exitFromSceneView)
                {
                    targetListOfViews.RemoveAt(i);
                }
            }

            return targetListOfViews;
        }

        public List<View> CheckIfInListOfViewsHasDuplicatesOfViews(List<View> targetListOfViews)
        {
            List<View> distinationList = targetListOfViews.Distinct().ToList();

            return distinationList;
        }

        // TODO - run this code before loading new scene
        // For example - when user click on button [Return to Menu] in gameplay scene
        // Need to reset current scene before initializing new
        // If don't do this - welcom to errror hell
        // My little pony (/ ^_^)/  \(^_^ \)
        public void ResetViewsBeforeSceneChange()
        {
            Utilities.Logger.Log($"Reseting views and SceneView in [{CurrentSceneView}]", Settings.LogTypes.Info);

            if (_views != null)
            {
                _views.Clear();
                _views = null;
            }

            if (_rootView != null)
            {
                _rootView.Dispose();
                _rootView = null;
            }

            if (_currentSceneView != null)
            {
                _currentSceneView.Dispose();
                _currentSceneView = null;
            }
        }

        public void ShowView<T>() where T : View
        {
            if (_currentView != null)
            {
                _previousView = _currentView;
                _currentView.Hide();
            }

            for (int i = 0; i < _views.Count; i++)
            {
                if (_views[i] is T)
                {
                    _currentView = _views[i];
                    break;
                }
            }

            _currentView.Show();
        }
    }
}