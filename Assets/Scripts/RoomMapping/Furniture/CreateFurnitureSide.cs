using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class CreateFurnitureSide : MonoBehaviour
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] CreateFurnitureFloor floor;
        [SerializeField] GameObject pointPrefab;
        [SerializeField] GameObject linePrefab;

        private Plane checkTop;
        private Vector3 position;
        private bool _onTop;
        private GameObject point;
        private GameObject line;
        private LineRenderer lineRenderer;
        private OVRCameraRig ovrCameraRig;
        private Transform rightController;

        void OnEnable()
        {
            _onTop = false;

            position = floor.transform.position;
            transform.position = position;
            transform.rotation = floor.transform.rotation;
            checkTop = new Plane(-floor.transform.forward, position);
            point = Instantiate(pointPrefab, transform.position, transform.rotation);

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

            if (checkTop.Raycast(castRay, out castDistance))
            {
                Vector3 endPoint = new Vector3(position.x, castRay.GetPoint(castDistance).y, position.z);
                transform.position = endPoint;

                if (!_onTop)
                {
                    _onTop = true;
                }
            }
            else
            {
                if (_onTop)
                {
                    _onTop = false;
                }
            }
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1, transform.position);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onTop)
                {
                    Destroy(point);
                    Destroy(line);
                    parent.ChildTriggered(2);
                }
            }
        }
    }
}
