using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBase : MonoBehaviour
{
    [Header("기본 값 세팅")]
    public float moveSpeed;
    public Animator anim;
    public CarBase car;

    [SerializeField]
    private Rigidbody2D[] allBodies;
    private bool activated = false;

    public float farDistanceSqr;
    public float checkDistanceSqr;
    public float checkRadius;
    public LayerMask collisionLayer;


    private void Awake()
    {
         // 부모 + 자식 전부 가져오기
        allBodies = GetComponentsInChildren<Rigidbody2D>();

        // 처음엔 전부 Kinematic (애니메이션 상태)
        foreach (var rb in allBodies)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    
    //다른 오브젝트를 참조할떄는 start() 함수를 사용
    void Start()
    {     
        car = FindFirstObjectByType<CarBase>();
    }

    void FixedUpdate()
    {
        //sqrMagnitude는 루트 계산을 하지 않아 magnitude나 Vector3.Distance보다 연산 속도가 빠름
        float sqrDist = (car.transform.position - transform.position).sqrMagnitude;

        if (sqrDist > farDistanceSqr)
        {
            DisableZombie();
            anim.SetBool("Move", false);
        }
        else if(sqrDist < checkDistanceSqr)
        {
            MoveZombie();
            CheckCollider();
        }
        else
        {
            EnableZombie();
        }
    }

    void MoveZombie()
    {
        anim.SetBool("Move", true);
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    void EnableZombie()
    {
        foreach (var rb in allBodies)
        {
            rb.simulated = true;
        }

    }
    void DisableZombie()
    {
        foreach (var rb in allBodies)
        {
            rb.simulated = false;  // 물리 계산 중지
        }
    }

    public void CheckCollider()
    {
        Collider2D hit = Physics2D.OverlapCircle(gameObject.transform.position, checkRadius, collisionLayer);

        if (hit != null)
        {
            ActivateRagdoll();
            anim.enabled = false;
        }
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
}