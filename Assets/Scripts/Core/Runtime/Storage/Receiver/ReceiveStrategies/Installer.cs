using Zenject;

namespace Core.Storage
{
    public class ReceiveStrategiesInstaller : Installer<ReceiveStrategiesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ReceiveStrategyAmountableItem>()
                .AsCached();
        }
    }
}