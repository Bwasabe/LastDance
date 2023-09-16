using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeathState : TurretBasicState
{
    public TurretDeathState(TurretStateMachine turretStateMachine) : base(turretStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        turret.HeadRigidbody.isKinematic = false;
        turret.HeadRigidbody.useGravity = true;
        turret.HeadRigidbody.AddExplosionForce(400, turret.transform.position, 50);
    }
}
