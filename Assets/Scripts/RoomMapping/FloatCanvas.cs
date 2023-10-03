using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatCanvas : MonoBehaviour
{
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 1.5f;
        transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
    }
}
