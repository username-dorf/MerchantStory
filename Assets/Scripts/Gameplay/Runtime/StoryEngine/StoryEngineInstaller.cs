using Gameplay.StoryEngine.Core;
using Zenject;

namespace Gameplay.StoryEngine
{
    public class StoryEngineInstaller : Installer<StoryEngineInstaller>
    {
        public override void InstallBindings()
        {
            CoreInstaller.Install(Container);
        }
    }
}