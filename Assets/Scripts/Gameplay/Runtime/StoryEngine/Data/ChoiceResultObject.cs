using Gameplay.Runtime.StoryEngine;
using UnityEngine;

namespace Gameplay.StoryEngine
{
    [CreateAssetMenu(fileName = "ChoiceResultObject", menuName = "Scriptable Objects/StoryEngine/ChoiceResultObject")]
    public class ChoiceResultObject : ChoiceObject
    {
        [field: SerializeField] public float Chance { get; private set; }
        [field: SerializeField] public ItemModel Output { get; private set; }
    }
}