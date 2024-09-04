using UnityEngine;

namespace Gameplay.StoryEngine
{
    public abstract class ChoiceObject : ScriptableObject
    {
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public ChoiceObject ResultsChoice { get; private set; }
    }
}