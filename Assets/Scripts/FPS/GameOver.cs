using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] PlayManager playManager;

        void Awake()
        {

        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("BulletHead"))
            {
                playManager.GameOver();
            }
        }
    }
}
