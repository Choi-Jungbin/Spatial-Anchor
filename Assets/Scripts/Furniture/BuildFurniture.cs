using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class BuildFurniture : MonoBehaviour
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] CreateFurnitureTop furnitureTop;
        [SerializeField] LineRenderer line;

        public GameObject furniture;

        void Awake()
        {
            furniture = new GameObject("(Furniture)");
            furniture.transform.SetParent(parent.transform);

            Mesh furnitureMesh = new Mesh();
            furnitureMesh.vertices = furnitureTop.edges.ToArray();

            //calculate:
            furnitureMesh.RecalculateNormals();
            furnitureMesh.RecalculateBounds();

            //colliders (no need for mesh colliders - simplify with boxes):
            furniture.AddComponent<BoxCollider>();

            parent.ChildTriggered(4);
        }
    }
}
