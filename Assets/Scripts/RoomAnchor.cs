using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialAnchor
{
    public class RoomAnchor : MonoBehaviour
    {
        public static RoomAnchor Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<RoomAnchor>();
                }

                if (!_instance)
                {
                    _instance = new GameObject("(RoomAnchor)").AddComponent<RoomAnchor>();
                }

                return _instance;
            }
        }

        public Vector3[] Points
        {
            get;
            private set;
        }

        private static RoomAnchor _instance;

        
        void Awake()
        {
            StartCoroutine(SetupPlayArea());
        }

        private IEnumerator SetupPlayArea()
        {
            //find boundary:
            Points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

            //set anchor:
            transform.position = Vector3.Lerp(Points[0], Points[2], .5f);
            Vector3 roomForward = Vector3.Normalize(Points[1] - Points[0]);
            transform.rotation = Quaternion.LookRotation(roomForward);

            yield return null;
        }
    }
}
