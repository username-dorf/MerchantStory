using System.Collections.Generic;
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
            var inputAction = new InputAction("KeyboardSwipe", InputActionType.Button);

            inputAction.AddBinding("<Keyboard>/a");
            inputAction.AddBinding("<Keyboard>/d");

            inputAction.started += OnInputStarted;
            inputAction.performed += OnSwipePerformed;
            inputAction.canceled += OnInputCompleted;
            
            InputAction = new List<InputAction>()
            {
                inputAction
            };
        }

        public override void OnInputStarted(InputAction.CallbackContext context)
        {
            _released = false;
            _startPosition = Vector2.zero;
        }

        public override void OnSwipePerformed(InputAction.CallbackContext context)
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
                _released = true;

                OnSwipeRegistered.Execute(direction);
                Debug.Log($"Keyboard Swipe {direction}");
            }
        }

        public override void OnInputCompleted(InputAction.CallbackContext context)
        {
        }
    }
}