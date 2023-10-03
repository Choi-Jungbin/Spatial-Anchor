using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor
{
    public class AcceptRoom : ClickButton
    {
        [SerializeField] CreateRoom parent;
        [SerializeField] TextMeshPro text;
        [SerializeField] LineRenderer line;

        // Update is called once per frame
        new void Update()
        {
            base.Update();
            text.text = hit.collider.gameObject.name;

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
