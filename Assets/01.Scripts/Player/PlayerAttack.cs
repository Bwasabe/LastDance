using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerComponentBase
{
    private readonly int attackHash = Animator.StringToHash("Attack");

    private Animator playerAnimator;

    [SerializeField]
    private float _attackDuration = 0.5f;

    private float _timer;
    protected override void Start()
    {
        base.Start();

        playerAnimator = transform.GetComponentCache<Animator>();
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        _timer += Time.deltaTime;

        if(_timer >= _attackDuration)
        {
            if(Input.GetMouseButtonDown(0))
            {
                _timer = 0f;
                
                playerAnimator.SetTrigger(attackHash);
            }
        }
    }
}
