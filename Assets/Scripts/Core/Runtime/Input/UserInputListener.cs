using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

namespace Core.Input
{
    public interface IUserInputListener
    {
        ReadOnlyReactiveProperty<bool> IsListening { get; }
        void SetListening(bool isListening);

        IObservable<Direction> OnSwipeRegistered { get; }
        IObservable<SwipeProgress> OnSwipeProgressChanged { get; }
    }

    public class UserInputListener : IUserInputListener, IInitializable, IDisposable
    {
        public IObservable<Direction> OnSwipeRegistered
        {
            get
            {
                return _deviceInputStrategies
                    .Select(strategy => strategy.OnSwipeRegistered)
                    .Merge();
            }
        }
        
        public IObservable<SwipeProgress> OnSwipeProgressChanged
        {
            get
            {
                return _deviceInputStrategies
                    .Select(strategy => strategy.OnSwipeProgressChanged)
                    .Merge();
            }
        }

        public ReadOnlyReactiveProperty<bool> IsListening => _isListening.ToReadOnlyReactiveProperty();


        private ReactiveProperty<bool> _isListening;
        private CompositeDisposable _disposable;
        private List<DeviceInputStrategy> _deviceInputStrategies;

        public UserInputListener(
            List<DeviceInputStrategy> deviceInputStrategies)
        {
            _deviceInputStrategies = deviceInputStrategies;
            _isListening = new ReactiveProperty<bool>(false);
            _disposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            foreach (var deviceInputStrategy in _deviceInputStrategies)
            {
                deviceInputStrategy.Initialize();
            }
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
                foreach (var deviceInputStrategy in _deviceInputStrategies)
                {
                    deviceInputStrategy.InputAction.ForEach(x=>x.Enable());
                }
            }
            else
            {
                foreach (var deviceInputStrategy in _deviceInputStrategies)
                {
                    deviceInputStrategy.InputAction.ForEach(x => x.Dispose());
                }
            }
        }

        public void Dispose()
        {
            _isListening?.Dispose();
            _disposable?.Dispose();
        }
    }
}