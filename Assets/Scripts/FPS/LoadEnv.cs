using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;


namespace SpatialAnchor
{
    public class LoadEnv : MonoBehaviour
    {
        [SerializeField] EnemyManager enemyManager;

        private LoadMap loadMap;
        private NavMeshSurface surface;

        private void Awake()
        {
            loadMap = FindAnyObjectByType<LoadMap>();
            loadMap.LoadPrevious();

            surface = gameObject.AddComponent<NavMeshSurface>();
            surface.BuildNavMesh();

            enemyManager.gameObject.SetActive(true);
        }
    }
}
