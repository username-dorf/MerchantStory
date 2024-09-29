using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Core.Input
{
    public class KeyboardInputStrategy : DeviceInputStrategy
    {
        public KeyboardInputStrategy(UserInputSettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public override void Initialize()
        {
            InputAction = new InputAction("KeyboardSwipe", InputActionType.Button);

            InputAction.AddBinding("<Keyboard>/a");
            InputAction.AddBinding("<Keyboard>/d");

            InputAction.started += OnInputStarted;
            InputAction.performed += OnInputPerformed;
            InputAction.canceled += OnInputCanceled;
        }

        public override void OnInputStarted(InputAction.CallbackContext context)
        {
            _detected = false;
            _startPosition = Vector2.zero;
        }

        public override void OnInputPerformed(InputAction.CallbackContext context)
        {
            if (context.control is KeyControl keyControl)
            {
                Direction direction;
                if (keyControl.keyCode == Key.A)
                {
                    direction = Direction.Left;
                }
                else if (keyControl.keyCode == Key.D)
                {
                    direction = Direction.Right;
                }
                else
                {
                    return;
                }

                var progress = 1f;

                OnSwipeProgressChanged.Execute(new SwipeProgress(direction, progress));

                _endPosition = Vector2.zero;
                _detected = true;

                OnSwipeRegistered.Execute(direction);
                Debug.Log($"Keyboard Swipe {direction}");
            }
        }

        public override void OnInputCanceled(InputAction.CallbackContext context)
        {
        }
    }
}