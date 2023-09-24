using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    public static float PlayerTimeScale{ get; set; } = 1f;

    public void StopTime(float duration, float timeScale = 0f)
    {
        float prevTimeScale = Time.timeScale;

        Time.timeScale = timeScale;

        StartCoroutine(Timer(duration, prevTimeScale));
    }

    private IEnumerator Timer(float duration, float prevTimeScale)
    {
        yield return Yields.WaitForSeconds(duration);

        Time.timeScale = prevTimeScale;
    }
}
