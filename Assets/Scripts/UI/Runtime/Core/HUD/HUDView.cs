using System;
using UnityEngine;

namespace UI.HUD
{
    public interface IHUDView
    {
        RectTransform Header { get; }
        RectTransform Body { get; }
        RectTransform Footer { get; }
    }
    [Serializable] public abstract class HUDView : MonoBehaviour, IHUDView
    {
        [field: SerializeField] public RectTransform Header { get; private set; }
        [field: SerializeField] public RectTransform Body { get; private set; }
        [field: SerializeField] public RectTransform Footer { get; private set; }
    }
}