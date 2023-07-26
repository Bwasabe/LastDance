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

        // test�� ����
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
        // �ʱ� ȸ�� ���� ����
        Quaternion initialRotation = stateMachine.Turret.Head.rotation;

        // 0���� 180�� ������ ������ ������ �����մϴ�.
        float targetAngle = Random.Range(turret.Data.MinRotateZ, turret.Data.MaxRotateZ);

        // �����ϰ� ȸ�� ������ �����մϴ�.
        float rotationDirection = Random.value < 0.5f ? -1f : 1f;

        // ȸ���� �ɸ��� �ð��� ����մϴ�.
        float rotationTime = Mathf.Abs(targetAngle / turret.Data.RotationSpeed);

        // ȸ�� ����� ������ ���� ��ǥ ȸ���� ����մϴ�.
        Vector3 targetEulerAngles = initialRotation.eulerAngles.SetZ(0) + new Vector3(0f, 0f, rotationDirection * targetAngle);
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        currentCoroutine = RotateObject(targetRotation, rotationTime);
        stateMachine.Turret.StartCoroutine(currentCoroutine);
    }

    private void SettingRotateX()
    {
        // �ʱ� ȸ�� ���� ����
        Quaternion initialRotation = stateMachine.Turret.Head.rotation;

        float targetAngle = Random.Range(turret.Data.MinRotateX, turret.Data.MaxRotateX);


        // ȸ���� �ɸ��� �ð��� ����մϴ�.
        float rotationTime = Mathf.Abs(targetAngle / turret.Data.RotationSpeed);

        // ȸ�� ����� ������ ���� ��ǥ ȸ���� ����մϴ�.
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

        // ȸ�� �Ϸ� ��, ��Ȯ�� ��ǥ ȸ�� ������ �����մϴ�.
        turretHead.rotation = targetRotation;

        float rand = Random.Range(turret.Data.DelayMaxAgainRotate, turret.Data.DelayMaxAgainRotate);
        yield return new WaitForSeconds(rand);

        ChoiceRotate();
    }
}
