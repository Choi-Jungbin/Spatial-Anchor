using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor
{
    public class CreateRoom : MonoBehaviour
    {
        [SerializeField] CreateFurniture createFurniture;

        public List<Vector3> corners;
        public List<GameObject> childObj;

        public static GameObject Floor { get; set; }
        public static GameObject Ceiling { get; set; }
        public static List<GameObject> Walls { get; set; }

        public Material ceilingMaterial;
        public Material floorMaterial;

        public float RoomHeight
        {
            get
            {
                return corners[0].y;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            corners = new List<Vector3>();

            childObj = new List<GameObject>();
            foreach (Transform child in transform)
            {
                GameObject c = child.gameObject;
                c.SetActive(false);
                childObj.Add(c);
            }
            childObj[0].SetActive(true);
        }

        public void DestroyRoom()
        {
            //sets:
            corners = new List<Vector3>();

            //destroy:
            Destroy(Ceiling);
            Destroy(Floor);
            foreach (var wall in Walls)
            {
                Destroy(wall);
            }

            Walls = new List<GameObject>();
        }

        public void ChildTriggered(int child, bool redo = false)
        {
            childObj[child].SetActive(false);
            if (redo)
            {
                DestroyRoom();
                childObj[0].SetActive(true);
            }
            else
            {
                childObj[child + 1].SetActive(true);
            }
        }
    }
}
