using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationParameter
{
    public string name;

    private readonly int ParameterHash;

    private AnimationController _animationController;

    public AnimationParameter(AnimationController animationController)
    {
        _animationController = animationController;

        ParameterHash = Animator.StringToHash(name);
    }
    
}