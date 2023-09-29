using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class CreateWall : MonoBehaviour
    {
        [SerializeField] CreateRoom parent;
        [SerializeField] CreateCeiling ceilingAnchor;
        [SerializeField] GameObject anchorPrefab;
        [SerializeField] GameObject linePrefab;

        private bool _onCeiling;
        private Plane ceiling;
        private Vector3 startPoint;
        private List<Vector3> corners;
        private GameObject anchor;
        private List<GameObject> cornerMarkers;
        private GameObject line;
        private List<GameObject> lines;
        private LineRenderer lineRenderer;
        private OVRCameraRig ovrCameraRig;
        private Transform rightController;

        void Awake()
        {
            _onCeiling = false;
            corners = new List<Vector3>();
            cornerMarkers = new List<GameObject>();
            lines = new List<GameObject>();

            ceiling = new Plane(Vector3.up, ceilingAnchor.transform.position);
            startPoint = parent.corners[0];
            corners.Add(startPoint);

            anchor = Instantiate(anchorPrefab, startPoint, Quaternion.identity);
            cornerMarkers.Add(Instantiate(anchorPrefab, startPoint, Quaternion.identity));

            line = Instantiate(linePrefab, startPoint, Quaternion.identity);
            lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, startPoint);

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;
        }

        private void OnDisable()
        {
            //clean up:
            foreach (var marker in cornerMarkers)
            {
                Destroy(marker);
            }

            foreach (var l in lines)
            {
                Destroy(l);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Ray castRay = new Ray(rightController.position, rightController.forward);
            float castDistance;

            if (ceiling.Raycast(castRay, out castDistance))
            {
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
            transform.position = castRay.GetPoint(castDistance);
            anchor.transform.position = transform.position;
            lineRenderer.SetPosition(1, transform.position);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onCeiling)
                {
                    if(corners.Count > 2 && Vector3.Distance(transform.position, startPoint) < 0.5f)
                    {
                        lines.Add(line);
                        corners.Add(startPoint);
                        cornerMarkers.Add(anchor);
                        parent.corners = corners;
                        parent.ChildTriggered(3);
                    }
                    else
                    {
                        corners.Add(transform.position);
                        cornerMarkers.Add(anchor);
                        anchor = Instantiate(anchorPrefab, transform.position, Quaternion.identity);
                        lines.Add(line);
                        line = Instantiate(linePrefab, startPoint, Quaternion.identity);
                        lineRenderer = line.GetComponent<LineRenderer>();
                        lineRenderer.SetPosition(0, transform.position);
                        lineRenderer.SetPosition(1, transform.position);
                    }
                }
            }
        }
    }
}
