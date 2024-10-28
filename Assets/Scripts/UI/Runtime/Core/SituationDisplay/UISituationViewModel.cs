using System;
using Core.Input;
using PrimeTween;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.SituationDisplay
{
    public class UISituationViewModel : IInitializable, IDisposable
    {
        public ReactiveCommand<UISituationViewModel> OnViewClosed { get; }
        public ReactiveProperty<SwipeProgress> SwipeProgress { get; }
        public ReactiveProperty<float> FlipProgress { get; }
        public ReactiveProperty<float> CancelProgress { get; }
        public ReactiveProperty<string> Description { get; }
        public ReactiveProperty<string> ChoiceADesc { get; }
        public ReactiveProperty<string> ChoiceBDesc { get; }
        
        public ReactiveProperty<UISituationState> CurrentState { get; }


        private CompositeDisposable _disposable;
        private IUISituationModel _model;
        private IUserInputListener _inputListener;

        public UISituationViewModel(IUISituationModel model, IUserInputListener inputListener)
        {
            _inputListener = inputListener;
            _model = model;
            _disposable = new CompositeDisposable();
            
            SwipeProgress = new ReactiveProperty<SwipeProgress>();
            FlipProgress = new ReactiveProperty<float>();
            CancelProgress = new ReactiveProperty<float>();
            Description = new ReactiveProperty<string>();
            ChoiceADesc = new ReactiveProperty<string>();
            ChoiceBDesc = new ReactiveProperty<string>();
            CurrentState = new ReactiveProperty<UISituationState>();
            OnViewClosed = new ReactiveCommand<UISituationViewModel>();
        }
        
        public void Initialize()
        {
            _inputListener.OnSwipeProgressChanged
                .Subscribe(OnSwipeProgressChanged)
                .AddTo(_disposable);
            _inputListener.OnSwipeCanceled
                .Subscribe(_ => OnSwipeCanceled())
                .AddTo(_disposable);
            
            _inputListener.OnSwipeRegistered
                .Subscribe(OnSwipeRegistered)
                .AddTo(_disposable);
            
            _model.Description
                .Subscribe( Description.SetValueAndForceNotify)
                .AddTo(_disposable);
            _model.ChoiceA
                .Subscribe(ChoiceADesc.SetValueAndForceNotify)
                .AddTo(_disposable);
            _model.ChoiceB
                .Subscribe(ChoiceBDesc.SetValueAndForceNotify)
                .AddTo(_disposable);
        }

        private void OnSwipeRegistered(Direction direction)
        {
            if(CurrentState.Value is not UISituationState.Revealed)
                return;
            Tween.Custom(0f, 1f, 0.5f, value =>
            {
                FlipProgress.Value = value;
            }).OnComplete(SetResultState);
        }
        private void OnSwipeCanceled()
        {
            Tween.Custom(0f, 1f, 0.2f, value =>
            {
                CancelProgress.Value = value;
            });
        }
        public void SetRevealedState()
        {
            CurrentState.Value = UISituationState.Revealed;
        }

        public void SetResultState()
        {
            CurrentState.Value = UISituationState.Result;
        }

        public void SetHiddenState()
        {
            CurrentState.Value = UISituationState.Hidden;
        }
        
        public void OnCloseButtonClick()
        {
            OnViewClosed.Execute(this);
        }
        
        private void OnSwipeProgressChanged(SwipeProgress progress)
        {
            if(CurrentState.Value is not UISituationState.Revealed)
                return;
            Debug.Log($"Swipe progress {progress.Progress} {progress.Direction}");
            SwipeProgress.Value = progress;
        }
        
        public void Dispose()
        {
            Description?.Dispose();
            ChoiceADesc?.Dispose();
            ChoiceBDesc?.Dispose();
            _disposable?.Dispose();
        }
        
        public class Factory : PlaceholderFactory<UISituationViewModel>
        {
            
        }
    }
}