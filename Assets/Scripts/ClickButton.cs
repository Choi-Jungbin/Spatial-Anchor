using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialAnchor
{
    public class ClickButton : MonoBehaviour
    {
        [SerializeField] GameObject rayPrefab;

        private bool _onButton;
        private OVRCameraRig ovrCameraRig;
        private Transform rightController;
        private GameObject ray;
        private LineRenderer rayRenderer;

        void Awake()
        {
            _onButton = false;

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;

            ray = Instantiate(rayPrefab, transform.position, Quaternion.identity);
            rayRenderer = ray.GetComponent<LineRenderer>();
            rayRenderer.SetPosition(0, transform.position);
            rayRenderer.SetPosition(1, transform.position);
        }

        // Update is called once per frame
        void Update()
        {
            Ray castRay = new Ray(rightController.position, rightController.forward);
            RaycastHit hit;

            if (Physics.Raycast(castRay, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Button"))
                {
                    _onButton = true;
                    transform.position = hit.point;
                }
            }
            else
            {
                transform.position = castRay.origin + rightController.forward * 4f;
            }
            rayRenderer.SetPosition(0, castRay.origin);
            rayRenderer.SetPosition(1, transform.position);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onButton)
                {
                    hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }
}
