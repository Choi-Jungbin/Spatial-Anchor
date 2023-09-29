using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialAnchor
{
    public class AcceptFurniture : MonoBehaviour
    {
        [SerializeField] CreateFurniture parent;
        [SerializeField] BuildFurniture build;
        [SerializeField] Button yes;
        [SerializeField] Button no;

        private Camera _mainCamera;

        void Awake()
        {
            yes.onClick.AddListener(Accept);
            no.onClick.AddListener(Reject);

            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 2f;
            transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
        }

        public void Accept()
        {
            parent.Furniture.Add(build.furniture);
            parent.ChildTriggered(5);
        }

        public void Reject()
        {
            parent.ChildTriggered(5);
        }
    }
}
