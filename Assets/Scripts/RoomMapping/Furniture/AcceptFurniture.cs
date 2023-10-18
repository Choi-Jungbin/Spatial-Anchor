using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class AcceptFurniture : ClickButton
    {
        [SerializeField] CreateFurniture parent;

        public List<Vector3> edges;
        public GameObject furniture;

        public void Accept()
        {
            parent.Furniture.Add(edges);
            parent.furnitures.Add(furniture);
            parent.ChildTriggered(5);
        }

        public void Reject()
        {
            Destroy(furniture);
            parent.ChildTriggered(5);
        }
    }
}
