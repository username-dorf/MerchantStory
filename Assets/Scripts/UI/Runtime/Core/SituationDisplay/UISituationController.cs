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
        private List<UISituationViewModel> _viewModels;
        private UISituationViewPool _uiSituationViewPool;

        public UISituationController(UISituationViewPool uiSituationViewPool)
        {
            _uiSituationViewPool = uiSituationViewPool;
            _currentViewModelIndex = new ReactiveProperty<int>(0);
            _viewModels = new List<UISituationViewModel>();
            _disposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            for (int i = 0; i < INITIAL_VIEW_COUNT; i++)
            {
                var viewModel = _uiSituationViewPool.Spawn();
                _viewModels.Add(viewModel);
                
                viewModel.OnViewClosed
                    .Subscribe(OnViewClosed)
                    .AddTo(_disposable);
            }
        }

        public void Run(int index=-1)
        {
            var i = _currentViewModelIndex.Value;
            if(index>=0)
                i = index;
            _viewModels[i].SetRevealedState();
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