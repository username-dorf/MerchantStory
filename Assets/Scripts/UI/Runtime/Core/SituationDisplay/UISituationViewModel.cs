using System;
using MVVM;
using UniRx;
using Zenject;

namespace UI.Runtime.SituationDisplay
{
    public class UISituationViewModel : IInitializable, IDisposable
    {
        [Data("Description")] public ReactiveProperty<string> description = new();
        [Data("ChoiceA")] public ReactiveProperty<string> choiceA = new();
        [Data("ChoiceB")] public ReactiveProperty<string> choiceB = new();

        private CompositeDisposable _disposable;
        private IUISituationModel _model;

        public UISituationViewModel(IUISituationModel model)
        {
            _model = model;
            _disposable = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            _model.Description
                .Subscribe( description.SetValueAndForceNotify)
                .AddTo(_disposable);
            _model.ChoiceA
                .Subscribe(choiceA.SetValueAndForceNotify)
                .AddTo(_disposable);
            _model.ChoiceB
                .Subscribe(choiceB.SetValueAndForceNotify)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            description?.Dispose();
            choiceA?.Dispose();
            choiceB?.Dispose();
            _disposable?.Dispose();
        }
    }
}