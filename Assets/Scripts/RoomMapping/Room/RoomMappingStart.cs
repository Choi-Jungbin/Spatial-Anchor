using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class RoomMappingStart : ClickButton
    {
        [SerializeField] CreateRoom parent;

        // Update is called once per frame
        new void Update()
        {
            base.Update();

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onButton)
                {
                    Destroy(ray);
                    Next();
                }
            }
        }

        public void Next()
        {
            parent.ChildTriggered(0);
        }
    }
}
