using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{

    public DoorController door;
    
    public GameObject cube;

    public float speed = 1f;
    private float curSpeed = 1f;
    private Vector3 originalTransform;

    void Start()
    {
        originalTransform = cube.transform.position;
    }

    public void clickMe()
    {
        door.Open();
    }

    void FixedUpdate()
    {
        var offset = Mathf.Sin(Time.time * curSpeed);
        transform.Rotate(Vector3.up, 1 * curSpeed);
        Vector3 newTransform = originalTransform;
        newTransform.y += offset/12;
        transform.position = newTransform;
    }
}

