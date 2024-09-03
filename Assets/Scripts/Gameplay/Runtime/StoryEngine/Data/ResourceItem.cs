using System;
using UnityEngine;

namespace Gameplay.Runtime.StoryEngine
{
    [Serializable] public class ResourceItem
    {
        [field: SerializeField] public ItemObject ItemObject { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}