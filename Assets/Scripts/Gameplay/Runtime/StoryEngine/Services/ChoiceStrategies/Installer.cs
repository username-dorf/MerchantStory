using Zenject;

namespace Gameplay.StoryEngine
{
    public class ChoiceStrategiesInstaller : Installer<ChoiceStrategiesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ChoiceStrategyReceiveItem>()
                .AsCached();
        }
    }
}