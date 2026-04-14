using UnityEngine;

public class DamagableItem : MonoBehaviour, IDestroyable
{
    [Header("파괴에 필요한 힘")]
    [SerializeField] private float neededPower;

    [Header("효과 설정")]
    [SerializeField] private GameObject damageVFX;
    [SerializeField] private AudioClip[] damageSFX;

    protected bool isDestroyed = false;

    [Header("파편 프리팹 설정")]
    [SerializeField] private GameObject[] fragmentPrefab;

    //오버로드.
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
                Debug.Log($"{rb.name}에 {force} 만큼  힘을 가했습니다");
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
}
