using System;
using Core.Utils;
using Gameplay.StoryEngine.Constructor;
using UniRx;
using Zenject;

namespace Gameplay.StoryEngine
{
    public sealed class SituationModel
    {
        public ReactiveProperty<string> Description { get; private set; }
        public ReactiveProperty<SituationContextObject> Context { get; private set; }

        public SituationModel(SituationContextObject[] contextObjects)
        {
            Context =new ReactiveProperty<SituationContextObject>(contextObjects.ChooseRandom());
            Description = new ReactiveProperty<string>(Context.Value.Description);
        }
        
        public class Factory : PlaceholderFactory<SituationObject,SituationModel>
        {
            
        }
    }
}