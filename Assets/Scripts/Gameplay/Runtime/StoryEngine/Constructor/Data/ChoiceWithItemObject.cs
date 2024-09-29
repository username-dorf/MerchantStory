using Core.Storage;
using UnityEngine;

namespace Gameplay.StoryEngine.Constructor
{
    [CreateAssetMenu(fileName = "ChoiceWithItemObject", menuName = "Scriptable Objects/StoryEngine/ChoiceWithItemObject")]
    public class ChoiceWithItemObject : ChoiceObject
    {
        [field: SerializeField] public AmountableItem[] Output { get; private set; }
    }
}