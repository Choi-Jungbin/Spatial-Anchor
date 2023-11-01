using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class IsHit : MonoBehaviour
    {
        private GameObject root;
        private Animator animator;
        private Enemy enemy;
        private EnemyManager enemyManager;

        // Start is called before the first frame update
        void Awake()
        {
            root = GetComponent<Collider>().transform.root.gameObject;
            enemy = root.GetComponent<Enemy>();
            animator = root.GetComponent<Animator>();
            enemyManager = FindObjectOfType<EnemyManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag == "BulletHead" && enemy.alive)
            {
                Destroy(col.gameObject);

                enemy.alive = false;
                animator.enabled = false;

                enemyManager.DestroyEnemy(root);
            }
        }
    }
}
