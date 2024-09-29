using UniRx;

namespace Gameplay.StoryEngine
{
    public sealed class SituationContextModel
    {
        public ReadOnlyReactiveProperty<string> Description { get; private set; }
        public ReadOnlyReactiveProperty<ChoiceModel> Choices { get; private set; }
    }
}