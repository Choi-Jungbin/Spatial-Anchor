using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum enemyState
    {
        Walk,
        Dead,
        Search,
        attack

    }

    private Rigidbody[] ragdollRigidbodies;
    private enemyState currentState = enemyState.Search;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        InvokeRepeating("setAnimator", 0, 5);
    }

    void setAnimator()
    {
        if (animator.GetBool("act1"))
        {
            animator.SetBool("act1", false);
        }
        else
        {
            animator.SetBool("act1", true);
        }
    }
}
