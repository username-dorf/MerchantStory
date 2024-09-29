using Zenject;

namespace UI.Runtime
{
    public class ActionButtonMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<ActionButtonModel, ActionButtonViewModel, ActionButtonViewModel.Factory>()
                .AsSingle()
                .NonLazy();
        }
    }
}