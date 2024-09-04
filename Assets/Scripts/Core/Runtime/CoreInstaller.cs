using Core.AssetProvider;
using Core.SceneManager;
using Core.StateMachine;
using Gameplay.StoryEngine;
using Zenject;

namespace Core
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SceneManagerInstaller.Install(Container);
            StateMachineInstaller.Install(Container);
            AssetProviderInstaller.Install(Container);
            
            StoryEngineInstaller.Install(Container);
            
            BindBootstrap(Container);
        }

        private void BindBootstrap(DiContainer diContainer)
        {
            diContainer.BindInterfacesTo<Bootstrap>()
                .AsSingle()
                .NonLazy();
        }
    }
}
