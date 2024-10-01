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
            Container.Bind<DeviceInputStrategy>()
                .To<KeyboardInputStrategy>()
                .AsCached();
            Container.Bind<DeviceInputStrategy>()
                .To<TouchInputStrategy>()
                .AsCached();
            Container.Bind<DeviceInputStrategy>()
                .To<MouseInputStrategy>()
                .AsCached();
        }
    }
}