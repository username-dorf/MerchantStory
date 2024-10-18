using Core.Input;
using UnityEngine;

public class SituationViewAnimation 
{
    private const float HORIZONTAL_RADIUS = 100f; 
    public float VERTICAL_RADIUS = 50f;
    
    private RectTransform _rectTransform;

    public SituationViewAnimation(RectTransform rectTransform)
    {
        _rectTransform = rectTransform;
    }
    
    public void DoSelectionDrag(float progress, Direction direction)
    {
        Vector2 startPosition = _rectTransform.anchoredPosition;

        float xOffset = HORIZONTAL_RADIUS * Mathf.Cos(progress * Mathf.PI / 2);
        float yOffset = VERTICAL_RADIUS * Mathf.Sin(progress * Mathf.PI / 2);

        if (direction == Direction.Left)
        {
            xOffset = -xOffset;
        }
        _rectTransform.anchoredPosition = startPosition + new Vector2(xOffset, -yOffset);
    }
}
