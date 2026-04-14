using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ZombieEffect : MonoBehaviour
{
    [Header("기본 참조 요소들")]
    [SerializeField] private ZombieBase zombie;

    [Header("효과 설정")]
    [SerializeField] private GameObject damageVFX;
    [SerializeField] private AudioClip[] damageSFX;
    [SerializeField] private AudioClip[] hitSFX;
    [SerializeField] private AudioSource zombieAudio;

    private void Awake()
    {
        if (zombie == null)
        {
            zombie = GetComponent<ZombieBase>();
        }
        zombieAudio = GetComponent<AudioSource>();
    }

    private void OnEnable() // 이벤트 연결
    {
        zombie.OnDestroy += DestoryRandomEFX;
        zombie.OnDie += HitSFX;
    }

    private void OnDisable() // 이벤트 해제 (메모리 누수 방지)
    {
        zombie.OnDestroy -= DestoryRandomEFX;
        zombie.OnDie -= HitSFX;
    }

    private void DestoryRandomEFX()
    {
        damageVFX.SetActive(true);
        int random = Random.Range(0, damageSFX.Length);
        //객체가 사라져도, 소리를 재생함
        SoundManager.Instance.PlaySfxOneShot(damageSFX[random]);
    }

    private void HitSFX()
    {
        int random = Random.Range(0, hitSFX.Length);
        zombieAudio.PlayOneShot(hitSFX[random]);
    }
}
