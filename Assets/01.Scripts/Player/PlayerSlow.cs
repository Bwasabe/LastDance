using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlow : PlayerComponentBase
{
    [SerializeField]
    private float _slowDuration = 5f;
    [SerializeField]
    private float _timeScale = 0.2f;
    [SerializeField]
    private float _slowChargeDuration = 10f;

    private float _timer;
    private float _originScale;

    protected override void Start()
    {
        base.Start();

        _timer = 1f;
    }


    private void Update()
    {
        // Slow를 쓰는 동안 깎인다
        if(_playerStateController.HasState(Player_State.Slow))
        {
            if(_timer < 0)
            {
                _timer = 0f;
                return;
            }

            _timer -= Time.unscaledDeltaTime * _slowDuration;
        }
        else
        {
            if(_timer > 1f)
            {
                _timer = 1f;
                return;
            }

            _timer += Time.unscaledDeltaTime * _slowChargeDuration;
        }
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(_playerStateController.HasState(Player_State.Slow))
            {
                EndSlow();                
            }
            else
            {
                StartSlow();
            }
        }
        
        
    }

    private void StartSlow()
    {
        _playerStateController.AddState(Player_State.Slow);
        
        // TimeScale을 Smooth하게 만들자
        _originScale = Time.timeScale;
        Time.timeScale = _timeScale;

        _timer = 0f;
        
    }

    private void EndSlow()
    {
        _playerStateController.RemoveState(Player_State.Slow);
        
        Time.timeScale = _originScale;
        
    }
}
