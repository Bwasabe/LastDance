using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShootState : TurretBasicState
{
    IEnumerator currentCoroutine = null;

    public TurretShootState(TurretStateMachine turretStateMachine) : base(turretStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // turret.SensorMesh.enabled = false;

        currentCoroutine = Shoot();
        turret.StartCoroutine(Shoot());
    }

    public override void Exit()
    {
        if(currentCoroutine != null)
            stateMachine.Turret.StopCoroutine(currentCoroutine);
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(turret.Data.ShootDelay);

        GameObject target = GameObject.FindAnyObjectByType<PlayerStateController>().gameObject;

        Vector3 l_vector = target.transform.position - stateMachine.Turret.transform.position;
        Debug.Log(Quaternion.LookRotation(l_vector, Vector3.up));
        stateMachine.Turret.Head.transform.localRotation = Quaternion.LookRotation(l_vector, Vector3.up) * Quaternion.Euler(-90, 0, 0);

        Vector3 moveDir = (turret.transform.position - turret.BulletPos.position).normalized;
        Quaternion quaternion = Quaternion.LookRotation(moveDir, Vector3.up);

        turret.MuzzleFlash.Play();
        turret.SpawnBullet(turret.BulletPos.position, Quaternion.Euler(0f, quaternion.eulerAngles.y - 90f, 0f));

        yield return new WaitForSeconds(turret.Data.CheckStateDelay);

        CheckState();

    }

    private void CheckState()
    {
        if (stateMachine.Turret.SensorCheck != null)
        {
            if(stateMachine.Turret.SensorCheck.CurrentState())
            {
                currentCoroutine = Shoot();
                turret.StartCoroutine(currentCoroutine);
            }
            else
                stateMachine.ChangeState(stateMachine.PatrolState);
        }
    }
}
