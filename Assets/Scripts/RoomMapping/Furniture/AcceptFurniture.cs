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

        // Update is called once per frame
        new void Update()
        {
            base.Update();

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onButton)
                {
                    Destroy(ray);
                    if (hit.collider.gameObject.name == "Yes")
                    {
                        Accept();
                    }
                    else if (hit.collider.gameObject.name == "No")
                    {
                        Reject();
                    }
                }
            }
        }

        public void Accept()
        {
            parent.Furniture.Add(build.furniture);
            parent.ChildTriggered(5);
        }

        public void Reject()
        {
            parent.ChildTriggered(5);
        }
    }
}
