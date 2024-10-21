using System.Collections.Generic;
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
        public List<InputAction> InputAction { get; protected set; }
        public ReactiveCommand<SwipeProgress> OnSwipeProgressChanged { get; }
        public ReactiveCommand<Direction> OnSwipeRegistered { get; }
        public ReactiveCommand<Unit> OnSwipeCanceled { get; }

        private UserInputSettingsProvider _settingsProvider;

        protected Direction _lastDirection;
        protected Vector2 _startPosition;
        protected Vector2 _endPosition;
        protected bool _released;
        
        protected DeviceInputStrategy(UserInputSettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            OnSwipeProgressChanged = new ReactiveCommand<SwipeProgress>();
            OnSwipeRegistered = new ReactiveCommand<Direction>();
            OnSwipeCanceled = new ReactiveCommand<Unit>();
        }

        public abstract void Initialize();
        public abstract void OnSwipePerformed(InputAction.CallbackContext context);

        public virtual void OnInputCompleted(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnInputStarted(InputAction.CallbackContext context)
        {
            
        }
        
        private float _smoothedProgress = 0f;
        private const float _smoothingFactor = 0.1f; // Adjust between 0 and 1

        protected void UpdateSwipeProgress(Vector2 currentPosition)
        {
            var swipeDelta = currentPosition - _startPosition;
            var currentDirection = swipeDelta.x > 0 ? Direction.Right : Direction.Left;

            if (_lastDirection != currentDirection && _lastDirection != Direction.None)
            {
                _startPosition = currentPosition;
                swipeDelta = Vector2.zero;
                _smoothedProgress = 0f;
                OnSwipeProgressChanged.Execute(new SwipeProgress(currentDirection, _smoothedProgress));
                Debug.Log($"OnSwipeProgressChanged {currentDirection} {_smoothedProgress}");
            }

            _lastDirection = currentDirection;
            var rawProgress = Mathf.Clamp(swipeDelta.magnitude / _settingsProvider.SwipeThreshold, 0f, 1f);
            _smoothedProgress = Mathf.Lerp(_smoothedProgress, rawProgress, _smoothingFactor);

            OnSwipeProgressChanged.Execute(new SwipeProgress(currentDirection, _smoothedProgress));
            Debug.Log($"OnSwipeProgressChanged {currentDirection} {_smoothedProgress}");
        }

        protected void DetectSwipe()
        {
            if (_released)
                return;

            var swipeDelta = _endPosition - _startPosition;
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) &&
                Mathf.Abs(swipeDelta.x) > _settingsProvider.SwipeThreshold)
            {
                var direction = swipeDelta.x > 0 ? Direction.Right : Direction.Left;
                if (_smoothedProgress > 0.9f)
                {
                    OnSwipeRegistered.Execute(direction);
                }
                else
                {
                    OnSwipeCanceled.Execute(Unit.Default);

                }
                _released = true;
            } 
            
        }
    }
}