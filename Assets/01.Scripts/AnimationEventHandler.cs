using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private readonly Dictionary<string, Action> _playerAnimationEventDict = new();

    protected void OnAnimationExecute(string actionName)
    {
        if(_playerAnimationEventDict.TryGetValue(actionName, out Action action))
            _playerAnimationEventDict[actionName]?.Invoke();
        else
            throw new SystemException("ActionName is wrong please check the parameter");
    }

    public void AddEvent(string actionName, Action action)
    {
        if (_playerAnimationEventDict.TryGetValue(actionName, out Action thisEvent))
        {
            thisEvent += action;
            _playerAnimationEventDict[actionName] = thisEvent;
        }
        else
        {
            _playerAnimationEventDict.Add(actionName, action);
        }
    }
}
