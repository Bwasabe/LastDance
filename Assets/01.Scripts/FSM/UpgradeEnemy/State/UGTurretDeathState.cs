using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGTurretDeathState : UGTurretBasicState
{
    public UGTurretDeathState(UGTurretStateMachine turretStateMachine) : base(turretStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Àç»ýÁß");
        turret.HeadRigidbody.isKinematic = false;
        turret.HeadRigidbody.useGravity = true;
        turret.HeadRigidbody.AddExplosionForce(400, turret.transform.position, 50);
    }
}
