using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class TeleportTarget : MonoBehaviour
{
    public Camera cam;
    public GameObject cube;

    public float blinkSpeed = .4f;
    public float speed = 1f;
    private float curSpeed = 1f;

    public float offset = 1.15f;

    private Vector3 originalTransform;

    private static GameObject[] cubes;

    void Start()
    {
        if (cubes == null)
        {
            cubes = GameObject.FindGameObjectsWithTag("MoveTarget");
        }
        originalTransform = transform.position;
    }

    public void MoveMe()
    {
        CameraFade.StartAlphaFade(Color.black, false, blinkSpeed, 0f, () =>
        {
            Vector3 newPos = transform.position;
            newPos.y += offset;
            cam.transform.position = newPos;

            // mark all cubes as visited
            if (cubes != null) { 
                foreach (GameObject obj in cubes)
                {
                    obj.SetActive(true);
                }
            }

            // except this one...
            cube.SetActive(false);
            CameraFade.StartAlphaFade(Color.black, true, blinkSpeed, 0f, null);
        });
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

