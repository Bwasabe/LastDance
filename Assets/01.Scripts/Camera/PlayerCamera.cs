using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance{ get; private set; }

    public CinemachineVirtualCamera VirtualCamera{ get; private set; }
    
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
            Instance = this;

        
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        // SceneManager.sceneLoaded -= OnSceneLoaded;
        // SceneManager.sceneLoaded += OnSceneLoaded;

    }
    // private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    // {
    //     VirtualCamera  = GetComponent<CinemachineVirtualCamera>();
    // }

}
