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

        void OnEnable()
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
                if(hit.collider.gameObject == room.Floor)
                {
                    _onFloor = true;
                    point.SetActive(true);
                }
                else
                {
                    _onFloor = false;
                    point.SetActive(false);
                }
                transform.position = hit.point;
            }
            else
            {
                _onFloor = false;
                point.SetActive(false);
                transform.position = castRay.origin + rightController.forward * 4f;
            }

            //position (force to floor):
            Vector3 position = hit.point;
            if (_onFloor)
            {
                position.y = RoomAnchor.Instance.transform.position.y;
            }
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
