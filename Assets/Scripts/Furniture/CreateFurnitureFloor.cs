using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class CreateFurnitureFloor : MonoBehaviour
    {
        [SerializeField] CreateRoom room;
        [SerializeField] CreateFurniture parent;
        [SerializeField] GameObject pointPrefab;
        [SerializeField] GameObject rayPrefab;

        private bool _onFloor;
        private float _lerpSpeed = 3.5f;
        private OVRCameraRig ovrCameraRig;
        private Transform rightController;
        private GameObject point;
        private GameObject ray;
        private LineRenderer rayRenderer;

        void Awake()
        {
            _onFloor = false;

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;

            point = Instantiate(pointPrefab, transform.position, transform.rotation);
            ray = Instantiate(rayPrefab, transform.position, Quaternion.identity);
            rayRenderer = ray.GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            Ray castRay = new Ray(rightController.position, rightController.forward);
            RaycastHit hit;

            if (Physics.Raycast(castRay, out hit))
            {
                _onFloor = (hit.collider.gameObject == room.Floor);
                point.SetActive(hit.collider.gameObject == room.Floor);
            }
            else
            {
                _onFloor = false;
                point.SetActive(false);
            }

            //position (force to floor):
            Vector3 position = hit.point;
            if (_onFloor)
            {
                position.y = RoomAnchor.Instance.transform.position.y;
            }
            transform.position = position;
            point.transform.position = transform.position;
            rayRenderer.SetPosition(0, castRay.origin);
            rayRenderer.SetPosition(1, transform.position);

            //rotation:
            if (_onFloor)
            {
                Vector3 flatForward = Vector3.ProjectOnPlane(rightController.forward, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flatForward), Time.deltaTime * _lerpSpeed);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(rightController.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _lerpSpeed);
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
