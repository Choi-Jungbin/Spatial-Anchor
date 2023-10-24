using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPosition : MonoBehaviour
{
    private GameObject position;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(gameObject.name + ": " + transform.position + ", " + transform.rotation);
        position = GameObject.CreatePrimitive(PrimitiveType.Cube);
        position.transform.position = transform.position;
        position.transform.rotation = transform.rotation;
        position.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.name + ": " + transform.position + ", " + transform.rotation);
        position.transform.position = transform.position;
        position.transform.rotation = transform.rotation;
    }
}
