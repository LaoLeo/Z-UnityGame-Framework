using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    public float maxWidth;
    public RectMask2D mask2d;

    public void UpdateProgress(float value)
    {
        mask2d.rectTransform.sizeDelta = new Vector2(value * maxWidth, mask2d.rectTransform.rect.height);
    }
}