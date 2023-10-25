using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor {
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] SimpleShoot shoot;
        [SerializeField] EnemyManager enemyManager;
        [SerializeField] GameObject gamePlay;
        [SerializeField] GameObject gameOver;

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

        public void GameOver()
        {
            gamePlay.SetActive(false);
            enemyManager.gameObject.SetActive(false);
            gameOver.SetActive(true);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
