using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Core.Input
{
    public class TouchInputStrategy : DeviceInputStrategy, IDisposable
    {
        public TouchInputStrategy(UserInputSettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public override void Initialize()
        {
            InputAction = new InputAction("TouchSwipe", InputActionType.PassThrough);
            InputAction.AddBinding("<Touchscreen>/primaryTouch");
            InputAction.performed += OnInputPerformed;
            InputAction.canceled += OnInputCanceled;
        }

        public void Dispose()
        {
            OnSwipeRegistered?.Dispose();
            OnSwipeProgressChanged?.Dispose();
        }


        public override void OnInputPerformed(InputAction.CallbackContext context)
        {
            var touch = ((TouchControl) context.control).ReadValue();

            switch (touch.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    _startPosition = touch.position;
                    _detected = false;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    UpdateSwipeProgress(touch.position);
                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                    _endPosition = touch.position;
                    DetectSwipe();
                    break;
            }
        }
    }
}