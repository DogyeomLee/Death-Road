using System;
using Unity.VisualScripting;
using UnityEngine;

public class SoliderZombie : ZombieBase
{
    [Header("슈루탄 설정")]
    [SerializeField] private Grenade grenade;
    [SerializeField] private float throwPower;

    private bool isThrow = false;

    public event Action OnThrow;

    protected override void Update()
    {
        base.Update();

        if (zombieFSMManager.CurrentState == ZombieState.Attack)
        {
            if(!isThrow)
            {
                ThrowGrenade();
            }
        }
    }

    private void ThrowGrenade()
    {
        Rigidbody2D grenadeRB = grenade.gameObject.GetComponent<Rigidbody2D>();

        OnThrow?.Invoke();

        grenadeRB.simulated = true;

        float direction = zombieMovement.GetDirection;

        Vector2 divePos = new Vector2(direction, 3);
        //impulse 순간적으로 한순간에 만 힘을 주는 모드
        grenadeRB.AddForce(divePos * throwPower, ForceMode2D.Impulse);

        isThrow = true; 
    }
}
