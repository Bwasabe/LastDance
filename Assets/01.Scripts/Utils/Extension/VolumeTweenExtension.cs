using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class VolumeTweenExtension
{
    public static TweenerCore<Vector4, Vector4, VectorOptions> DOVector(this Vector4Parameter parameter, Vector4 endValue, float duration)
    {
        return DOTween.To(
            () => parameter.value,
            parameter.Override,
            endValue, duration);
    }

    public static TweenerCore<float, float, FloatOptions> DOFloat(this FloatParameter parameter, float endValue, float duration)
    {
        return  DOTween.To(
            () => parameter.value,
            parameter.Override,
            endValue, duration);
    }
    
    public static TweenerCore<Color, Color, ColorOptions> DOColor(this ColorParameter parameter, Color endValue, float duration)
    {
        return  DOTween.To(
            () => parameter.value,
            parameter.Override,
            endValue, duration);
    }
    
}