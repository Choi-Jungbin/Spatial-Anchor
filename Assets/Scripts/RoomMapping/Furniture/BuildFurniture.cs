using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor
{
    public class BuildFurniture : MonoBehaviour
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] CreateFurnitureTop top;
        [SerializeField] AcceptFurniture accept;
        [SerializeField] Material material;
        [SerializeField] TextMeshPro text;

        private List<Vector3> edges;

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

            GameObject furniture = GameObject.CreatePrimitive(PrimitiveType.Cube);
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

            accept.edges = edges;
            text.text = "edges";
            accept.furniture = furniture;
            text.text = "furniture";

            parent.ChildTriggered(4);
        }
    }
}
