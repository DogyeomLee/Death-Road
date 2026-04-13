using UnityEngine;

public class DamagableItem : MonoBehaviour, IDamagable
{
    [Header("파괴에 필요한 힘")]
    [SerializeField] private float neededPower;

    [Header("부분 물리 설정")]
    [SerializeField] private Rigidbody2D[] allPart;

    [Header("효과 설정")]
    [SerializeField] private GameObject damageVFX;
    [SerializeField] private AudioClip[] damageSFX;


    protected bool isDestroyed = false;


    protected virtual void Start()
    {
        DeactivatePart();
    }
    public virtual void Destory(float speed)
    {
        if (isDestroyed)
        {
            return;
        }

        if (speed <= neededPower)
        {
            return;
        }

        isDestroyed = true;

        ActivatePart();
        RandomSFX();
        IgnoreLayer();

        Destroy(gameObject, 5f);
    }

    private void DeactivatePart()
    {
        foreach (var part in allPart)
        {
            part.bodyType = RigidbodyType2D.Kinematic;
            part.simulated = false;
        }
    }

    private void ActivatePart()
    {
        foreach (var part in allPart)
        {
            part.bodyType = RigidbodyType2D.Dynamic;
            part.simulated = true;
        }
    }

    private void RandomSFX()
    {
        damageVFX.SetActive(true);
        int random = Random.Range(0, damageSFX.Length);
        SoundManager.Instance.PlaySfxOneShot(damageSFX[random]);
    }

    private void IgnoreLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("IgnoreCar");
    }
}
