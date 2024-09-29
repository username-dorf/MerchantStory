using Core.Runtime.Storage;
using Core.Storage.Receiver;
using Zenject;

namespace Core.Storage
{
    public class StorageInstaller : Installer<StorageInstaller>
    {
        public override void InstallBindings()
        {
            ReceiveStrategiesInstaller.Install(Container);
            Container.BindInterfacesTo<StorageReceiver>()
                .AsSingle();
        }
    }
}