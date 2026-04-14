using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
    [Header("좀비 물리 부품")]
    [SerializeField] private Rigidbody2D[] allBodies;
    [SerializeField] private BoxCollider2D boxCollider2D;

    [Header("파편 프리팹 설정")]
    [SerializeField] private GameObject[] fragmentPrefab;

    private bool activated = false;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        DeactivateRagdoll();
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
        ActivateRagdoll();
    }

    private void SetActiveFragments(float force)
    {
        foreach (GameObject prefab in fragmentPrefab)
        {
            prefab.SetActive(true);

            prefab.transform.position = transform.position;

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
    //박스 콜라이더를 꺼야, 레깅돌 과의 충돌이 사실적이다.
    private void DisableCollider()
    {
        boxCollider2D.enabled = false;
    }

    public void DieZombie()
    {
        ActivateRagdoll();
        DisableCollider();
    }
}
