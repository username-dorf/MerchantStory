using Core.AssetProvider;
using Core.Database;
using Core.SceneManager;
using Core.StateMachine;
using Core.Storage;
using Gameplay.StoryEngine;
using Zenject;

namespace Core
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            DatabaseInstaller.Install(Container);
            SceneManagerInstaller.Install(Container);
            StateMachineInstaller.Install(Container);
            AssetProviderInstaller.Install(Container);
            StorageInstaller.Install(Container);
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
