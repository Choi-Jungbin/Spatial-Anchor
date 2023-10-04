using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class CreateCeiling : MonoBehaviour
    {
        [SerializeField] CreateRoom parent;
        [SerializeField] CreateFloor floor;
        [SerializeField] GameObject linePrefab;

        private Plane checkCeiling;
        private Vector3 position;
        private bool _onCeiling;
        private GameObject line;
        private LineRenderer lineRenderer;
        private OVRCameraRig ovrCameraRig;
        private Transform rightController;

        void OnEnable()
        {
            _onCeiling = false;

            position = floor.transform.position;
            transform.position = position;
            transform.rotation = floor.transform.rotation;
            checkCeiling = new Plane(-floor.transform.forward, position);

            line = Instantiate(linePrefab, position, Quaternion.identity);
            lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1, position);

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;
        }

        // Update is called once per frame
        void Update()
        {
            Ray castRay = new Ray(rightController.position, rightController.forward);
            float castDistance;

            if (checkCeiling.Raycast(castRay, out castDistance))
            {
                Vector3 endPoint = new Vector3(position.x, castRay.GetPoint(castDistance).y, position.z);
                transform.position = endPoint;

                if (!_onCeiling)
                {
                    _onCeiling = true;
                }
            }
            else
            {
                if (_onCeiling)
                {
                    _onCeiling = false;
                }
            }
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1, transform.position);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onCeiling)
                {
                    parent.corners.Add(transform.position);
                    Destroy(line);

                    parent.ChildTriggered(2);
                }
            }
        }
    }
}
