using UnityEngine;

/// <summary>
/// State의 공통 함수 정의
/// </summary>
public interface IState
{
    public void Enter(); // 해당 State의 들어왔는가
    public void Exit(); // 해당 State를 나갔는가
    public void HandleInput(); // 입력
    public void Update(); // 반복
    public void PhysicsUpdate(); // 물리적인 반복(FixedUpdate)
    public void OnTriggerEnter(Collider collider);
    public void OnTriggerExit(Collider collider);
    public void OnCollisionEnter(Collision collision);
    public void OnCollisionExit(Collision collision);
    public void OnDrawGizmos();
}