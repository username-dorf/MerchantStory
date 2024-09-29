using Zenject;

namespace Core.Storage
{
    public class StorageInstaller : Installer<StorageInstaller>
    {
        public override void InstallBindings()
        {
            ReceiveStrategiesInstaller.Install(Container);
            SpendStrategiesInstaller.Install(Container);
            
            Container.BindInterfacesTo<StorageReceiver>()
                .AsSingle();
            Container.BindInterfacesTo<StorageSpender>()
                .AsSingle();
        }
    }
}