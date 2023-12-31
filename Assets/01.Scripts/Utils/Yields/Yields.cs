/**
     Couroutine 사용시 return Yield return의 Object가 동적으로 생성되는 것 같다.
    반복적인 생성이 GC를 생성하기 때문에 캐싱하여 사용한다.
*/
using UnityEngine;
using System.Collections.Generic;
using System;

public static class Yields {
    //WaitForEndOfFrame을 만들어 두고 반환
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    //WaitForFixedUpdate를 만들어 두고 반환
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    //WaitForSeconds 캐싱을 위해 정보를 담아두기 위한 자료구조
    private static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>();

    //WaitForSeconds 정보를 만들어 두고 반환
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_timeInterval.TryGetValue(seconds, out WaitForSeconds waitForSeconds))
            _timeInterval.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
        return waitForSeconds;
    }

    //WaitForRealtime
    private static Dictionary<float, WaitForSecondsRealtime> _realTimeInterval = new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds){
        if (!_realTimeInterval.TryGetValue(seconds, out WaitForSecondsRealtime waitForSecondsRealtime))
            _realTimeInterval.Add(seconds, waitForSecondsRealtime = new WaitForSecondsRealtime(seconds));
        return waitForSecondsRealtime;
    }

}