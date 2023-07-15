using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private bool _visible = false;

    [SerializeField]
    private CursorLockMode _cursorLockMode;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    [ContextMenu("SetCursorVisible")]
    private void SetCursorVisible()
    {
        Cursor.visible = _visible;
    }
    
    [ContextMenu("SetCursorLockState")]
    private void SetCursorLockState()
    {
        Cursor.lockState = _cursorLockMode;
    }
}
