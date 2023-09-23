using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGTurretStateMachine : StateMachine
{
    public UpgradeTurretController Turret { get; }

    public UGTurretIdleState IdleState { get; }
    public UGTurretShootState SearchState { get; }

    public UGTurretStateMachine(UpgradeTurretController turret)
    {
        Turret = turret;

        IdleState = new UGTurretIdleState(this);
        SearchState = new UGTurretShootState(this);
    }
}
