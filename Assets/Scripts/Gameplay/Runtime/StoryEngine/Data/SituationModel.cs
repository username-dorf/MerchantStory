using System;
using Core.Utils;
using Gameplay.StoryEngine.Constructor;
using UI.SituationDisplay;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.StoryEngine
{
    public sealed class SituationModel
    {
        public ReactiveProperty<string> Description { get; private set; }
        public ReactiveProperty<SituationContextObject> Context { get; private set; }

        public SituationModel(SituationObject situationObject)
        {
            Context =new ReactiveProperty<SituationContextObject>(situationObject.Contexts.ChooseRandom());
            Description = new ReactiveProperty<string>(Context.Value.Description);
        }
        
        public class Factory : PlaceholderFactory<SituationObject,SituationModel>
        {
            
        }
    }

    public class UISituationModel : IUISituationModel
    {
        public ReactiveProperty<string> Description { get; private set; }
        public ReactiveProperty<string> ChoiceA { get; private set; }
        public ReactiveProperty<string> ChoiceB { get; private set; }
        
        public UISituationModel()
        {
            Description = new ReactiveProperty<string>();
            ChoiceA = new ReactiveProperty<string>();
            ChoiceB = new ReactiveProperty<string>();
        }
        
        public void Update(string description, string choiceA, string choiceB)
        {
            Description.Value = description;
            ChoiceA.Value = choiceA;
            ChoiceB.Value = choiceB;
        }
       
    }
}