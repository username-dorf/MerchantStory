using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace UI.SituationDisplay
{
    public class UISituationController : IDisposable
    {
        private const int INITIAL_VIEW_COUNT = 2;
        private ReactiveProperty<int> _currentViewModelIndex;

        private CompositeDisposable _disposable;
        private Dictionary<UISituationViewModel,UISituationView> _viewModels;
        
        private UISituationViewPool _uiSituationViewPool;
        private UISituationViewModel.Factory _viewModelFactory;

        public UISituationController(UISituationViewPool uiSituationViewPool,UISituationViewModel.Factory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            _uiSituationViewPool = uiSituationViewPool;
            _currentViewModelIndex = new ReactiveProperty<int>(0);
            _viewModels = new Dictionary<UISituationViewModel,UISituationView>();
            _disposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            for (int i = 0; i < INITIAL_VIEW_COUNT; i++)
            {
                var view = _uiSituationViewPool.Spawn();
                var viewModel = _viewModelFactory.Create();
                view.Initialize(viewModel);
                viewModel.Initialize();
                viewModel.SetHiddenState();
                
                viewModel.OnViewClosed
                    .Subscribe(OnViewClosed)
                    .AddTo(_disposable);
                
                _viewModels.Add(viewModel,view);
            }
        }

        public void Run(int index=-1)
        {
            var i = _currentViewModelIndex.Value;
            if(index>=0)
                i = index;
            _viewModels.ElementAt(i).Key.SetRevealedState();
        }
        
        private void OnViewClosed(UISituationViewModel viewModel)
        {
            viewModel.SetHiddenState();
            _currentViewModelIndex.Value = (_currentViewModelIndex.Value + 1) % _viewModels.Count;
            Run(_currentViewModelIndex.Value);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _uiSituationViewPool?.Dispose();
        }
    }
}