using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Runtime.StoryEngine
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/StoryEngine/Item")]

    public class ItemObject : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public AssetReferenceSprite Icon { get; private set; }
    }
}