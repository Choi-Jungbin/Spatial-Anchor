using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor {
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] SimpleShoot shoot;
        [SerializeField] EnemyManager enemyManager;

        public int ammo;
        public int score;

        void Awake()
        {
            ammo = 0;
            score = 0;
        }

        // Update is called once per frame
        void Update()
        {
            ammo = shoot.currentammo;
            score = enemyManager.kill;
        }
    }
}
