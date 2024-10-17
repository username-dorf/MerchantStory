using Gameplay.StoryEngine;
using UI.HUD;
using UI.Runtime.Situation;
using UnityEngine;
using Zenject;

namespace Gameplay.Modes.SearchMode
{
    public class AdventureModeInstaller : MonoInstaller
    {
        [SerializeField] private HUDView hudView;

        public override void InstallBindings()
        {
            StoryEngineInstaller.Install(Container);
            Container.BindInterfacesTo<AdventureModeBootstrap>()
                .AsSingle();

            Container.Bind<UISituationViewFactory>()
                .AsSingle();
            
            Container.Bind<IHUDView>().WithId(nameof(AdventureHUDView)).FromInstance(hudView)
                .AsCached();
        }
    }
}