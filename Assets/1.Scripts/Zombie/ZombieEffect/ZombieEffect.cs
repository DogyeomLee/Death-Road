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
    [SerializeField] private AudioClip[] bloodSFX;

        
    private void Awake()
    {
        if (zombie == null)
        {
            zombie = GetComponent<ZombieBase>();
        }
    }


    private void OnEnable() // 이벤트 연결
    {
        zombie.OnDestroy += DestoryVFX;
        zombie.OnHit += HitSFX;
        zombie.OnDie += DieRandomSFX;
    }

    private void OnDisable() // 이벤트 해제 (메모리 누수 방지)
    {
        zombie.OnDestroy -= DestoryVFX;
        zombie.OnHit -= HitSFX;
        zombie.OnDie -= DieRandomSFX;
    }

    //파괴 됬을떄
    private void DestoryVFX()
    {
        //파티클의 위치를 좀비의 위치로
        damageVFX.transform.position = zombie.GetPevisPosition.position;
        damageVFX.SetActive(true);

        int bloodRandom = Random.Range(0, bloodSFX.Length);
        //객체가 사라져도, 소리를 재생함
        SoundManager.Instance.PlaySfxOneShot(bloodSFX[bloodRandom], 1);
    }

    //죽었을때 만.
    private void DieRandomSFX()
    {
        int damageRandom = Random.Range(0, damageSFX.Length);
        //객체가 사라져도, 소리를 재생함
        SoundManager.Instance.PlaySfxOneShot(damageSFX[damageRandom], 1);
    }

    //충돌했을때
    private void HitSFX(float impact)
    {
        int hitRandom = Random.Range(0, hitSFX.Length);
        //충돌에 따라 소리 조절, 두번쨰 매개변수
        SoundManager.Instance.PlaySfxOneShot(hitSFX[hitRandom], (impact * 0.35f));
    }

}
