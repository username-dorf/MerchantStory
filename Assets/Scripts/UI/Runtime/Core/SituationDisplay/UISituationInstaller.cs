using Zenject;

namespace UI.Runtime.SituationDisplay
{
    public class UISituationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UISituationViewModel>()
                .AsSingle();
        }
    }
}