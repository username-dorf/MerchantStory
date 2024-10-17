using Core.AssetProvider;
using Core.Database;
using Core.Input;
using Core.SceneManager;
using Core.StateMachine;
using Core.Storage;
using Gameplay.StoryEngine;
using UI;
using UI.Runtime;
using Zenject;

namespace Core
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindersInstaller.Install(Container);
            UserInputInstaller.Install(Container);
            DatabaseInstaller.Install(Container);
            SceneManagerInstaller.Install(Container);
            StateMachineInstaller.Install(Container);
            AssetProviderInstaller.Install(Container);
            StorageInstaller.Install(Container);
            UIInstaller.Install(Container);
            
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
