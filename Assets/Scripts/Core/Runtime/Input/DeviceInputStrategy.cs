using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public interface IDeviceInputStrategy
    {
        ReactiveCommand<SwipeProgress> OnSwipeProgressChanged { get; }
        ReactiveCommand<Direction> OnSwipeRegistered { get; }
    }

    public abstract class DeviceInputStrategy : IDeviceInputStrategy
    {
        public InputAction InputAction { get; protected set; }
        public ReactiveCommand<SwipeProgress> OnSwipeProgressChanged { get; }
        public ReactiveCommand<Direction> OnSwipeRegistered { get; }

        private UserInputSettingsProvider _settingsProvider;

        protected Vector2 _startPosition;
        protected Vector2 _endPosition;
        protected bool _detected;
        
        protected DeviceInputStrategy(UserInputSettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            OnSwipeProgressChanged = new ReactiveCommand<SwipeProgress>();
            OnSwipeRegistered = new ReactiveCommand<Direction>();
        }

        public abstract void Initialize();
        public abstract void OnInputPerformed(InputAction.CallbackContext context);

        public virtual void OnInputCanceled(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnInputStarted(InputAction.CallbackContext context)
        {
            
        }
        
        protected void UpdateSwipeProgress(Vector2 currentPosition)
        {
            var swipeDelta = currentPosition - _startPosition;
            var progress = Mathf.Clamp(swipeDelta.magnitude / _settingsProvider.SwipeThreshold, 0f, 1f);
            var direction = swipeDelta.x > 0 ? Direction.Right : Direction.Left;
            OnSwipeProgressChanged.Execute(new SwipeProgress(direction, progress));
        }
        protected void DetectSwipe()
        {
            if (_detected)
                return;

            var swipeDelta = _endPosition - _startPosition;
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) &&
                Mathf.Abs(swipeDelta.x) > _settingsProvider.SwipeThreshold)
            {
                var direction = swipeDelta.x > 0 ? Direction.Right : Direction.Left;
                OnSwipeRegistered.Execute(direction);
                Debug.Log($"Swipe {direction}");
                _detected = true;
            }
        }
    }
}