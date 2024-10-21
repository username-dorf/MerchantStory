using Gameplay.StoryEngine;
using UI.SituationDisplay;
using Zenject;

namespace Gameplay.Modes.SearchMode
{
    public class AdventureModeBootstrap : IInitializable
    {
        private SituationQueueProcessor _situationQueueProcessor;
        private UISituationController _uiSituationController;

        public AdventureModeBootstrap(
            UISituationController uiSituationController,
            SituationQueueProcessor situationQueueProcessor)
        {
            _uiSituationController = uiSituationController;
            _situationQueueProcessor = situationQueueProcessor;
        }
        public async void Initialize()
        {
            _uiSituationController.Initialize();
            await _situationQueueProcessor.CreateQueue(default);
            _uiSituationController.Run();
            await _situationQueueProcessor.Run(default);
        }
    }
}