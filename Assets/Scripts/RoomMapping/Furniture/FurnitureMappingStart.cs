using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class FurnitureMappingStart : ClickButton
    {
        [SerializeField] CreateFurniture parent;

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
                        Next();
                    }
                    else if (hit.collider.gameObject.name == "No")
                    {
                        NextScene();
                    }
                }
            }
        }

        public void NextScene()
        {
            parent.ChildTriggered(0, true);
        }

        public void Next()
        {
            parent.ChildTriggered(0);
        }
    }
}
