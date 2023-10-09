using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            CreateRoom.Walls = walls;
        }

        private void BuildHorizontalSurfaces()
        {
            GameObject ceiling = new GameObject("(Ceiling)");
            GameObject floor = new GameObject("(Floor)");
            ceiling.transform.SetParent(parent.transform);
            ceiling.transform.Rotate(Vector3.forward * 180);
            floor.transform.SetParent(parent.transform);

            Mesh ceilingMesh = new Mesh();
            Mesh floorMesh = new Mesh();

            //get all local points:
            List<Vector3> ceilingVerts3D = new List<Vector3>();
            List<Vector2> ceilingVerts2D = new List<Vector2>();
            List<Vector3> floorVerts3D = new List<Vector3>();
            List<Vector2> floorVerts2D = new List<Vector2>();
            for (int i = 0; i < parent.corners.Count - 1; i++)
            {
                //ceiling:
                Vector3 ceilingVert = ceiling.transform.InverseTransformPoint(parent.transform.TransformPoint(parent.corners[i]));
                ceilingVerts3D.Add(ceilingVert);
                ceilingVerts2D.Add(new Vector2(ceilingVert.x, ceilingVert.z));

                //floor:
                Vector3 floorVert = floor.transform.InverseTransformPoint(parent.transform.TransformPoint(parent.corners[i]));
                floorVerts3D.Add(floorVert);
                floorVerts2D.Add(new Vector2(floorVert.x, floorVert.z));
            }

            //set vers:
            ceilingMesh.vertices = ceilingVerts3D.ToArray();
            floorMesh.vertices = floorVerts3D.ToArray();

            //triangles:
            int[] ceilingTriangles = new Triangulator(ceilingVerts2D.ToArray()).Triangulate();
            int[] floorTriangles = new Triangulator(floorVerts2D.ToArray()).Triangulate();
            if (_windingDirection == 1)
            {
                ceilingTriangles.Reverse();
            }
            else
            {
                floorTriangles.Reverse();
            }
            ceilingMesh.triangles = ceilingTriangles;
            floorMesh.triangles = floorTriangles;

            //uvs:
            Vector2[] uvs = new Vector2[floorVerts2D.Count];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(floorVerts2D[i].x, floorVerts2D[i].y);
            }
            ceilingMesh.uv = uvs;
            floorMesh.uv = uvs;

            // rendering components:
            ceiling.AddComponent<MeshFilter>().mesh = ceilingMesh;
            floor.AddComponent<MeshFilter>().mesh = floorMesh;

            // materials:
            MeshRenderer ceilingRenderer = ceiling.AddComponent<MeshRenderer>();
            if (parent.ceilingMaterial)
            {
                ceilingRenderer.material = parent.ceilingMaterial;
            }
            else
            {
                ceilingRenderer.enabled = false;
            }

            MeshRenderer floorRenderer = floor.AddComponent<MeshRenderer>();
            if (parent.floorMaterial)
            {
                floorRenderer.material = parent.floorMaterial;
            }
            else
            {
                floorRenderer.enabled = false;
            }

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

            CreateRoom.Ceiling = ceiling;
            CreateRoom.Floor = floor;
        }
    }
}
