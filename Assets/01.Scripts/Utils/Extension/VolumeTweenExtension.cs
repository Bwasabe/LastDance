using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class VolumeTweenExtension
{
    public static TweenerCore<float, float, FloatOptions> DOIntensity(this ChromaticAberration chromaticAberration, float endValue, float duration)
    {
        return DOTween.To(
            () => chromaticAberration.intensity.value,
            value => chromaticAberration.intensity.Override(value),
            endValue, duration);
    }
    
    public static TweenerCore<Vector4, Vector4, VectorOptions> DOLift(this LiftGammaGain liftGammaGain, Vector4 endValue, float duration)
    {
        return DOTween.To(
            () => liftGammaGain.lift.value,
            value => liftGammaGain.lift.Override(value),
            endValue, duration);
    }
    
    public static TweenerCore<Vector4, Vector4, VectorOptions> DOGain(this LiftGammaGain liftGammaGain, Vector4 endValue, float duration)
    {
        return liftGammaGain.gain.value.DOVector(endValue, duration);
    }
    
    public static TweenerCore<Vector4, Vector4, VectorOptions> DOGamma(this LiftGammaGain liftGammaGain, Vector4 endValue, float duration)
    {
        return DOTween.To(
            () => liftGammaGain.gamma.value,
            value => liftGammaGain.gamma.Override(value),
            endValue, duration);
    }
    
    public static TweenerCore<float, float, FloatOptions> DOIntensity(this LensDistortion lensDistortion, float endValue, float duration)
    {
        return DOTween.To(
            () => lensDistortion.intensity.value,
            value => lensDistortion.intensity.Override(value),
            endValue, duration);
    }
    
    public static TweenerCore<float, float, FloatOptions> DOIntensity(this Vignette vignette, float endValue, float duration)
    {
        return DOTween.To(
            () => vignette.intensity.value,
            value => vignette.intensity.Override(value),
            endValue, duration);
    }
    
    public static TweenerCore<float, float, FloatOptions> DOIntensity(this MotionBlur motionBlur, float endValue, float duration)
    {
        return DOTween.To(
            () => motionBlur.intensity.value,
            value => motionBlur.intensity.Override(value),
            endValue, duration);
    }
}