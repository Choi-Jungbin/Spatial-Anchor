using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class GameOver : MonoBehaviour
    {

        void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("BulletHead"))
            {

            }
        }
    }
}
