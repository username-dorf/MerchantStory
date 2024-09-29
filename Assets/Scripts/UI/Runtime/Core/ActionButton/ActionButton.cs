using System;
using UnityEngine;
using Zenject;

namespace UI.Runtime
{
    [RequireComponent(typeof(ActionButtonView))]
    public class ActionButton : MonoBehaviour
    {
        private ActionButtonView _view;
        private ActionButtonViewModel.Factory _viewModelFactory;
        private ActionButtonViewModel _viewModel;

        [Inject]
        public void Construct(ActionButtonViewModel.Factory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }
        public void Awake()
        {
            _view = GetComponent<ActionButtonView>();
        }

        public void Initialize(ActionButtonModel model)
        {
            _viewModel = _viewModelFactory.Create(model);
            _view.Initialize(_viewModel);
        }

        public void OnDestroy()
        {
            _viewModel?.Dispose();
        }
    }
}