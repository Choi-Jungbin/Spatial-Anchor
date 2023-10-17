using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor {
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] SimpleShoot shoot;
        [SerializeField] EnemyManager enemyManager;
        [SerializeField] TextMeshProUGUI ammo;
        [SerializeField] TextMeshProUGUI score;

        void Awake()
        {
            ammo.text = "0";
            score.text = "0";
        }

        // Update is called once per frame
        void Update()
        {
            ammo.text = shoot.currentammo.ToString();
            score.text = enemyManager.kill.ToString();
        }
    }
}
