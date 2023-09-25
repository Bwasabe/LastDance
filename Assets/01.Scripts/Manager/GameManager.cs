using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{ get; private set; }
    
    
    public PlayerStateController Player{
        get {
            if(_player == null) _player = FindObjectOfType<PlayerStateController>();

            return _player;
        }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
            Instance = this;
        
    }

    private void Start()
    {
        SceneManager.sceneLoaded -= ResetPlayer;
        SceneManager.sceneLoaded += ResetPlayer;
    }
    private void ResetPlayer(Scene arg0, LoadSceneMode arg1)
    {
        _player = null;
    }

    private PlayerStateController _player;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            TimeManager.Instance.StopAllCoroutines();
            Time.timeScale = 1f;
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
