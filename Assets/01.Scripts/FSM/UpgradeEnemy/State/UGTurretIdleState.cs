using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UGTurretIdleState : UGTurretBasicState
{
    private Color _red = new Color(1f, 0f, 0f, 0.2f);

    public UGTurretIdleState(UGTurretStateMachine turretStateMachine) : base(turretStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        Vector3 interV = Define.Player.transform.position - turret.transform.position;

        // target�� �� ������ �Ÿ��� radius ���� �۴ٸ�
        if (interV.magnitude <= turret.Radius)
        {
            // 'Ÿ��-�� ����'�� '�� ���� ����'�� ����
            float dot = Vector3.Dot(interV.normalized, turret.transform.forward);
            // �� ���� ��� ���� �����̹Ƿ� ���� ����� cos�� ���� ���ؼ� theta�� ����
            float theta = Mathf.Acos(dot);
            // angleRange�� ���ϱ� ���� degree�� ��ȯ
            float degree = Mathf.Rad2Deg * theta;

            // �þ߰� �Ǻ�
            if (degree <= turret.AngleRange / 2f)
                stateMachine.ChangeState(stateMachine.SearchState);
        }
    }
#if UNITY_EDITOR

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Handles.color = _red;
        // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
        Handles.DrawSolidArc(turret.transform.position, Vector3.up, turret.transform.forward, turret.AngleRange / 2, turret.Radius);
        Handles.DrawSolidArc(turret.transform.position, Vector3.up, turret.transform.forward, -turret.AngleRange / 2, turret.Radius);
    }
    #endif
}
