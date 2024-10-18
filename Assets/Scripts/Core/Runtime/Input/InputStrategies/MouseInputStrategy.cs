using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class MouseInputStrategy : DeviceInputStrategy, IDisposable
    {
        private bool _isInputActive;
        private InputAction _positionAction;
        private InputAction _buttonAction;

        public MouseInputStrategy(UserInputSettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public override void Initialize()
        {
            _positionAction = new InputAction("MousePosition", InputActionType.Value);
            _positionAction.AddBinding("<Mouse>/position");

            _buttonAction = new InputAction("MouseButton", InputActionType.Button);
            _buttonAction.AddBinding("<Mouse>/leftButton");

            _buttonAction.started += OnInputStarted;
            _buttonAction.canceled += OnInputCompleted;
            _positionAction.performed += OnSwipePerformed;

            InputAction = new List<InputAction>()
            {
                _positionAction,
                _buttonAction
            };
            
        }

        public void Dispose()
        {
            _buttonAction.started -= OnInputStarted;
            _buttonAction.canceled -= OnInputCompleted;
            _positionAction.performed -= OnSwipePerformed;

            _buttonAction.Dispose();
            _positionAction.Dispose();
        }

        public override void OnInputStarted(InputAction.CallbackContext context)
        {
            _startPosition = Mouse.current.position.ReadValue();
            _released = false;
            _isInputActive = true;
            _lastDirection = Direction.None;
        }

        public override void OnSwipePerformed(InputAction.CallbackContext context)
        {
            if (_isInputActive)
            {
                Vector2 currentPosition = context.ReadValue<Vector2>();
                UpdateSwipeProgress(currentPosition);
            }
        }

        public override void OnInputCompleted(InputAction.CallbackContext context)
        {
            if (_isInputActive)
            {
                _endPosition = Mouse.current.position.ReadValue();
                DetectSwipe();
                _isInputActive = false;
                _lastDirection = Direction.None;
            }
        }
    }
}
