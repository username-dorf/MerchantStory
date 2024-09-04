using Gameplay.Runtime.StoryEngine;
using Zenject;

namespace Gameplay.StoryEngine.Core
{
    public class CoreInstaller : Installer<CoreInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<SituationModel.Factory>()
                .AsSingle();
            
            Container.BindInterfacesTo<SituationsResourcesController>()
                .AsSingle();
            Container.Bind<SituationsQueueFactory>()
                .AsSingle();
            
           
        }
    }
}