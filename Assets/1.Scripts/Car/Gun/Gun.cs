using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Gun : MonoBehaviour
{
    private List<ZombieBase> zombies = new List<ZombieBase>();

    [Header("공격 설정")]
    [SerializeField] private float reloadTime;
    private float currentCoolTime;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private Animator animator;

    [Header("소리 설정")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gunSFX;

    private void Update()
    {
        currentCoolTime += Time.deltaTime;

        if (currentCoolTime >= reloadTime && zombies.Count > 0)
        {
            FireGun();
        }
    }

    private void FireGun()
    {
        animator.SetTrigger("Fire");

        int randomSFX = Random.Range(0, gunSFX.Length);

        audioSource.PlayOneShot(gunSFX[randomSFX]);

         ZombieBase firstZombie = zombies[0];

        if (firstZombie != null)
        {
            firstZombie.Destroy();

            // 발사 성공 시 쿨타임 초기화
            currentCoolTime = 0;

            zombies.Remove(firstZombie);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((zombieLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.TryGetComponent(out ZombieBase zombie))
            {
                zombies.Add(zombie);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((zombieLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.TryGetComponent(out ZombieBase zombie))
            {
                zombies.Remove(zombie);
            }
        }
    }
}
