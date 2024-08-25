using Zenject;

namespace Core.SceneManager
{
    public class SceneManagerInstaller : Installer<SceneManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SceneManager>()
                .AsSingle();
        }
    }
}