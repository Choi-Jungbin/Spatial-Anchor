using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor
{
    public class CreateRoom : MonoBehaviour
    {
        public List<Vector3> corners;
        public List<GameObject> childObj;

        public GameObject Floor { get; set; }
        public GameObject Ceiling { get; set; }
        public List<GameObject> Walls { get; set; }

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

        public void ChildTriggered(int child)
        {
            childObj[child].SetActive(false);
            if (child < childObj.Count)
            {
                childObj[child + 1].SetActive(true);
            }
            else
            {
                CreateFurniture createFurniture = FindFirstObjectByType<CreateFurniture>();
                createFurniture.gameObject.SetActive(true);
            }
        }
    }
}
