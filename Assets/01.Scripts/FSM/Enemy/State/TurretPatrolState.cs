using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPatrolState : TurretBasicState
{
    IEnumerator currentCoroutine = null;

    public TurretPatrolState(TurretStateMachine turretStateMachine) : base(turretStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.Turret.LaserRenderer.enabled = true;
        ChoiceRotate();
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.Turret.LaserCheck != null && stateMachine.Turret.LaserCheck.CurrentState())
        {
            if(currentCoroutine != null)
            {
                stateMachine.Turret.StopCoroutine(currentCoroutine);
                stateMachine.Turret.LaserRenderer.enabled = false;
                stateMachine.ChangeState(stateMachine.ShootState);
            }
        }

        // test용 죽음
        if(Input.GetKeyDown(KeyCode.M))
        {
            stateMachine.Turret.StopCoroutine(currentCoroutine);
            stateMachine.ChangeState(stateMachine.DeathState);
        }
    }

    public override void Exit()
    {
        base.Exit();


    }

    public override void PhysicsUpdate()
    {
        
    }

    private void ChoiceRotate()
    {
        if (Random.Range(0, 2) == 0)
        {
            SettingRotateX();
        }
        else
            SettingRotateZ();
    }

    private void SettingRotateZ()
    {
        // 초기 회전 상태 저장
        Quaternion initialRotation = stateMachine.Turret.Head.rotation;

        // 0에서 180도 사이의 랜덤한 각도를 생성합니다.
        float targetAngle = Random.Range(turret.Data.MinRotateZ, turret.Data.MaxRotateZ);

        // 랜덤하게 회전 방향을 선택합니다.
        float rotationDirection = Random.value < 0.5f ? -1f : 1f;

        // 회전에 걸리는 시간을 계산합니다.
        float rotationTime = Mathf.Abs(targetAngle / turret.Data.RotationSpeed);

        // 회전 방향과 각도에 따라서 목표 회전을 계산합니다.
        Vector3 targetEulerAngles = initialRotation.eulerAngles.SetZ(0) + new Vector3(0f, 0f, rotationDirection * targetAngle);
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        currentCoroutine = RotateObject(targetRotation, rotationTime);
        stateMachine.Turret.StartCoroutine(currentCoroutine);
    }

    private void SettingRotateX()
    {
        // 초기 회전 상태 저장
        Quaternion initialRotation = stateMachine.Turret.Head.rotation;

        float targetAngle = Random.Range(turret.Data.MinRotateX, turret.Data.MaxRotateX);


        // 회전에 걸리는 시간을 계산합니다.
        float rotationTime = Mathf.Abs(targetAngle / turret.Data.RotationSpeed);

        // 회전 방향과 각도에 따라서 목표 회전을 계산합니다.
        Vector3 targetEulerAngles = initialRotation.eulerAngles.SetX(0) + new Vector3(targetAngle, 0f, 0f);
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        currentCoroutine = RotateObject(targetRotation, rotationTime);
        stateMachine.Turret.StartCoroutine(currentCoroutine);
    }

    IEnumerator RotateObject(Quaternion targetRotation, float rotationTime)
    {
        Transform turretHead = stateMachine.Turret.Head;
        Quaternion startRotation = turretHead.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            turretHead.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회전 완료 후, 정확히 목표 회전 각도로 설정합니다.
        turretHead.rotation = targetRotation;

        float rand = Random.Range(turret.Data.DelayMaxAgainRotate, turret.Data.DelayMaxAgainRotate);
        yield return Yields.WaitForSeconds(rand);

        ChoiceRotate();
    }
}
