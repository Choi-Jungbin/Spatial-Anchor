using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] EnemyShoot shooter;
        [SerializeField] float shootTime = 3f;

        public bool alive;

        private Animator animator;
        private OVRCameraRig ovrCameraRig;
        private Transform player;
        private EnemyManager enemyManager;

        // Start is called before the first frame update
        void Awake()
        {
            alive = true;
            animator = gameObject.GetComponent<Animator>();
            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            player = ovrCameraRig.centerEyeAnchor;
            enemyManager = FindObjectOfType<EnemyManager>();

            // Shoot method will be called every 3 seconds after an initial delay of 3 seconds.
            InvokeRepeating("Shoot", shootTime, shootTime);
        }

        void Update()
        {
            Vector3 direction = player.position - transform.position;
            direction = Quaternion.Euler(0, 35, 0) * direction;
            transform.forward = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        }

        void Shoot()
        {
            if (alive)
            {
                shooter.Shoot();
            }
        }
    }
}
