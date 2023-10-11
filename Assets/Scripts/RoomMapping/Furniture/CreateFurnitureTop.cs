using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor
{
    public class CreateFurnitureTop : MonoBehaviour
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] CreateFurnitureSide topAnchor;
        [SerializeField] GameObject pointPrefab;
        [SerializeField] GameObject linePrefab;
        [SerializeField] GameObject rayPrefab;

        public List<Vector3> edges;

        private bool _onTop;
        private Plane top;
        private Vector3 startPoint;
        private Vector3 position;
        private Vector3 lineX;
        private Vector3 lineY;
        private GameObject anchor;
        private GameObject line;
        private LineRenderer lineRenderer;
        private OVRCameraRig ovrCameraRig;
        private Transform rightController;
        private GameObject ray;
        private LineRenderer rayRenderer;

        void OnEnable()
        {
            _onTop = false;

            top = new Plane(Vector3.up, topAnchor.transform.position);
            startPoint = topAnchor.transform.position;
            position = startPoint;
            lineX = startPoint;
            lineY = startPoint;

            anchor = Instantiate(pointPrefab, startPoint, Quaternion.identity);

            line = Instantiate(linePrefab, startPoint, Quaternion.identity);
            lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, startPoint);
            lineRenderer.SetPosition(2, startPoint);
            lineRenderer.SetPosition(3, startPoint);
            lineRenderer.SetPosition(4, startPoint);

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
            float castDistance;

            if (top.Raycast(castRay, out castDistance))
            {
                if (!_onTop)
                {
                    _onTop = true;
                }
                anchor.SetActive(true);
                position = castRay.GetPoint(castDistance);
                anchor.transform.position = position;

                lineX = lineRenderer.GetPosition(1);
                lineX.x = position.x;
                lineY = lineRenderer.GetPosition(3);
                lineY.x = position.y;
                lineRenderer.SetPosition(1, line.transform.InverseTransformPoint(lineX));
                lineRenderer.SetPosition(2, line.transform.InverseTransformPoint(position));
                lineRenderer.SetPosition(3, line.transform.InverseTransformPoint(lineY));
            }
            else
            {
                if (_onTop)
                {
                    _onTop = false;
                }
                anchor.SetActive(false);
            }
            transform.position = position;
            rayRenderer.SetPosition(0, castRay.origin);
            rayRenderer.SetPosition(1, transform.position);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onTop)
                {
                    edges = new List<Vector3> { startPoint, lineX, position, lineY };
                    float bottom = RoomAnchor.Instance.transform.position.y;
                    for (int i = 0; i < 4; i++)
                    {
                        edges.Add(new Vector3(edges[i].x, bottom, edges[i].z));
                    }
                    Destroy(anchor);
                    Destroy(line);
                    parent.ChildTriggered(3);
                }
            }
        }
    }
}
