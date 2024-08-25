using Zenject;

namespace Core.AssetProvider
{
    public class AssetProviderInstaller : Installer<AssetProviderInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AssetProvider>()
                .AsSingle();
        }
    }
}