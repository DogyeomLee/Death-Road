using System;
using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
    [Header("좀비 물리 부품")]
    [SerializeField] private Rigidbody2D[] allBodies;
    [SerializeField] private BoxCollider2D boxCollider2D;
    private bool activated = false;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        DeactivateRagdoll();
    }
    void DeactivateRagdoll()
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
