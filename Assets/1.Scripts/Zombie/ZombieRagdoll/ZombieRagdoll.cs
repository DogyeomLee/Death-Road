using System;
using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
    [Header("좀비 물리 부품")]
    [SerializeField] private Rigidbody2D[] allBodies;
    [SerializeField] private BoxCollider2D boxCollider2D;
    private bool activated = false;
    

    private void Start()
    {
        foreach (var rb in allBodies)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        DisableZombie();
    }
    void DisableZombie()
    {
        foreach (var rb in allBodies)
        {
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
        }
    }
    void EnableZombie()
    {
        foreach (var rb in allBodies)
        {
            rb.simulated = true;
        }

    }

    private void RemoveBoxColliders()
    {
        boxCollider2D.enabled = false;
    }

    public void DieZombie()
    {
        EnableZombie();
        ActivateRagdoll();
        RemoveBoxColliders();
    }
}
