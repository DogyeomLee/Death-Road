using UnityEngine;

public class DamagableItem : MonoBehaviour, IDestroyable
{
    [Header("파괴 설정")]
    [SerializeField] private float neededPower;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] protected LayerMask targetLayer;

    [Header("효과 설정")]
    [SerializeField] private GameObject damageVFX;
    [SerializeField] private AudioClip[] damageSFX;
    [SerializeField] private AudioClip[] hitSFX;

    protected bool isDestroyed = false;

    [Header("파편 프리팹 설정")]
    [SerializeField] private GameObject[] fragmentPrefab;

    private void Awake()
    {
        isDestroyed = false;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Destory(float speed, float force)
    {
        if (isDestroyed)
        {
            return;
        }

        if (speed < neededPower)
        {
            return;
        }

        isDestroyed = true;

        RandomEFX();
        SetActiveFragments(force);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            float impactForce = collision.relativeVelocity.magnitude;

            HitSFX(impactForce);
            Destory(impactForce, impactForce * 0.15f);
        }
    }

    private void SetActiveFragments(float force)
    {
        foreach(GameObject prefab in fragmentPrefab)
        {
            prefab.SetActive(true);

            prefab.transform.position = transform.position;

            Rigidbody2D rb = prefab.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // 폭발 중심으로부터의 랜덤 방향으로 튕겨나가게 함
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDir * force, ForceMode2D.Impulse);
            }
        }
    }

    private void RandomEFX()
    {
        damageVFX.transform.position = this.transform.position;
        damageVFX.SetActive(true);
        int random = Random.Range(0, damageSFX.Length);
        SoundManager.Instance.PlaySfxOneShot(damageSFX[random]);
    }

    private void HitSFX(float impact)
    {
        int random = Random.Range(0, hitSFX.Length);
        SoundManager.Instance.ChangeVolme(impact * 0.35f);
        SoundManager.Instance.PlaySfxOneShot(hitSFX[random]);
    }

    void IDestroyable.OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
}
