using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialAnchor
{
    public class AcceptFurniture : ClickButton
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] BuildFurniture furniture;

        public void Accept()
        {
            parent.Furniture.Add(furniture.edges);
            Destroy(furniture.Furniture);
            parent.ChildTriggered(5);
        }

        public void Reject()
        {
            parent.ChildTriggered(5);
        }
    }
}
