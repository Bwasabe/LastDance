using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Player_State
{
    Idle = 1 << 0,
    Move = 1 << 1,
    Jump = 1 << 2,
    Attack = 1 << 3,
    Dash = 1 << 4,
    Invincible = 1 << 5,
    WallRunning = 1 << 6,
    Sliding = 1 << 7,
    ReadyToClimb = 1 << 8,
    Climbing = 1 << 9,
}

public class PlayerStateController : MonoBehaviour
{
    
    private Player_State _currentState;
    
#if UNITY_EDITOR
    
    private void Update()
    {
        OnGUIManager.Instance.SetGUI("State : ", _currentState);
    }
    
#endif

    public void AddState(Player_State state)
    {
        _currentState |= state;
    }

    public void RemoveState(Player_State state)
    {
        _currentState &= ~state;
    }

    public bool HasState(Player_State state)
    {
        return _currentState.HasFlag(state);
    }

}
