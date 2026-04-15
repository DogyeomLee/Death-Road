using Unity.VisualScripting;
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

    private void Awake()
    {
        if (zombie == null)
        {
            zombie = GetComponent<ZombieBase>();
        }
    }


    private void OnEnable() // 이벤트 연결
    {
        zombie.OnDestroy += DestoryRandomEFX;
        zombie.OnHit += HitSFX;
    }

    private void OnDisable() // 이벤트 해제 (메모리 누수 방지)
    {
        zombie.OnDestroy -= DestoryRandomEFX;
        zombie.OnHit -= HitSFX;
    }

    private void DestoryRandomEFX()
    {
        //파티클의 위치를 좀비의 위치로
        damageVFX.transform.position = zombie.GetPevisPosition.position;

        damageVFX.SetActive(true);

        int random = Random.Range(0, damageSFX.Length);
        //객체가 사라져도, 소리를 재생함
        SoundManager.Instance.PlaySfxOneShot(damageSFX[random]);
    }

    private void HitSFX(float impact)
    {
        //충돌힘 에 따라 충돌소리가 커지게.
        SoundManager.Instance.ChangeVolme(impact * 0.35f);
        int random = Random.Range(0, hitSFX.Length);
        SoundManager.Instance.PlaySfxOneShot(hitSFX[random]);
    }
}
