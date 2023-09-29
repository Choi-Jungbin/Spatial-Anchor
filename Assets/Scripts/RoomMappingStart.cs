using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialAnchor
{
    public class RoomMappingStart : MonoBehaviour
    {
        [SerializeField] CreateRoom parent;

        private Camera _mainCamera;

        void Awake()
        {
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 3f;
            transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
        }

        public void Next()
        {
            parent.ChildTriggered(0);
        }
    }
}
