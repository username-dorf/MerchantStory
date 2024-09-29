using UnityEngine;
using Zenject;

namespace Core.Input
{
    public class UserInputInstaller : Installer<UserInputInstaller>
    {
        public override void InstallBindings()
        {
            BindInputStrategy();
            Container.Bind<UserInputSettingsProvider>()
                .AsSingle();
            Container.BindInterfacesTo<UserInputListener>()
                .AsSingle();
        }

        private void BindInputStrategy()
        {
            Container.Bind<KeyboardInputStrategy>().AsSingle();
            Debug.Log("Keyboard input strategy bound");

            var touchSupported = UnityEngine.InputSystem.Touchscreen.current != null;
            var mouseSupported = UnityEngine.InputSystem.Mouse.current != null;
            if (touchSupported)
            {
                Container.Bind<DeviceInputStrategy>().To<TouchInputStrategy>()
                    .AsSingle();
                Debug.Log("Touch input strategy bound");
            }
            else
            {
                Container.Bind<DeviceInputStrategy>().To<MouseInputStrategy>()
                    .AsSingle();
                Debug.Log("Mouse input strategy bound");
            }
        }
    }
}