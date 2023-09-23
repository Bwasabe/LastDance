using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField]
    private float _pickupRadius = 3f;
    [SerializeField]
    private LayerMask _itemLayer;
    [SerializeField]
    private Transform _handTransform;

    public Transform HandTransform => _handTransform;

    private CapsuleCollider _capsuleCollider;

    public event Action<ItemBase> OnEquipItem;

    private ItemBase _currentItem;

    private void Start()
    {
        _capsuleCollider = transform.GetComponentCache<CapsuleCollider>();

        ItemBase defaultWeapon = transform.GetComponentCache<DefaultWeapon>();
        
        EquipItem(defaultWeapon);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ItemBase item = GetClosestItem();
            if(item != null)
            {
                EquipItem(item);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            _currentItem.Execute();
        }
    }

    private void EquipItem(ItemBase item)
    {
        if(_currentItem != null)
            _currentItem.OnRemoved();
        
        _currentItem = item;
        
        item.transform.SetParent(_handTransform);
        
        item.IsEquiped = true;

        item.OnEquipItem();
        
        item.transform.SetLocalPositionAndRotation(item.EquipPos, Quaternion.Euler(item.EquipRotation));
        item.transform.localScale = item.EquipScale;
        
        
        OnEquipItem?.Invoke(item);
    }

    

    private ItemBase GetClosestItem()
    {
        Vector3 center = transform.position + _capsuleCollider.center;
        
        // ReSharper disable once Unity.PreferNonAllocApi
        Collider[] colliders = Physics.OverlapSphere(center, _pickupRadius, _itemLayer.value);

        if(colliders.Length == 0) return null;

        float minDistance = float.MaxValue;

        ItemBase closestItem = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider obj = colliders[i];
            
            if(obj.TryGetComponent(out ItemBase item))
            {
                if(item.IsEquiped) continue;

                float distance = Vector3.Distance(obj.transform.position, center);
                if(distance < minDistance)
                {
                    closestItem = item;
                }
            }
        }

        return closestItem;
    }
}