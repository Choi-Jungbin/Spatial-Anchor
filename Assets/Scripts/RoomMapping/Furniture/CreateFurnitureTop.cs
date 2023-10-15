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
        [SerializeField] TextMeshPro text;

        public List<Vector3> edges;

        private bool _onTop;
        private bool _check;
        private Plane top;
        private Vector3 position;
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
            _check = false;

            top = new Plane(Vector3.up, topAnchor.transform.position);
            position = topAnchor.transform.position;
            edges = new List<Vector3> { position, position, position, position };

            anchor = Instantiate(pointPrefab, edges[0], Quaternion.identity);

            line = Instantiate(linePrefab, edges[0], Quaternion.identity);
            lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, edges[0]);

            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;

            ray = Instantiate(rayPrefab, transform.position, Quaternion.identity);
            rayRenderer = ray.GetComponent<LineRenderer>();
            rayRenderer.SetPosition(0, transform.position);
            rayRenderer.SetPosition(1, transform.position);
        }

        private void OnDisable()
        {
            Destroy(anchor);
            Destroy(line);
            Destroy(ray);
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

            }
            else
            {
                if (_onTop)
                {
                    _onTop = false;
                }
                anchor.SetActive(false);
            }

            if (_check)
            {
                Vector3 direction = (edges[1] - edges[0]).normalized;

                // '위' 방향과 direction 사이의 외적으로 '수직' 방향 계산
                Vector3 perpendicularDirection = Vector3.Cross(Vector3.up, direction).normalized;

                // position에서 edges[1]까지의 거리 계산
                float distance = Vector3.Distance(position, edges[1]);

                // edges[2] 업데이트: edges[1]에서 '수직' 방향으로 거리만큼 이동한 위치
                edges[2] = edges[1] + perpendicularDirection * distance;

                anchor.transform.position = edges[2];

                Vector3 vector1to2 = edges[1] - edges[0];
                Vector3 vector2to3 = edges[2] - edges[1];

                // 위의 두 벡터를 합함
                Vector3 diagonalVector = vector1to2 + vector2to3;
                edges[3] = edges[0] + diagonalVector;
            }
            else
            {
                anchor.transform.position = position;
            }
            transform.position = position;
            rayRenderer.SetPosition(0, castRay.origin);
            rayRenderer.SetPosition(1, transform.position);
            UpdateLine();
            text.text = edges[2].ToString() + ", " + edges[3].ToString();

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (_onTop)
                {
                    if (_check)
                    {
                        float bottom = RoomAnchor.Instance.transform.position.y;
                        for (int i = 0; i < 4; i++)
                        {
                            edges.Add(new Vector3(edges[i].x, bottom, edges[i].z));
                        }
                        
                        parent.ChildTriggered(3);
                    }
                    else
                    {
                        _check = true;
                        edges[2] = position;
                    }
                }
            }
        }

        private void UpdateLine()
        {
            if (_check)
            {
                lineRenderer.positionCount = 5;

                for (int i = 0; i < 4; i++)
                {
                    lineRenderer.SetPosition(i, edges[i]);
                }
                lineRenderer.SetPosition(4, edges[0]);
            }
            else
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(1, position);
            }
        }
    }
}
