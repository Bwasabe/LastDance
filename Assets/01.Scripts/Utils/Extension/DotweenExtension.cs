using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public static class DotweenExtension
{
    public static TweenerCore<float, float, FloatOptions> DOFloat(this float f, float endValue, float duration)
    {
        return DOTween.To(() => f, value => f = value, endValue,duration);
    }
    
    public static TweenerCore<Vector4, Vector4, VectorOptions> DOVector(this Vector4 v, Vector4 endValue, float duration)
    {
        return DOTween.To(() => v, value => v = value, endValue,duration);
    }
}