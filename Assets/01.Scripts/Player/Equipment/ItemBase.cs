using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public bool IsEquiped{ get; set; }

    [field: SerializeField] public Vector3 EquipPos{ get; private set; } = Vector3.zero;
    [field: SerializeField] public Vector3 EquipRotation{ get; private set; } = Vector3.zero;
    [field: SerializeField] public Vector3 EquipScale{ get; set; } 


    public virtual void OnEquipItem()
    {
        // 파티클을 띄운다던지 그런 느낌
    }

    public abstract void Execute();

    public virtual void OnRemoved() {}
}
