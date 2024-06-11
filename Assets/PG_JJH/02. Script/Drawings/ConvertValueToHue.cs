using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script takes a value and converts it to a hue along the HSV color spectrum.
/// </summary>

public class ConvertValueToHue : MonoBehaviour
{
    [Range(0, 1)] public float saturation = 1.0f;
    [Range(0, 1)] public float value = 0.5f;

    // 이 색변화를 어떻게 트레일 렌더러에 적용 시키고 있는거지? 
    [Serializable] public class ColorChangeEvent : UnityEvent<Color> { }
    public ColorChangeEvent OnColorChange = new ColorChangeEvent();

    public void SetHue(float value)
    {
        value = Mathf.Clamp(value, 0, 1);
        Color newColor = Color.HSVToRGB(value, saturation, value);
        OnColorChange.Invoke(newColor);
    }
}
