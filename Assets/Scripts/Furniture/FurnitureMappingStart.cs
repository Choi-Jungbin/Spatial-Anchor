using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialAnchor
{
    public class FurnitureMappingStart : MonoBehaviour
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] Button yes;
        [SerializeField] Button no;

        private OVRCameraRig ovrCameraRig;

        void Awake()
        {
            yes.onClick.AddListener(Next);
            no.onClick.AddListener(NextScene);

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = ovrCameraRig.transform.position + ovrCameraRig.transform.forward * 2f;
            transform.rotation = Quaternion.LookRotation(ovrCameraRig.transform.forward);
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
