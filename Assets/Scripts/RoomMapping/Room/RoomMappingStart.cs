using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class RoomMappingStart : ClickButton
    {
        [SerializeField] CreateRoom parent;

        public void Next()
        {
            parent.ChildTriggered(0);
        }
    }
}
