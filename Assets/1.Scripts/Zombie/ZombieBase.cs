using System;
using UnityEngine;

[RequireComponent(typeof(ZombieFSMManager))]
[RequireComponent(typeof(ZombieMovement))]
[RequireComponent(typeof(ZombieRagdoll))]
[RequireComponent(typeof(ZombieEffect))]
public class ZombieBase : MonoBehaviour, IDestroyable
{
    [Header("죽음에 필요한 힘")]
    [SerializeField] private float diePower;

    [Header("파괴에 필요한 힘")]
    [SerializeField] private float destoryPower;

    [Header("기본 참조 세팅")]
    [SerializeField] protected ZombieMovement zombieMovement;
    [SerializeField] protected ZombieFSMManager zombieFSMManager;
    [SerializeField] protected ZombieRagdoll zombieRagdoll;
    [SerializeField] protected ZombieEffect zombieEffect;

    [Header("타켓")]
    [SerializeField] public GameObject targetCar;
    [SerializeField] protected LayerMask targetLayer;

    //파편 폭발 중심점 찾기위한 프로퍼티
    public Transform GetPevisPosition => zombieRagdoll.GetPevisPosition;

    protected bool isDestroyed = false;

    //좀비 이펙트를 위한 이벤트,
    public event Action OnDestroy;

    public event Action<float> OnHit;

    private void Awake()
    {
        zombieFSMManager = GetComponent<ZombieFSMManager>();
        zombieMovement  = GetComponent<ZombieMovement>();
        zombieRagdoll = GetComponent<ZombieRagdoll>();
        zombieEffect = GetComponent<ZombieEffect>();
    }

    private void Update()
    {
        if (zombieFSMManager.CurrentState == ZombieState.Dead)
        {
            return; 
        }
        if(targetCar == null)
        {
            TryFindTargetCar();
            return;
        }

        zombieFSMManager.ChangeStateByCondition(targetCar.transform);
    }

    //상속 받을 함수.
    protected virtual void FixedUpdate()
    {
        if (zombieFSMManager.CurrentState == ZombieState.Chase)
        {
            Move();
        }
    }

    private void TryFindTargetCar()
    {
        targetCar = GameObject.FindGameObjectWithTag("Car");
    }

    public void DieZombie(float speed)
    {
        if(speed < diePower)
        {
            return;
        }

        zombieFSMManager.ChangeStateToDead();
        zombieMovement.DieZombie();
        zombieRagdoll.DieZombie();
    }

    //재정의 할 함수
    protected virtual void Move()
    {
        zombieMovement.MoveZombie(targetCar.transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //물리적으로 비교할때는 레이어로, 비트연산으로 선능에 좋다.
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            float impactForce = collision.relativeVelocity.magnitude;

            DieZombie(impactForce);
            Destory(impactForce, impactForce * 0.15f);
            OnHit?.Invoke(impactForce);
        }
    }

    public void Destory(float speed, float force)
    {
        if (isDestroyed)
        {
            return;
        }

        if (speed < destoryPower)
        {
            return;
        }

        isDestroyed = true;

        //이벤트 발동
        OnDestroy?.Invoke();

        //DieZombie();
        zombieRagdoll.DestoryZombie(force);
        gameObject.SetActive(false);
    }

    void IDestroyable.OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
}