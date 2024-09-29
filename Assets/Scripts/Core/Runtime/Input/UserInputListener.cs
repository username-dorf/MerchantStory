using System;
using UniRx;
using Zenject;

namespace Core.Input
{
    public interface IUserInputListener
    {
        ReadOnlyReactiveProperty<bool> IsListening { get; }
        void SetListening(bool isListening);

        ReactiveCommand<Direction> OnSwipeRegistered { get; }
        ReactiveCommand<SwipeProgress> OnSwipeProgressChanged { get; }
    }

    public class UserInputListener : IUserInputListener, IInitializable, IDisposable
    {
        public ReactiveCommand<Direction> OnSwipeRegistered => _deviceInputStrategy.OnSwipeRegistered;
        public ReactiveCommand<SwipeProgress> OnSwipeProgressChanged => _deviceInputStrategy.OnSwipeProgressChanged;

        public ReadOnlyReactiveProperty<bool> IsListening => _isListening.ToReadOnlyReactiveProperty();


        private ReactiveProperty<bool> _isListening;
        private CompositeDisposable _disposable;
        private DeviceInputStrategy _deviceInputStrategy;
        private KeyboardInputStrategy _keyboardInputStrategy;

        public UserInputListener(
            KeyboardInputStrategy keyboardInputStrategy,
            DeviceInputStrategy deviceInputStrategy)
        {
            _keyboardInputStrategy = keyboardInputStrategy;
            _deviceInputStrategy = deviceInputStrategy;
            _isListening = new ReactiveProperty<bool>(false);
            _disposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _deviceInputStrategy.Initialize();
            _keyboardInputStrategy.Initialize();
            _isListening
                .Subscribe(OnListeningChanged)
                .AddTo(_disposable);
        }

        public void SetListening(bool isListening)
        {
            _isListening.Value = isListening;
        }

        private void OnListeningChanged(bool isListening)
        {
            if (isListening)
            {
                _deviceInputStrategy.InputAction.Enable();
                _keyboardInputStrategy.InputAction.Enable();
            }
            else
            {
                _deviceInputStrategy.InputAction.Disable();
                _keyboardInputStrategy.InputAction.Disable();
            }
        }

        public void Dispose()
        {
            _isListening?.Dispose();
            _disposable?.Dispose();
        }
    }
}