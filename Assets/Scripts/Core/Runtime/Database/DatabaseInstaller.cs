using Zenject;

namespace Core.Database
{
    public class DatabaseInstaller : Installer<DatabaseInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DatabasePathProvider>()
                .AsSingle();

            Container.BindInterfacesTo<DatabaseConnectionProvider>()
                .AsSingle();
            
            Container.Bind<IRepositoryFactory>().To<RepositoryFactory>().AsSingle();
        }
    }
}