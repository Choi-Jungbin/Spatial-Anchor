using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsHit
{
    public class IsHit : MonoBehaviour
    {
        Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag == "bullet_head")
            {
                GameObject enemy = GetComponent<Collider>().transform.root.gameObject;
                animator = enemy.GetComponent<Animator>();
                Destroy(col.gameObject);
                animator.SetTrigger("bullet");
                Debug.Log("shoot");
                Destroy(enemy.gameObject, 5f);
            }
        }
    }
}
