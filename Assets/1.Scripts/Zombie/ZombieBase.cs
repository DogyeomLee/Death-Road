using System;
using System.ComponentModel;
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

    //범퍼에 닿으면 바로 파괴되게
    [SerializeField] private LayerMask bumperLayer;

    //애니메이션 전용 변수
    protected bool isMoving;

    //파편 폭발 중심점 찾기위한 프로퍼티
    public Transform GetPevisPosition => zombieRagdoll.GetPevisPosition;

    //중복 파괴, 죽음 방지
    protected bool isDestroyed = false;

    private bool isDie = false;

    //좀비 이펙트를 위한 이벤트,
    public event Action OnDestroy;

    public event Action<float> OnHit;

    public event Action OnDie;

    public static event Action OnDieForUI;

    //private void Awake()
    //{
    //    //RequireComponent 를 사용하고 있어서 참조해줄 필요가 없다.(미세한 성능 향상)
    //    //zombieFSMManager = GetComponent<ZombieFSMManager>();
    //    //zombieMovement  = GetComponent<ZombieMovement>();
    //    //zombieRagdoll = GetComponent<ZombieRagdoll>();
    //    //zombieEffect = GetComponent<ZombieEffect>();
    //}

    protected virtual void Update()
    {
        //조건문을 통해 성능 향상
        if (targetCar == null)
        {
            TryFindTargetCar();
            return;
        }

        zombieFSMManager.ChangeStateByCondition(targetCar.transform);
    }

    private void TryFindTargetCar()
    {
        targetCar = GameObject.FindGameObjectWithTag("Car");
    }

    //상속 받을 함수.
    protected virtual void FixedUpdate()
    {
        if (zombieFSMManager.CurrentState == ZombieState.Chase)
        {
            Move();
        }
        else
        {
            zombieMovement.animator.SetBool("isMoving", false); 
        }
    }


    public void DieZombie(float speed)
    {
        if(speed < diePower)
        {
            return;
        }

        isDie = true;
        //죽었을떄 이벤트 발동
        OnDie?.Invoke();

        zombieFSMManager.ChangeStateToDead();
        zombieMovement.DieZombie();
        zombieRagdoll.DieZombie();
    }

    //재정의 할 함수
    protected virtual void Move()
    {
        zombieMovement.MoveZombie(targetCar.transform);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //물리적으로 비교할때는 레이어로, 비트연산으로 선능에 좋다.
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            float impactForce = collision.relativeVelocity.magnitude;

            //죽지 않았을 때만 죽게끔
            if (!isDie)
            {
                DieZombie(impactForce);
            }

            Destroy(impactForce, impactForce * 0.15f);
            OnHit?.Invoke(impactForce);
        }
        if ((bumperLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy();
        }
    }

    public void Destroy(float speed, float force)
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
        OnDieForUI?.Invoke();

        zombieRagdoll.DestoryZombie(force);
        gameObject.SetActive(false);
    }

    //오버로딩
    public void Destroy()
    {
        if (isDestroyed)
        {
            return;
        }

        isDestroyed = true;

        //이벤트 발동
        OnDestroy?.Invoke();
        OnDieForUI?.Invoke();

        zombieRagdoll.DestoryZombie(7f);
        gameObject.SetActive(false);
    }

    void IDestroyable.OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
}