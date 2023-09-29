using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpatialAnchor
{
    public class CreateFurniture : MonoBehaviour
    {
        public List<GameObject> childObj;
        public List<GameObject> Furniture { get; set; }

        void Awake()
        {
            childObj = new List<GameObject>();
            foreach (Transform child in transform)
            {
                GameObject c = child.gameObject;
                c.SetActive(false);
                childObj.Add(c);
            }

            childObj[0].SetActive(true);
        }

        public void ChildTriggered(int child, bool redo = false)
        {
            childObj[child].SetActive(false);

            if (redo)
            {
                SceneManager.LoadScene("FPSGame");
            }
            else
            {
                if (child < childObj.Count)
                {
                    childObj[child + 1].SetActive(true);
                }
                else
                {
                    childObj[0].SetActive(true);
                }
            }
        }
    }
}
