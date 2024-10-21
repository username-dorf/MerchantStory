using Gameplay.StoryEngine;
using UI.HUD;
using UI.SituationDisplay;
using UnityEngine;
using Zenject;

namespace Gameplay.Modes.SearchMode
{
    public class AdventureModeInstaller : MonoInstaller
    {
        [SerializeField] private HUDView hudView;

        public override void InstallBindings()
        {
            Container.Bind<IHUDView>().WithId(nameof(AdventureHUDView)).FromInstance(hudView)
                .AsCached();
            StoryEngineInstaller.Install(Container);
            UISituationViewPoolInstaller.Install(Container);
            Container.BindInterfacesTo<AdventureModeBootstrap>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<UISituationController>()
                .AsSingle();

        }
    }
}