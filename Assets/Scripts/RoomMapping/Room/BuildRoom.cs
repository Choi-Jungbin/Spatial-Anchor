using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class BuildRoom : MonoBehaviour
    {
        [SerializeField] CreateRoom parent;
        [SerializeField] LineRenderer line;

        private Vector3 _ceilingCenter;
        private float _windingDirection;

        void Awake()
        {
            line.transform.SetParent(parent.transform);
        }

        void OnEnable()
        {
            line.positionCount = 0;

            SetCeilingCenter();
            SetWindingDirection();
            BuildWalls();
            BuildHorizontalSurfaces();

            line.gameObject.SetActive(true);

            parent.ChildTriggered(4);
        }

        private void SetCeilingCenter()
        {
            // find bounds:
            Bounds bounds = new Bounds(RoomAnchor.Instance.transform.TransformPoint(parent.corners[0]), Vector3.zero);
            foreach (var corner in parent.corners)
            {
                bounds.Encapsulate(RoomAnchor.Instance.transform.TransformPoint(corner));
            }

            //sets:
            _ceilingCenter = bounds.center;
        }

        private void SetWindingDirection()
        {
            //discover winding direction:
            Vector3 centerToFirst = Vector3.Normalize(RoomAnchor.Instance.transform.TransformPoint(parent.corners[0]) - _ceilingCenter);
            Vector3 centerToLast = Vector3.Normalize(RoomAnchor.Instance.transform.TransformPoint(parent.corners[parent.corners.Count - 2]) - _ceilingCenter);
            float windingAngle = Vector3.SignedAngle(centerToLast, centerToFirst, Vector3.up);

            //1 = clockwise, -1 = counterclockwise
            _windingDirection = Mathf.Sign(windingAngle);
        }

        private void BuildWalls()
        {
            List<GameObject> walls = new List<GameObject>();

            for(int i = 0; i < parent.corners.Count-1; i++)
            {
                //create:
                GameObject wall = new GameObject("(Walls)");
                wall.transform.SetParent(parent.transform);

                //orientation discovery:
                Vector3 crossPointA = _windingDirection == 1 ? parent.transform.TransformPoint(parent.corners[i]) : parent.transform.TransformPoint(parent.corners[i]);
                Vector3 crossPointB = _windingDirection == 1 ? parent.transform.TransformPoint(parent.corners[i+1]) : parent.transform.TransformPoint(parent.corners[0]);
                Vector3 wallForward = Vector3.Cross(Vector3.Normalize(crossPointA - crossPointB), Vector3.up);

                //orient:
                Vector3 left = parent.transform.TransformPoint(parent.corners[i]) + Vector3.down * (parent.RoomHeight * .5f);
                Vector3 right = parent.transform.TransformPoint(parent.corners[i + 1]) + Vector3.down * (parent.RoomHeight * .5f);
                wall.transform.position = Vector3.Lerp(left, right, .5f);
                wall.transform.rotation = Quaternion.LookRotation(wallForward);
                wall.transform.localScale = new Vector3(Vector3.Distance(parent.corners[i], parent.corners[i + 1]), parent.RoomHeight, 1);

                //lists:
                List<Vector3> verts = new List<Vector3>();
                List<int> tris = new List<int>();

                //quad corners:
                Vector3 lowerLeft = parent.transform.TransformPoint(parent.corners[i]) + Vector3.down * parent.RoomHeight;
                Vector3 upperLeft = parent.transform.TransformPoint(parent.corners[i]);
                Vector3 upperRight = parent.transform.TransformPoint(parent.corners[i + 1]);
                Vector3 lowerRight = parent.transform.TransformPoint(parent.corners[i + 1]) + Vector3.down * parent.RoomHeight;

                //set vertices (in local space of wall):
                verts.Add(wall.transform.InverseTransformPoint(lowerLeft));
                verts.Add(wall.transform.InverseTransformPoint(upperLeft));
                verts.Add(wall.transform.InverseTransformPoint(upperRight));
                verts.Add(wall.transform.InverseTransformPoint(lowerRight));

                //build/extend wireframe:
                if (i == 0)
                {
                    line.positionCount += 5;
                    line.SetPosition(0, line.transform.InverseTransformPoint(lowerLeft));
                    line.SetPosition(1, line.transform.InverseTransformPoint(upperLeft));
                    line.SetPosition(2, line.transform.InverseTransformPoint(upperRight));
                    line.SetPosition(3, line.transform.InverseTransformPoint(lowerRight));
                    line.SetPosition(4, line.transform.InverseTransformPoint(lowerLeft));
                }
                else
                {
                    line.positionCount += 4;
                    line.SetPosition(line.positionCount - 4, line.transform.InverseTransformPoint(upperLeft));
                    line.SetPosition(line.positionCount - 3, line.transform.InverseTransformPoint(upperRight));
                    line.SetPosition(line.positionCount - 2, line.transform.InverseTransformPoint(lowerRight));
                    line.SetPosition(line.positionCount - 1, line.transform.InverseTransformPoint(lowerLeft));
                }

                if (i == parent.corners.Count - 2)
                {
                    line.positionCount += 1;
                    line.SetPosition(line.positionCount - 1, line.transform.InverseTransformPoint(upperRight));
                }

                //set triangles:
                tris.Add(verts.Count - 4);
                tris.Add(verts.Count - 3);
                tris.Add(verts.Count - 2);
                tris.Add(verts.Count - 2);
                tris.Add(verts.Count - 1);
                tris.Add(verts.Count - 4);

                if (_windingDirection == -1)
                {
                    tris.Reverse();
                }

                //populate mesh:
                Mesh mesh = new Mesh();
                mesh.vertices = verts.ToArray();
                mesh.triangles = tris.ToArray();
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();

                //rendering:
                wall.AddComponent<MeshFilter>().mesh = mesh;

                //collider:
                wall.AddComponent<BoxCollider>().size = new Vector3(1, 1, .01f);

                walls.Add(wall);
            }
            parent.Walls = walls;
        }

        private void BuildHorizontalSurfaces()
        {
            GameObject ceiling = new GameObject("(Ceiling)");
            GameObject floor = new GameObject("(Floor)");
            ceiling.transform.SetParent(parent.transform);
            floor.transform.SetParent(parent.transform);

            Mesh ceilingMesh = new Mesh();
            Mesh floorMesh = new Mesh();

            List<Vector3> ceilingVerts3D = new List<Vector3>();
            List<Vector3> floorVerts3D = new List<Vector3>();
            foreach (Vector3 corner in parent.corners)
            {
                // Assuming your corners are in local space, transform them to world space
                Vector3 cornerWorldPosition = parent.transform.TransformPoint(corner);
                // Set the vertices in world space
                ceilingVerts3D.Add(cornerWorldPosition);

                cornerWorldPosition.y = RoomAnchor.Instance.transform.position.y;
                floorVerts3D.Add(cornerWorldPosition);
            }

            //set vers:
            ceilingMesh.vertices = ceilingVerts3D.ToArray();
            floorMesh.vertices = floorVerts3D.ToArray();

            // rendering components:
            ceiling.AddComponent<MeshFilter>().mesh = ceilingMesh;
            floor.AddComponent<MeshFilter>().mesh = floorMesh;

            //calculate:
            ceilingMesh.RecalculateNormals();
            ceilingMesh.RecalculateBounds();
            floorMesh.RecalculateNormals();
            floorMesh.RecalculateBounds();

            //colliders (no need for mesh colliders - simplify with boxes):
            ceiling.AddComponent<BoxCollider>();
            floor.AddComponent<BoxCollider>();

            //push floor down:
            floor.transform.Translate(Vector3.down * parent.RoomHeight);

            parent.Ceiling = ceiling;
            parent.Floor = floor;
        }
    }
}
