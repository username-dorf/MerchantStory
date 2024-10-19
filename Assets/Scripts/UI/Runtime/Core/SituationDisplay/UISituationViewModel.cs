using System;
using Core.Input;
using MVVM;
using PrimeTween;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Runtime.SituationDisplay
{
    public class UISituationViewModel : IInitializable, IDisposable
    {
        public ReactiveProperty<SwipeProgress> SwipeProgress { get; }
        public ReactiveProperty<float> FlipProgress { get; }
        public ReactiveProperty<string> Description { get; }
        public ReactiveProperty<string> ChoiceADesc { get; }
        public ReactiveProperty<string> ChoiceBDesc { get; }

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
            Description = new ReactiveProperty<string>();
            ChoiceADesc = new ReactiveProperty<string>();
            ChoiceBDesc = new ReactiveProperty<string>();
        }
        
        public void Initialize()
        {
            _inputListener.OnSwipeProgressChanged
                .Subscribe(SwipeProgress.SetValueAndForceNotify)
                .AddTo(_disposable);
            _inputListener.OnSwipeRegistered
                .Subscribe(_ => OnSwipeRegistered())
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

        private void OnSwipeRegistered()
        {
            Tween.Custom(0f, 1f, 0.5f, value =>
            {
                FlipProgress.Value = value;
            });
        }

        public void Dispose()
        {
            Description?.Dispose();
            ChoiceADesc?.Dispose();
            ChoiceBDesc?.Dispose();
            _disposable?.Dispose();
        }
    }
}