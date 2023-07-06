using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerComponentBase : MonoBehaviour
{
    protected PlayerStateController _playerStateController;
    
    protected virtual void Start()
    {
        _playerStateController = transform.GetComponentCache<PlayerStateController>();
    }
}
