using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpatialAnchor
{
    public class CreateFurniture : MonoBehaviour
    {
        public List<GameObject> childObj;
        public List<List<Vector3>> Furniture;

        void OnEnable()
        {
            Furniture = new List<List<Vector3>>();
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
                DontDestroyOnLoad(FindAnyObjectByType<LoadMap>());
                SceneManager.LoadScene("FPSGame");
            }
            else
            {
                if (child < childObj.Count-1)
                {
                    childObj[child + 1].SetActive(true);
                }
                else if (child == childObj.Count - 1)
                {
                    childObj[0].SetActive(true);
                }
            }
        }
    }
}
