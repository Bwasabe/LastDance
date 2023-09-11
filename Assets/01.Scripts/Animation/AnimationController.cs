using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Animator _animator;

    private Dictionary<Type, AnimationParameter> Parameters = new();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    
}
