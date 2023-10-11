using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class AcceptRoom : ClickButton
    {
        [SerializeField] CreateRoom parent;
        [SerializeField] LineRenderer line;

        public void Accept()
        {
            line.gameObject.SetActive(false);
            parent.ChildTriggered(5);
        }

        public void Reject()
        {
            line.gameObject.SetActive(false);
            parent.ChildTriggered(5, true);
        }
    }
}
