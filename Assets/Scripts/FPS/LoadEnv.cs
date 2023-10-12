using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpatialAnchor
{
    public class LoadEnv : MonoBehaviour
    {
        [SerializeField] Material ceilingMaterial;
        [SerializeField] Material floorMaterial;

        private List<Vector3> CeilingCorners;
        private Vector3 _ceilingCenter;
        private float _windingDirection;

        public GameObject Floor;
        public GameObject Ceiling;
        public List<GameObject> Walls;

        public float RoomHeight
        {
            get
            {
                return CeilingCorners[0].y;
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            LoadPrevious();
            SetCeilingCenter();
            SetWindingDirection();
            BuildWalls();
            BuildHorizontalSurfaces();
        }

        public void LoadPrevious()
        {
            // PlayerPrefs에서 저장된 방의 정보 불러오기
            string input = PlayerPrefs.GetString("RoomMapper", "");
            string[] inputs = input.Split('|');
            CeilingCorners = new List<Vector3>();
            foreach (var item in inputs)
            {
                string[] corners = item.Split(',');
                CeilingCorners.Add(new Vector3(float.Parse(corners[0]), float.Parse(corners[1]), float.Parse(corners[2])));
            }
        }

        private void SetCeilingCenter()
        {
            // find bounds:
            Bounds bounds = new Bounds(RoomAnchor.Instance.transform.TransformPoint(CeilingCorners[0]), Vector3.zero);
            foreach (var corner in CeilingCorners)
            {
                bounds.Encapsulate(RoomAnchor.Instance.transform.TransformPoint(corner));
            }

            //sets:
            _ceilingCenter = bounds.center;
        }

        private void SetWindingDirection()
        {
            //discover winding direction:
            Vector3 centerToFirst = Vector3.Normalize(RoomAnchor.Instance.transform.TransformPoint(CeilingCorners[0]) - _ceilingCenter);
            Vector3 centerToLast = Vector3.Normalize(RoomAnchor.Instance.transform.TransformPoint(CeilingCorners[CeilingCorners.Count - 2]) - _ceilingCenter);
            float windingAngle = Vector3.SignedAngle(centerToLast, centerToFirst, Vector3.up);

            //1 = clockwise, -1 = counterclockwise
            _windingDirection = Mathf.Sign(windingAngle);
        }

        private void BuildWalls()
        {
            Walls = new List<GameObject>();

            for (int i = 0; i < CeilingCorners.Count - 1; i++)
            {
                //create:
                GameObject wall = new GameObject("(Walls)");
                wall.transform.SetParent(transform);

                //orientation discovery:
                Vector3 crossPointA = _windingDirection == 1 ? transform.TransformPoint(CeilingCorners[i]) : transform.TransformPoint(CeilingCorners[i]);
                Vector3 crossPointB = _windingDirection == 1 ? transform.TransformPoint(CeilingCorners[i + 1]) : transform.TransformPoint(CeilingCorners[0]);
                Vector3 wallForward = Vector3.Cross(Vector3.Normalize(crossPointA - crossPointB), Vector3.up);

                //orient:
                Vector3 left = transform.TransformPoint(CeilingCorners[i]) + Vector3.down * (RoomHeight * .5f);
                Vector3 right = transform.TransformPoint(CeilingCorners[i + 1]) + Vector3.down * (RoomHeight * .5f);
                wall.transform.position = Vector3.Lerp(left, right, .5f);
                wall.transform.rotation = Quaternion.LookRotation(wallForward);
                wall.transform.localScale = new Vector3(Vector3.Distance(CeilingCorners[i], CeilingCorners[i + 1]), RoomHeight, 1);

                //lists:
                List<Vector3> verts = new List<Vector3>();
                List<int> tris = new List<int>();

                //quad corners:
                Vector3 lowerLeft = transform.TransformPoint(CeilingCorners[i]) + Vector3.down * RoomHeight;
                Vector3 upperLeft = transform.TransformPoint(CeilingCorners[i]);
                Vector3 upperRight = transform.TransformPoint(CeilingCorners[i + 1]);
                Vector3 lowerRight = transform.TransformPoint(CeilingCorners[i + 1]) + Vector3.down * RoomHeight;

                //set vertices (in local space of wall):
                verts.Add(wall.transform.InverseTransformPoint(lowerLeft));
                verts.Add(wall.transform.InverseTransformPoint(upperLeft));
                verts.Add(wall.transform.InverseTransformPoint(upperRight));
                verts.Add(wall.transform.InverseTransformPoint(lowerRight));

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

                Walls.Add(wall);
            }
        }

        private void BuildHorizontalSurfaces()
        {
            Ceiling = new GameObject("(Ceiling)");
            Floor = new GameObject("(Floor)");
            Ceiling.transform.SetParent(transform);
            Ceiling.transform.Rotate(Vector3.forward * 180);
            Floor.transform.SetParent(transform);

            Mesh ceilingMesh = new Mesh();
            Mesh floorMesh = new Mesh();

            //get all local points:
            List<Vector3> ceilingVerts3D = new List<Vector3>();
            List<Vector2> ceilingVerts2D = new List<Vector2>();
            List<Vector3> floorVerts3D = new List<Vector3>();
            List<Vector2> floorVerts2D = new List<Vector2>();
            for (int i = 0; i < CeilingCorners.Count - 1; i++)
            {
                //ceiling:
                Vector3 ceilingVert = Ceiling.transform.InverseTransformPoint(transform.TransformPoint(CeilingCorners[i]));
                ceilingVerts3D.Add(ceilingVert);
                ceilingVerts2D.Add(new Vector2(ceilingVert.x, ceilingVert.z));

                //floor:
                Vector3 floorVert = Floor.transform.InverseTransformPoint(transform.TransformPoint(CeilingCorners[i]));
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
            Ceiling.AddComponent<MeshFilter>().mesh = ceilingMesh;
            Floor.AddComponent<MeshFilter>().mesh = floorMesh;

            // materials:
            MeshRenderer ceilingRenderer = Ceiling.AddComponent<MeshRenderer>();
            if (ceilingMaterial)
            {
                ceilingRenderer.material = ceilingMaterial;
            }
            else
            {
                ceilingRenderer.enabled = false;
            }

            MeshRenderer floorRenderer = Floor.AddComponent<MeshRenderer>();
            if (floorMaterial)
            {
                floorRenderer.material = floorMaterial;
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
            Ceiling.AddComponent<BoxCollider>();
            Floor.AddComponent<BoxCollider>();

            //push floor down:
            Floor.transform.Translate(Vector3.down * RoomHeight);
        }
    }
}
