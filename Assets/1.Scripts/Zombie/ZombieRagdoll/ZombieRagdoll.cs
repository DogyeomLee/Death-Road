using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
    [Header("좀비 물리 부품")]
    [SerializeField] public Rigidbody2D[] allBodies;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Rigidbody2D thisBody2D;

    [Header("좀비 파편 폭발 중심위치")]
    [SerializeField] private Transform pevisPos;

    [Header("파편 프리팹 설정")]
    [SerializeField] private GameObject[] fragmentPrefab;

    private bool activated = false;

    public Transform GetPevisPosition => pevisPos;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        thisBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        DeactivateRagdoll();
        pevisPos = allBodies[6].transform;
    }

    private void DeactivateRagdoll()
    {
        foreach (var rb in allBodies)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false; 
        }
    }

    private void ActivateRagdoll()
    {
        if (activated)
        {
            return;
        }

        activated = true;

        foreach (var rb in allBodies)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }
    }

    public void DestoryZombie(float force)
    {
        SetActiveFragments(force);
    }

    //파괴시 파편으로, force 는 흩뿌러지는 정도.
    private void SetActiveFragments(float force)
    {
        foreach (GameObject prefab in fragmentPrefab)
        {
            prefab.SetActive(true);

            //레깅돌 pevis 를 중심으로 해서 파편 폭발 중심점 잡기.
            prefab.transform.position = pevisPos.position;

            Rigidbody2D rb = prefab.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // 폭발 중심으로부터의 랜덤 방향으로 튕겨나가게 함
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDir * force, ForceMode2D.Impulse);
                Debug.Log($"{rb.name}에 {force} 만큼  힘을 가했습니다");
            }
        }
    }

    //박스 콜라이더는 초기에 차량과의 충돌을 위한것
    private void DisableColliderRigidbody()
    {
        //리지드 바디를 끄지 않으면 계속해서 추락한다. 그래서 파편을 활성화 하면, 계속 추락된 포지션에서 활성화된다.
        boxCollider2D.enabled = false;
        thisBody2D.simulated = false;
    }

    public void DieZombie()
    {
        ActivateRagdoll();
        DisableColliderRigidbody();
    }
}
