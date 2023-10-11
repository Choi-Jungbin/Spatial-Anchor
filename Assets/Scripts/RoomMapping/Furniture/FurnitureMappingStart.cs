using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SpatialAnchor
{
    public class FurnitureMappingStart : ClickButton
    {
        [SerializeField] CreateFurniture parent;

        public void Next()
        {
            parent.ChildTriggered(0);
        }

        public void NextScene()
        {
            parent.ChildTriggered(0, true);
        }
    }
}
