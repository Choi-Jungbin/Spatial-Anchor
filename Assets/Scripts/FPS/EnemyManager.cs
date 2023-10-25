using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpatialAnchor
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] GameObject enemyPrefab;
        [SerializeField] int numberOfEnemy = 1;

        public List<GameObject> enemys;
        public int kill;

        private OVRCameraRig ovrCameraRig;
        private int count;

        void OnEnable()
        {
            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            enemys = new List<GameObject>();
            kill = 0;
            count = 0;

            for (int i = 0; i < numberOfEnemy; i++)
            {
                CreateEnemy();
            }
        }

        void OnDisable()
        {
            foreach (GameObject enemy in enemys)
            {
                Destroy(enemy);
            }
            enemys.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            if(enemys.Count < numberOfEnemy)
            {
                CreateEnemy();
            }
        }

        private void CreateEnemy()
        {
            Vector3 randomDirection = Random.insideUnitSphere * 20f;
            randomDirection += ovrCameraRig.transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas))
            {
                if (Vector3.Distance(ovrCameraRig.transform.position, hit.position) > 2f)
                {
                    enemys.Add(Instantiate(enemyPrefab, hit.position, Quaternion.identity));
                }
                else
                {
                    CreateEnemy(); // If too close to player, try again.
                }
            }
        }

        public void DestroyEnemy(GameObject e)
        {
            kill += 1;
            Destroy(e, 5f);

            enemys.Remove(e);
        }
    }
}
