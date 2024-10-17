using Gameplay.StoryEngine;
using UI.HUD;
using UI.Runtime.Situation;
using Zenject;

namespace Gameplay.Modes.SearchMode
{
    public class AdventureModeBootstrap : IInitializable
    {
        private IHUDView _hudView;
        private UISituationViewFactory _uiSituationViewFactory;
        private SituationQueueProcessor _situationQueueProcessor;

        public AdventureModeBootstrap(
            [Inject(Id = nameof(AdventureHUDView))] IHUDView hudView,
            UISituationViewFactory uiSituationViewFactory,
            SituationQueueProcessor situationQueueProcessor)
        {
            _situationQueueProcessor = situationQueueProcessor;
            _uiSituationViewFactory = uiSituationViewFactory;
            _hudView = hudView;
        }
        public async void Initialize()
        {
            _uiSituationViewFactory.Create(_hudView.Body);
            await _situationQueueProcessor.CreateQueue(default);
            await _situationQueueProcessor.Run(default);
        }
    }
}