using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingletonWithoutDontDestroy<GameManager>
{
    public PlayerStateController Player{
        get {
            if(_player == null) _player = FindObjectOfType<PlayerStateController>();

            return _player;
        }
    }

    private PlayerStateController _player;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
