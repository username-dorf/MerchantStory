using System;
using UniRx;
using Zenject;

namespace UI.Runtime
{
    public class ActionButtonViewModel : IDisposable
    {
        public ReactiveProperty<string> Text { get;private set; }
        public ReactiveProperty<Action> OnClick { get;private set; }

        private CompositeDisposable _disposable;
        private ActionButtonModel _model;

        public ActionButtonViewModel(ActionButtonModel model)
        {
            _model = model;
            
            Text = new ReactiveProperty<string>(model.Text.Value);
            OnClick = new ReactiveProperty<Action>(model.OnClick.Value);
            
            _disposable = new CompositeDisposable();
            
            _model.Text
                .Subscribe(OnTextChanged)
                .AddTo(_disposable);

            _model.OnClick
                .Subscribe(OnActionChanged)
                .AddTo(_disposable);
        }
        
        private void OnTextChanged(string value)
        {
            Text.Value = value;
        }
        private void OnActionChanged(Action value)
        {
            OnClick.Value = value;
        }

        public void Dispose()
        {
            Text?.Dispose();
            _disposable?.Dispose();
        }
    
        public class Factory : PlaceholderFactory<ActionButtonModel,ActionButtonViewModel>
        {
        
        }
    }
}
