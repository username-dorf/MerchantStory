using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Core.Input
{
    public class TouchInputStrategy : DeviceInputStrategy, IDisposable
    {
        private bool _isInputActive;

        public TouchInputStrategy(UserInputSettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public override void Initialize()
        {
            var inputAction = new InputAction("TouchSwipe", InputActionType.PassThrough);
            inputAction.AddBinding("<Touchscreen>/primaryTouch/position");
            inputAction.performed += OnSwipePerformed;
            
            var touchAction = new InputAction("Touch", InputActionType.PassThrough);
            touchAction.AddBinding("<Touchscreen>/primaryTouch/phase");
            touchAction.performed += OnTouchPerformed;
            touchAction.canceled += OnTouchPerformed;
            

            InputAction = new List<InputAction>()
            {
                inputAction,
                touchAction
                
            };
        }

        public void Dispose()
        {
            OnSwipeRegistered?.Dispose();
            OnSwipeProgressChanged?.Dispose();
        }

        public override void OnSwipePerformed(InputAction.CallbackContext context)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch == null)
                return;
            var touchPhase = touch.phase.ReadValue();
            var touchPosition = context.ReadValue<Vector2>();

            switch (touchPhase)
            {
                
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (_isInputActive)
                    {
                        var currentPosition = touchPosition;
                        UpdateSwipeProgress(currentPosition);
                    }
                    break;
            }
        }
        private void OnTouchPerformed(InputAction.CallbackContext context)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch == null)
                return;
            var phase = context.ReadValue<TouchPhase>();
            var touchPosition = touch.position.ReadValue();
            
            switch (phase)
            {
                case TouchPhase.Began:
                    _startPosition = touchPosition;
                    _isInputActive = true;
                    _released = false;
                    break;
                
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (_isInputActive)
                    {
                        _endPosition = touchPosition;
                        DetectSwipe();
                        _isInputActive = false;
                    }
                    break;
            }
        }
    }
}
