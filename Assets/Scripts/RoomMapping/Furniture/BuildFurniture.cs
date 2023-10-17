using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class BuildFurniture : MonoBehaviour
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] CreateFurnitureTop top;
        [SerializeField] Material material;

        public List<GameObject> furnitures;
        public GameObject furniture;
        public List<Vector3> edges;

        private void Awake()
        {
            furnitures = new List<GameObject>();
        }

        void OnEnable()
        {
            edges = top.edges;
            furniture = new GameObject("Furniture");

            // Calculate the center of the cube
            Vector3 center = Vector3.zero;
            foreach (Vector3 edge in edges)
            {
                center += edge;
            }
            center /= 8;

            furniture = GameObject.CreatePrimitive(PrimitiveType.Cube);
            furniture.transform.position = center;

            Vector3 widthVector = edges[1] - edges[0];
            Vector3 lengthVector = edges[2] - edges[1];
            Vector3 heightVector = edges[4] - edges[0];

            float width = Mathf.Abs(widthVector.magnitude);
            float length = Mathf.Abs(lengthVector.magnitude);
            float height = Mathf.Abs(heightVector.magnitude);

            furniture.transform.localScale = new Vector3(width, height, length);
            furniture.transform.rotation = Quaternion.LookRotation(lengthVector, heightVector);
            furniture.GetComponent<MeshRenderer>().material = material;

            parent.ChildTriggered(4);
        }
    }
}
