using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SpatialAnchor
{
    public class ClickButton : MonoBehaviour
    {
        [SerializeField] GameObject rayPrefab;

        protected OVRCameraRig ovrCameraRig;
        protected Transform rightController;
        protected GameObject ray;
        protected LineRenderer rayRenderer;

        protected void OnEnable()
        {
            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            rightController = ovrCameraRig.rightControllerAnchor;

            ray = Instantiate(rayPrefab, transform.position, Quaternion.identity);
            rayRenderer = ray.GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        protected void Update()
        {
            Ray castRay = new Ray(rightController.position, rightController.forward);

            // GraphicRaycaster �������� (Canvas ���� ������Ʈ�� �߰��Ǿ� �־�� ��)
            GraphicRaycaster graphicRaycaster = FindObjectOfType<GraphicRaycaster>();

            // PointerEventData ���� (EventSystem ���� ������Ʈ�� �߰��Ǿ� �־�� ��)
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

            // ScreenPoint�� �����ϱ� ���� ī�޶� ��ǥ ��ȯ
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(castRay.origin + castRay.direction * 1000f);

            pointerEventData.position = screenPoint;

            List<RaycastResult> results = new List<RaycastResult>();

            // Graphic Raycast ����
            graphicRaycaster.Raycast(pointerEventData, results);
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        Button button = result.gameObject.GetComponent<Button>();
                        if (button != null)
                        {
                            Destroy(ray);
                            button.onClick.Invoke();
                        }
                    }
                }
            }

            Vector3 endPoint = castRay.origin + rightController.forward * 4f;
            if (results.Count > 0)
            {
                endPoint = results[0].worldPosition;
            }

            // Set the start and end points of the line renderer to the start and end points of the casted Ray.
            rayRenderer.SetPosition(0, castRay.origin);
            rayRenderer.SetPosition(1, endPoint);
        }
    }
}
