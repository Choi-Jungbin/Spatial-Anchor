using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor
{
    public class ProtectorManager : MonoBehaviour
    {
        [SerializeField] GameObject protectorPrefab;

        private OVRCameraRig ovrCameraRig;
        private Transform leftController;
        private List<GameObject> protectors;
        private int _maxCount = 3;
        private bool _touch;
        private GameObject protector;

        void Awake()
        {
            _touch = false;

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            leftController = ovrCameraRig.leftControllerAnchor;

            protectors = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
            {
                CreateProtector();
            }
        }

        private void CreateProtector()
        {
            if(protectors.Count < _maxCount)
            {
                protectors.Add(Instantiate(protectorPrefab, leftController.position, leftController.rotation));
            }
        }

        public void DestroyProtector(GameObject p)
        {
            protectors.Remove(p);
            Destroy(p);
        }
    }
}
