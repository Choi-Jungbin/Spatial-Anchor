using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class Protector : MonoBehaviour
    {
        private ProtectorManager protectorManager;

        void Awake()
        {
            protectorManager = FindObjectOfType<ProtectorManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag == "BulletHead")
            {
                Destroy(col.gameObject);
                protectorManager.DestroyProtector(gameObject);
            }
        }
    }
}
