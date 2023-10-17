using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] EnemyShoot shooter;

        public bool alive;

        private Animator animator;
        private OVRCameraRig ovrCameraRig;
        private EnemyManager enemyManager;

        // Start is called before the first frame update
        void Awake()
        {
            alive = true;
            animator = gameObject.GetComponent<Animator>();
            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            enemyManager = FindObjectOfType<EnemyManager>();
        }

        private void Update()
        {
            transform.forward = Vector3.ProjectOnPlane((ovrCameraRig.transform.position - transform.position), Vector3.up).normalized;
        }

        void Shoot()
        {
            shooter.Shoot();
        }
    }
}
