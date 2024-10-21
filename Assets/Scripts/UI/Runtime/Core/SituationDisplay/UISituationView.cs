using System;
using System.Collections.Generic;
using Core.AssetProvider;
using Core.Input;
using NaughtyAttributes;
using TMPro;
using UI.HUD;
using UI.ViewStateMachine;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.SituationDisplay
{
    public class UISituationView : MonoBehaviour
    {
        [SerializeField,ReadOnly] private UISituationState currentState;
        [SerializeField] private Animator animator;
        [field: SerializeField] public Button CloseButton { get; private set; }
        
        public TMP_Text description;
        public TMP_Text choiceA;
        public TMP_Text choiceB;
        
        private UISituationViewAnimation _animation;
        private ViewStateMachine.ViewStateMachine _stateMachine;
        
        private IViewState _hiddenState;
        private IViewState _revealedState;
        private IViewState _resultState;

        
        public void Initialize(UISituationViewModel viewModel)
        {
            _animation = new UISituationViewAnimation(animator);
            _stateMachine = new ViewStateMachine.ViewStateMachine();
            _hiddenState = new StateHidden(this);
            _revealedState = new StateRevealed(this);
            _resultState = new StateResult(this);

            viewModel.ChoiceADesc
                .Subscribe(OnChoiceAChanged)
                .AddTo(this);

            viewModel.ChoiceBDesc
                .Subscribe(OnChoiceBChanged)
                .AddTo(this);

            viewModel.Description
                .Subscribe(OnDescriptionChanged)
                .AddTo(this);

            viewModel.FlipProgress
                .Subscribe(OnFlipValueChanged)
                .AddTo(this);

            viewModel.SwipeProgress
                .Subscribe(value=>OnSelectionValueChanged(value.Progress, value.Direction))
                .AddTo(this);

            viewModel.CancelProgress
                .Subscribe(OnCancelValueChanged)
                .AddTo(this);
            
            viewModel.CurrentState
                .Subscribe(OnStateChanged)
                .AddTo(this);

            CloseButton.OnClickAsObservable()
                .Subscribe(_=>viewModel.OnCloseButtonClick())
                .AddTo(this);
        }
        private void OnStateChanged(UISituationState newState)
        {
            currentState = newState;
            switch (newState)
            {
                case UISituationState.Hidden:
                    _stateMachine.ChangeState(_hiddenState);
                    break;
                case UISituationState.Revealed:
                    _stateMachine.ChangeState(_revealedState);
                    break;
                case UISituationState.Result:
                    _stateMachine.ChangeState(_resultState);
                    break;
            }
        }

        private void OnChoiceAChanged(string value)
        {
            choiceA.text = value;
        }
        
        private void OnChoiceBChanged(string value)
        {
            choiceB.text = value;
        }
        
        private void OnDescriptionChanged(string value)
        {
            description.text = value;
        }

        private void OnFlipValueChanged(float progress)
        {
            _animation.DoFlip(progress);
        }
        private void OnSelectionValueChanged(float progress, Direction direction)
        {
            _animation.DoSelectionDrag(progress, direction);
        }
        private void OnCancelValueChanged(float progress)
        {
            _animation.DoCancel(progress);
        }
        
        private void Reinitialize()
        {
            _animation.DoCancel(1);
        }
        
        private class StateHidden : IViewState
        {
            private readonly UISituationView _view;

            public StateHidden(UISituationView view)
            {
                _view = view;
            }
            public void Enter()
            {
                _view.Reinitialize();
                _view.transform.SetAsFirstSibling();
                _view.CloseButton.interactable = false;
            }

            public void Exit()
            {
            }
        }
        
        private class StateRevealed : IViewState
        {
            private readonly UISituationView _view;

            public StateRevealed(UISituationView view)
            {
                _view = view;
            }
            public void Enter()
            {
                _view.transform.SetAsLastSibling();
            }

            public void Exit()
            {
            }
        }
        
        private class StateResult : IViewState
        {
            private readonly UISituationView _view;

            public StateResult(UISituationView view)
            {
                _view = view;
            }
            public void Enter()
            {
                _view.CloseButton.interactable = true;
            }

            public void Exit()
            {
            }
        }
    }
    public enum UISituationState
    {
        Hidden,
        Revealed,
        Result
    }

    public class UISituationViewFactoryAssetRequest : IRequestAssets, IDisposable
    {
        public static string ASSET_TAG = "UISituationView";
        private Dictionary<Type, string> _dictionary;

        public Dictionary<Type, string> RequestAssets()
        {
            _dictionary = UnityEngine.Pool.DictionaryPool<Type,string>.Get();
            _dictionary.Add(typeof(UISituationView), ASSET_TAG);
            return _dictionary;
        }

        public void Dispose()
        {
            UnityEngine.Pool.DictionaryPool<Type, string>.Release(_dictionary);
        }
    }
    
    public class UISituationViewPool : MemoryPool<UISituationViewModel>
    {
        private Dictionary<UISituationViewModel,UISituationView> _viewModelDictionary;

        private IHUDView _hudView;
        private DiContainer _diContainer;
        private IAssetProvider _assetProvider;

        public UISituationViewPool(DiContainer diContainer, IAssetProvider assetProvider,[Inject(Id = nameof(AdventureHUDView))] IHUDView hudView)
        {
            _hudView = hudView;
            _assetProvider = assetProvider;
            _diContainer = diContainer;
            _viewModelDictionary = new Dictionary<UISituationViewModel,UISituationView>();
        }
        protected override void OnSpawned(UISituationViewModel viewModel)
        {
            base.OnSpawned(viewModel);
            var view = _diContainer.InstantiatePrefabForComponent<UISituationView>(
                _assetProvider.GetAsset<UISituationView>(UISituationViewFactoryAssetRequest.ASSET_TAG), _hudView.Body);
            viewModel.Initialize();
            view.Initialize(viewModel);
            viewModel.SetHiddenState();
            _viewModelDictionary.Add(viewModel,view);
        }
        protected override void OnDespawned(UISituationViewModel viewModel)
        {
            base.OnDespawned(viewModel);
            viewModel.SetHiddenState();
        }

        protected override void Reinitialize(UISituationViewModel viewModel)
        {
            base.Reinitialize(viewModel);
            viewModel.SetHiddenState();
        }
    }

    public class UISituationViewPoolInstaller : Installer<UISituationViewPoolInstaller>
    {
        [Inject(Id = nameof(AdventureHUDView))]
        private IHUDView _hudView;
        [Inject]
        private IAssetProvider _assetProvider;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<UISituationViewModel, UISituationViewPool>()
                .WithInitialSize(2);
        }
    }
}
