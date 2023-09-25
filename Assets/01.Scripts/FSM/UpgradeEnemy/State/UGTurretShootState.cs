using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGTurretShootState : UGTurretBasicState
{
    public UGTurretShootState(UGTurretStateMachine turretStateMachine) : base(turretStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        turret.StartCoroutine(Shoot());
    }

    public override void Update()
    {
        base.Update();

        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        Vector3 l_vector = turret.TargetPos.transform.position - stateMachine.Turret.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(l_vector, Vector3.up) * Quaternion.Euler(-90, 0, 0);

        Vector3 angle = targetRotation.eulerAngles;
        angle.y -= stateMachine.Turret.transform.localRotation.eulerAngles.y;

        targetRotation.eulerAngles = angle;

        stateMachine.Turret.Head.transform.localRotation = Quaternion.Slerp(stateMachine.Turret.Head.transform.localRotation, targetRotation, 5 * Time.deltaTime); Quaternion.Euler(-90, 0, 0);
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(turret.WaitTime);

        Vector3 l_vector = (turret.TargetPos.transform.position - stateMachine.Turret.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(l_vector, Vector3.up) * Quaternion.Euler(-90, 0, 0);

        turret.MuzzleFlash.Play();
        turret.SpawnBullet(turret.BulletPos.position, Quaternion.Euler(0f, targetRotation.eulerAngles.y + 90f, Quaternion.LookRotation(l_vector, Vector3.up).eulerAngles.x));

        turret.StartCoroutine(Shoot());
    }
}
