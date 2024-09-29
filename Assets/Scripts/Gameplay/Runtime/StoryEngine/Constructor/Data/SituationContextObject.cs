using Core.Utils;
using UnityEngine;

namespace Gameplay.StoryEngine.Constructor
{
    [CreateAssetMenu(fileName = "SituationContext", menuName = "Scriptable Objects/StoryEngine/SituationContext")]
    public class SituationContextObject : ScriptableObject, IContainingProbability
    {
        [field: SerializeField] public float Chance { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public ChoiceObject[] Choices { get; private set; } = new ChoiceObject[2];
    }
}