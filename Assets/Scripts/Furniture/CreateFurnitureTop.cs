using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class CreateFurnitureTop : MonoBehaviour
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] CreateFurnitureSide topAnchor;
        [SerializeField] GameObject pointPrefab;
        [SerializeField] GameObject linePrefab;

        public List<Vector3> edges;

        private bool _onTop;
        private Plane top;
        private Vector3 startPoint;
        private GameObject anchor;
        private GameObject line;
        private LineRenderer lineRenderer;
        private OVRCameraRig ovrCameraRig;
        private Transform rightController;

        void Awake()
        {
            _onTop = false;

            top = new Plane(Vector3.up, topAnchor.transform.position);
            startPoint = topAnchor.transform.position;

            anchor = Instantiate(pointPrefab, startPoint, Quaternion.identity);

            line = Instantiate(linePrefab, startPoint, Quaternion.identity);
            lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, startPoint);
            lineRenderer.SetPosition(2, startPoint);
            lineRenderer.SetPosition(3, startPoint);

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;
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
            }
            else
            {
                if (_onTop)
                {
                    _onTop = false;
                }
            }
            Vector3 position = castRay.GetPoint(castDistance);
            transform.position = position;
            anchor.transform.position = position;

            Vector3 lineX = lineRenderer.GetPosition(1);
            lineX.x = position.x;
            Vector3 lineY = lineRenderer.GetPosition(3);
            lineY.x = position.y;
            lineRenderer.SetPosition(1, line.transform.InverseTransformPoint(lineX));
            lineRenderer.SetPosition(2, line.transform.InverseTransformPoint(position));
            lineRenderer.SetPosition(3, line.transform.InverseTransformPoint(lineY));

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onTop)
                {
                    edges = new List<Vector3> { startPoint, lineX, position, lineY };
                    float bottom = RoomAnchor.Instance.transform.position.y;
                    for (int i = 4; i < edges.Count+4; i++)
                    {
                        edges[i] = new Vector3(edges[i].x, bottom, edges[i].z);
                    }
                    Destroy(anchor);
                    Destroy(line);
                    parent.ChildTriggered(3);
                }
            }
        }
    }
}
