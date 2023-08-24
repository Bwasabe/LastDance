using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private bool _visible = false;

    [SerializeField]
    private CursorLockMode _cursorLockMode = CursorLockMode.Locked;
    private void Start()
    {
/*        Cursor.visible = _visible;
        Cursor.lockState = _cursorLockMode;*/
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
