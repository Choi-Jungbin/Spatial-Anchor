using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class AcceptRoom : ClickButton
    {
        [SerializeField] CreateRoom parent;
        [SerializeField] LineRenderer line;

        // Update is called once per frame
        new void Update()
        {
            base.Update();

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onButton)
                {
                    Destroy(ray);
                    line.gameObject.SetActive(false);
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
            parent.ChildTriggered(5);
        }

        public void Reject()
        {
            parent.ChildTriggered(5, true);
        }
    }
}
