using UnityEngine;

[RequireComponent(typeof(ZombieFSMManager))]
[RequireComponent(typeof(ZombieMovement))]
[RequireComponent(typeof(ZombieRagdoll))]
public class ZombieBase : MonoBehaviour
{
    [Header("기본 참조 세팅")]
    [SerializeField] protected ZombieMovement zombieMovement;
    [SerializeField] protected ZombieFSMManager zombieFSMManager;
    [SerializeField] protected ZombieRagdoll zombieRagdoll;

    [Header("타켓 차량")]
    [SerializeField] protected GameObject targetCar;
    [SerializeField] protected LayerMask targetLayer;

    private void Awake()
    {
        zombieFSMManager = GetComponent<ZombieFSMManager>();
        zombieMovement  = GetComponent<ZombieMovement>();
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

    public void DieZombie()
    {
        zombieFSMManager.ChangeStateToDead();
        zombieMovement.DieZombie();
        zombieRagdoll.DieZombie();
    }

    //재정의 할 함수.
    protected virtual void Move()
    {
        zombieMovement.MoveZombie(targetCar.transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //물리적으로 비교할때는 레이어로, 비트연산으로 선능에 좋다.
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            DieZombie();
        }
    }
}