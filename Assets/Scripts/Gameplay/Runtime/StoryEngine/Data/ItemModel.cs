using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.StoryEngine
{
    [Serializable] public class ItemModel
    {
        [field: SerializeField] public AssetReferenceT<ItemObject> ItemObject { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}