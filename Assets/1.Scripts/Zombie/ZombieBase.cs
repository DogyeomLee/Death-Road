using UnityEngine;

[RequireComponent(typeof(ZombieFSMManager))]
[RequireComponent(typeof(ZombieMovement))]
public class ZombieBase : MonoBehaviour
{
    [Header("기본 참조 세팅")]
    [SerializeField] protected ZombieMovement zombieMovement;
    [SerializeField] protected ZombieFSMManager zombieFSMManager;

    [SerializeField] private Rigidbody2D[] allBodies;
    [SerializeField] protected GameObject targetCar;
    private bool activated = false;

    public LayerMask collisionLayer;


    private void Awake()
    {
        zombieFSMManager = GetComponent<ZombieFSMManager>();
        zombieMovement  = GetComponent<ZombieMovement>();

        // 처음엔 전부 Kinematic (애니메이션 상태)
        foreach (var rb in allBodies)
        {
            rb.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
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

    public void ActivateRagdoll()
    {
        if (activated) return;
        activated = true;

        foreach (var rb in allBodies)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    //재정의 할 함수.
    protected virtual void Move()
    {
        zombieMovement.MoveZombie(targetCar.transform);
    }
}