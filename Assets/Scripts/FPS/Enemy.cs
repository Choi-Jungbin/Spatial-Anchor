using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet_head"))
        {
            animator.SetTrigger("bullet");
        }
    }
}
