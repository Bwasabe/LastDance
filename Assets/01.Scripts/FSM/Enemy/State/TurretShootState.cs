using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShootState : TurretBasicState
{
    public TurretShootState(TurretStateMachine turretStateMachine) : base(turretStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        turret.StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(turret.Data.ShootDelay);

        Vector3 moveDir = (turret.transform.position - turret.BulletPos.position).normalized;
        Quaternion quaternion = Quaternion.LookRotation(moveDir, Vector3.up);

        turret.MuzzleFlash.Play();
        turret.SpawnBullet(turret.BulletPos.position, Quaternion.Euler(0f, quaternion.eulerAngles.y - 90f, 0f));

        yield return new WaitForSeconds(turret.Data.CheckStateDelay);

        CheckState();

    }

    private void CheckState()
    {
        if (stateMachine.Turret.LaserCheck != null)
        {
            if(stateMachine.Turret.LaserCheck.CurrentState())
                turret.StartCoroutine(Shoot());
            else
                stateMachine.ChangeState(stateMachine.PatrolState);
        }
    }
}
