using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class TeleportTarget : MonoBehaviour
{
    public Camera cam;
    public GameObject cube;

    public float blinkSpeed = .4f;

    public float maxSpeed = 1f;
    public float minSpeed = 0.5f;
    private float speed;

    public float minSize = 0.25f;
    public float maxSize = 0.5f;
    private float size;

    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;
    private float alpha;

    private float transition = 0f;

    public float offset = 1.15f;

    public float unblinkTime;

    private bool blinking;
    private bool focused;

    private Vector3 originalTransform;

    private static GameObject[] cubes;

    void Start()
    {
        // left over from when I was using setEnabled instead of disabling the renderer. Don't want to disable scripts!
        if (cubes == null)
        {
            cubes = GameObject.FindGameObjectsWithTag("MoveTarget");
        }
        originalTransform = transform.position;
        blinking = false;
        focused = false;
        size = minSize;
        alpha = minAlpha;
        speed = minSpeed;
    }

    public void Focus()
    { 
        focused = true;
    }

    public void Unfocus()
    {
        focused = false;
    }

    public void MoveMe()
    {
        // if already moving, ignore
        if (blinking) return;
        
        blinking = true;
        unblinkTime = Time.time + blinkSpeed + 0.05f;

        CameraFade.StartAlphaFade(Color.black, false, blinkSpeed, 0f, () =>
        {
            Vector3 newPos = transform.position;
            newPos.y += offset;
            cam.transform.position = newPos;

            MeshRenderer render;

            // mark all cubes as visited
            if (cubes != null) {
                
                foreach (GameObject obj in cubes)
                {
                    if (obj != null)
                    {
                        render = obj.GetComponent<MeshRenderer>();
                        render.enabled = true;
                    }
                }
            }

            // except this one...
            render = gameObject.GetComponentInChildren<MeshRenderer>();
            render.enabled = false;
        });
    }

    void Update()
    {
        if (blinking && Time.time > unblinkTime)
        {
            blinking = false;
            //CameraFade.StartAlphaFade(Color.black, true, blinkSpeed, 0f, null);
        }

        if (focused)
        {
            if (transition >= 1f)
            {
                transition = 1f;
            } else
            {
                transition += .01f;
            }
        } else
        {
            if (transition <= 0f)
            {
                transition = 0f;
            }
            else
            {
                transition -= .01f;
            }
        }

        size = Mathf.Lerp(minSize, maxSize, Mathf.SmoothStep(0.0f, 1.0f, transition));
        speed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.SmoothStep(0.0f, 1.0f, transition));
        alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.SmoothStep(0.0f, 1.0f, transition));
    }

    void FixedUpdate()
    {
        cube.transform.localScale = new Vector3(size, size, size);
        var offset = Mathf.Sin(Time.time * speed);
        transform.Rotate(Vector3.up, 1 * speed);
        Vector3 newTransform = originalTransform;
        newTransform.y += offset/12;
        transform.position = newTransform;
    }
}

