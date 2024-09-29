using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class MouseInputStrategy : DeviceInputStrategy, IDisposable
    {
        protected bool _isInputActive;

        public MouseInputStrategy(UserInputSettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public override void Initialize()
        {
            InputAction = new InputAction("MouseSwipe", InputActionType.Button);
            InputAction.AddBinding("<Mouse>/leftButton")
                .WithInteraction("Press");
            InputAction.started += OnInputStarted;
            InputAction.performed += OnInputPerformed;
            InputAction.canceled += OnInputCanceled;
        }

        public void Dispose()
        {
        }

        public override void OnInputStarted(InputAction.CallbackContext context)
        {
            if (Mouse.current != null)
            {
                _startPosition = Mouse.current.position.ReadValue();
                _detected = false;
                _isInputActive = true;
            }
        }


        public override void OnInputPerformed(InputAction.CallbackContext context)
        {
            if (_isInputActive)
            {
                Vector2 currentPosition = Mouse.current.position.ReadValue();
                UpdateSwipeProgress(currentPosition);
            }
        }

        public override void OnInputCanceled(InputAction.CallbackContext context)
        {
            if (_isInputActive)
            {
                _endPosition = Mouse.current.position.ReadValue();
                DetectSwipe();
                _isInputActive = false;
            }
        }
    }
}