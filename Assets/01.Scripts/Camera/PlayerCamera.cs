using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerCamera : MonoSingletonWithoutDontDestroy<PlayerCamera>
{
    public CinemachineVirtualCamera VirtualCamera{ get; private set; }
    
    private void Awake()
    {
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
}
