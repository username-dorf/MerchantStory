using System;
using UniRx;

namespace UI.Runtime
{
    public class ActionButtonModel
    {
        public ReactiveProperty<string> Text { get; private set; }
        public ReactiveProperty<Action> OnClick { get; private set; }

        public ActionButtonModel(string text, Action onClick = null)
        {
            Text = new ReactiveProperty<string>(text);
            OnClick = new ReactiveProperty<Action>(onClick);
        }
    }
}
