using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class CreateFloor : MonoBehaviour
    {
        [SerializeField] CreateRoom parent;
        [SerializeField] GameObject pointPrefab;
        [SerializeField] GameObject rayPrefab;

        private bool _onFloor;
        private float _maxCastDistance = 6f;
        private Vector3 roomAnchor;
        private OVRCameraRig ovrCameraRig;
        private Transform rightController;
        private Plane floor;
        private GameObject point;
        private GameObject ray;
        private LineRenderer rayRenderer;

        void OnEnable()
        {
            _onFloor = false;

            floor = new Plane(Vector3.up, RoomAnchor.Instance.transform.position);

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;
            point = Instantiate(pointPrefab, transform.position, transform.rotation);

            ray = Instantiate(rayPrefab, transform.position, Quaternion.identity);
            rayRenderer = ray.GetComponent<LineRenderer>();
            rayRenderer.SetPosition(0, transform.position);
            rayRenderer.SetPosition(1, transform.position);
        }

        // Update is called once per frame
        void Update()
        {
            float castDistance;
            Ray castRay = new Ray(rightController.position, rightController.forward);

            if (floor.Raycast(castRay, out castDistance))
            {
                _onFloor = true;
                point.SetActive(true);
            }
            else
            {
                _onFloor = false;
                point.SetActive(false);
            }

            //clamp:
            if (castDistance <= 0 || castDistance > _maxCastDistance)
            {
                castDistance = _maxCastDistance;
            }

            //position (force to floor):
            Vector3 position = castRay.GetPoint(castDistance);
            if (_onFloor)
            {
                position.y = roomAnchor.y;
            }
            transform.position = position;
            point.transform.position = transform.position;
            rayRenderer.SetPosition(0, castRay.origin);
            rayRenderer.SetPosition(1, transform.position);

            //rotation:
            if (_onFloor)
            {
                Vector3 flatForward = Vector3.ProjectOnPlane(rightController.forward, Vector3.up);
                transform.rotation = Quaternion.LookRotation(flatForward);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(rightController.forward);
                transform.rotation = rotation;
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onFloor)
                {
                    Destroy(ray);
                    Destroy(point);
                    parent.ChildTriggered(1);
                }
            }
        }
    }
}
