using UnityEngine;

namespace Gameplay.StoryEngine.Constructor
{
    public abstract class ChoiceObject : ScriptableObject
    {
        [field: SerializeField] public string Description { get; private set; }
    }
}