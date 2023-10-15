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

        public GameObject Furniture;
        public List<Vector3> edges;

        void OnEnable()
        {
            edges = top.edges;

            // Calculate the center of the cube
            Vector3 center = Vector3.zero;
            foreach (Vector3 edge in edges)
            {
                center += edge;
            }
            center /= 8;

            Furniture = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Furniture.transform.position = center;

            Vector3 widthVector = edges[1] - edges[0];
            Vector3 lengthVector = (edges[2] - edges[0]) + (edges[6] - edges[4]);
            Vector3 heightVector = edges[4] - edges[0];

            float width = Mathf.Abs(widthVector.magnitude);
            float length = Mathf.Abs(lengthVector.magnitude);
            float height = Mathf.Abs(heightVector.magnitude);

            Furniture.transform.localScale = new Vector3(width, height, length);
            Furniture.transform.rotation = Quaternion.LookRotation(lengthVector, heightVector);
            Furniture.GetComponent<MeshRenderer>().material = material;

            parent.ChildTriggered(4);
        }
    }
}
