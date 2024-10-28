using Gameplay.StoryEngine.Constructor;
using Gameplay.StoryEngine.Core;
using UI.SituationDisplay;
using Zenject;

namespace Gameplay.StoryEngine
{
    public class StoryEngineInstaller : Installer<StoryEngineInstaller>
    {
        public override void InstallBindings()
        {
            ChoiceStrategiesInstaller.Install(Container);
            
            Container.BindInterfacesTo<SituationsResourcesController>()
                .AsSingle();
            
            
            Container.BindFactory<SituationObject, SituationModel,SituationModel.Factory>()
                .AsSingle();
            
            Container.BindFactory<ChoiceObject,ChoiceModel,ChoiceModel.Factory>()
                .AsSingle();    
            
            Container.BindFactory<UISituationViewModel,UISituationViewModel.Factory>()
                .AsSingle();
            
            
            Container.Bind<ChoiceCommandStrategyFactory>()
                .AsSingle();
            Container.Bind<SituationsQueueFactory>()
                .AsSingle();  
            Container.Bind<SituationFactory>()
                .AsSingle();   
            Container.Bind<SituationQueueProcessor>()
                .AsSingle();

            Container.BindInterfacesTo<UISituationModel>()
                .AsSingle();
        }
    }
}