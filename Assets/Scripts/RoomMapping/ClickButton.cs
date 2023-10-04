using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class ClickButton : MonoBehaviour
    {
        [SerializeField] GameObject rayPrefab;

        protected LayerMask layerMask;

        protected bool _onButton;
        protected OVRCameraRig ovrCameraRig;
        protected Transform rightController;
        protected GameObject ray;
        protected LineRenderer rayRenderer;
        protected RaycastHit hit;

        protected void OnEnable()
        {
            _onButton = false;
            layerMask = 1 << LayerMask.NameToLayer("UI");

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;

            ray = Instantiate(rayPrefab, transform.position, Quaternion.identity);
            rayRenderer = ray.GetComponent<LineRenderer>();
            rayRenderer.SetPosition(0, rightController.position);
            rayRenderer.SetPosition(1, transform.position);
        }

        // Update is called once per frame
        protected void Update()
        {
            Ray castRay = new Ray(rightController.position, rightController.forward);

            if (Physics.Raycast(castRay, out hit, 4f, layerMask))
            {
                transform.position = hit.point;
                if (hit.collider.gameObject.CompareTag("Button"))
                {
                    _onButton = true;
                }
                else
                {
                    _onButton = false;
                }
            }
            else
            {
                _onButton = false;
                transform.position = castRay.origin + rightController.forward * 4f;
            }

            rayRenderer.SetPosition(0, castRay.origin);
            rayRenderer.SetPosition(1, transform.position);
        }
    }
}
