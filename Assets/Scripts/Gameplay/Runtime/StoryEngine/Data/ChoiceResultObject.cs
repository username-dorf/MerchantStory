using UnityEngine;

namespace Gameplay.Runtime.StoryEngine
{
    [CreateAssetMenu(fileName = "ChoiceResultObject", menuName = "Scriptable Objects/StoryEngine/ChoiceResultObject")]
    public class ChoiceResultObject : ChoiceObject
    {
        [field: SerializeField] public float Chance { get; private set; }
        [field: SerializeField] public ResourceItem OutputResource { get; private set; }
    }
}