using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class GlobalVolume : MonoSingleton<GlobalVolume>
{
    private Volume _volume;

    private ChromaticAberration _chromatic;
    private void Awake()
    {
        _volume = GetComponent<Volume>();
    }

    public bool GetProfile<T>(out T profile) where T : VolumeComponent
    {
        if(_volume.profile.TryGet(out T value))
        {
            profile = value;

            return true;
        }
        else
        {
            profile = value;
            return false;
        }
    }
}