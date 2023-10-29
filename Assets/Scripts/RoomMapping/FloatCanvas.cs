using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatCanvas : MonoBehaviour
{
    [SerializeField] float distance = 2f;
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * distance;
        transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
    }
}
