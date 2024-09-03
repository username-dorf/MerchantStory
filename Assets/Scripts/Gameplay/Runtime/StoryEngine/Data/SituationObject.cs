using UnityEngine;

namespace Gameplay.Runtime.StoryEngine
{
    [CreateAssetMenu(fileName = "SituationObject", menuName = "Scriptable Objects/StoryEngine/SituationObject")]
    public class SituationObject : ScriptableObject
    {
        [field: SerializeField] public string[] Tags { get; private set; }
        
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public SituationContextObject[] Contexts { get; private set; }
    }
}