using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Runtime
{
    public class ActionButtonView : MonoBehaviour
    {

        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;

        private CompositeDisposable _buttonDisposable = new();
    

        public void Initialize(ActionButtonViewModel viewModel)
        {
            viewModel.Text
                .Subscribe(OnTextChanged)
                .AddTo(this);

            viewModel.OnClick
                .Subscribe(OnActionChanged)
                .AddTo(this);
        }
    
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    
        private void OnTextChanged(string value)
        {
            text.text = value;
        }

        private void OnActionChanged(Action action)
        {
            _buttonDisposable?.Clear();
            button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Debug.Log("Action call");
                    action?.Invoke();
                })
                .AddTo(_buttonDisposable);
        }
    
    }
}
