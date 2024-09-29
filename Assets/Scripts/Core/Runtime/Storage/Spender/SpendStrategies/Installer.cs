using Zenject;

namespace Core.Storage
{
    public class SpendStrategiesInstaller : Installer<SpendStrategiesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SpendStrategyAmountableItem>()
                .AsCached();
        }
    }
}