using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("폭발 기본 설정")]
    [SerializeField] private Transform explosionPos;
    [SerializeField] private float radius;
    [SerializeField] private float explosionForce;
    [SerializeField] private float time;
    [SerializeField] private SoliderZombie soliderZombie;

    [Header("폭발 레이어 설정")]
    [SerializeField] private LayerMask explosionLayer;

    [Header("폭발 효과 설정")]
    [SerializeField] private GameObject boomVFX;
    [SerializeField] private AudioClip[] boomSFX;

    public float GetExplosionForce => explosionForce;

    private void OnEnable()
    {
        soliderZombie.OnThrow += Boom;
    }
    private void OnDisable()
    {
        soliderZombie.OnThrow -= Boom;
    }

    public void Boom()
    {
        this.transform.SetParent(null);
        Invoke("GrenadeExplosion", time);
    }

    public void GrenadeExplosion()
    {
        Explosion(explosionPos.position, radius, explosionForce, explosionForce);
        DestoryVFX();
        this.gameObject.SetActive(false);
    }

    private void AddExplosionForce(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius)
    {
        // 폭발 중심에서 물체까지의 방향과 거리 계산
        Vector2 explosionDir = rb.position - explosionPosition;
        float distance = explosionDir.magnitude;

        //거리 비율 계산 (중심에 가까울수록 힘이 강하게)
        float hitEffect = 1 - (distance / explosionRadius);

        //거리 안에 들어온 경우에만 힘 가하기
        if (hitEffect > 0)
        {
            // 정규화된 방향 벡터에 힘과 거리 비율을 곱함
            rb.AddForce(explosionDir.normalized * explosionForce * hitEffect, ForceMode2D.Impulse);
        }
    }

    private void Explosion(Vector2 position, float radius, float force, float speed)
    {
        // 원형 영역 내의 모든 Collider2D를 감지
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, explosionLayer);

        foreach (Collider2D hit in colliders)
        {
            //자기 자신은 폭발 대상에서 제외
            if (hit.gameObject == this.gameObject)
            {
                continue;
            }

            IDestroyable target = hit.GetComponentInParent<IDestroyable>();

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (target != null && rb != null)
            {
                // 폭발력 전달 및 대상 파괴
                // AddExplosionForce를 재사용하거나 여기서 직접 계산
                AddExplosionForce(rb, force, position, radius);
                target.Destory(speed, force);
            }
            //파편 전용
            else if (rb != null)
            {
                AddExplosionForce(rb, force, position, radius);
            }
        }
    }

    private void DestoryVFX()
    {
        //파티클의 위치를 좀비의 위치로
        boomVFX.transform.position = this.transform.position;
        boomVFX.SetActive(true);

        int boomRandom = Random.Range(0, boomSFX.Length);
        //객체가 사라져도, 소리를 재생함
        SoundManager.Instance.PlaySfxOneShot(boomSFX[boomRandom], 0.5f);
    }
}
