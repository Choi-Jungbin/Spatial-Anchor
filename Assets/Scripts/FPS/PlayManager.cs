using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor {
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] SimpleShoot shoot;
        [SerializeField] EnemyManager enemyManager;
        [SerializeField] GameObject gamePlay;
        [SerializeField] GameObject gameOver;
        [SerializeField] GameObject gun;
        [SerializeField] Transform damageEffect;

        public int ammo;
        public int score;

        private int _maxShot = 2;
        private int shot;
        private List<Transform> damageEffects;

        void Awake()
        {
            ammo = 0;
            score = 0;
            shot = 0;

            damageEffects = new List<Transform>();
            for (int i = 0; i < _maxShot; i++)
            {
                damageEffects.Add(damageEffect.GetChild(i));
            }
        }

        // Update is called once per frame
        void Update()
        {
            ammo = shoot.currentammo;
            score = enemyManager.kill;
        }

        public void Shot()
        {
            if(shot < _maxShot)
            {
                damageEffects[shot].gameObject.SetActive(true);
                shot++;
            }
            else
            {
                GameOver();
            }
        }

        void GameOver()
        {
            foreach(Transform effect in damageEffects)
            {
                effect.gameObject.SetActive(false);
            }
            gun.SetActive(false);
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
