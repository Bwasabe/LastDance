using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        // target과 나 사이의 거리가 radius 보다 작다면
        if (interV.magnitude <= turret.Radius)
        {
            // '타겟-나 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot(interV.normalized, turret.transform.forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= turret.AngleRange / 2f)
                stateMachine.ChangeState(stateMachine.SearchState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Handles.color = _red;
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(turret.transform.position, Vector3.up, turret.transform.forward, turret.AngleRange / 2, turret.Radius);
        Handles.DrawSolidArc(turret.transform.position, Vector3.up, turret.transform.forward, -turret.AngleRange / 2, turret.Radius);
    }
}
