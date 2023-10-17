using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialAnchor
{
    public class AcceptFurniture : ClickButton
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] BuildFurniture build;

        public void Accept()
        {
            parent.Furniture.Add(build.edges);
            build.furnitures.Add(build.furniture);
            parent.ChildTriggered(5);
        }

        public void Reject()
        {
            parent.ChildTriggered(5);
        }
    }
}
