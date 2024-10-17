using UniRx;

namespace UI.Runtime.SituationDisplay
{
    public interface IUISituationModel
    {
        ReactiveProperty<string> Description { get; }
        ReactiveProperty<string> ChoiceA { get; }
        ReactiveProperty<string> ChoiceB { get; }
        
        void Update(string description, string choiceA, string choiceB);
    }
}