using Core.Utils;
using UnityEngine;

namespace Gameplay.StoryEngine.Constructor
{
    [CreateAssetMenu(fileName = "SituationObject", menuName = "Scriptable Objects/StoryEngine/SituationObject")]
    public class SituationObject : ScriptableObject, IContainingProbability
    {
        [field: SerializeField] public float Chance { get;  private set; }
        
        [field: SerializeField] public string[] Tags { get; private set; }
        
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public SituationContextObject[] Contexts { get; private set; }
    }
}