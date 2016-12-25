using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class TeleportTarget : MonoBehaviour
{
    public Camera cam;
    public GameObject cube;

    public float blinkSpeed = .3f;

    public float maxSpeed = 4f;
    public float minSpeed = 1f;
    private float speed;

    public float minSize = 0.35f;
    public float maxSize = 0.46f;
    private float size;

    public float minAlpha = 0.3f;
    public float maxAlpha = 0.7f;
    private float alpha;

    public float minEmit = 0.0f;
    public float maxEmit = 0.8f;
    private float emit;

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
        SimpleFader.BeginFadeOut();

    }

    void Update()
    {
        if (blinking && Time.time >= unblinkTime)
        {
            Vector3 newPos = transform.position;
            newPos.y += offset;
            cam.transform.position = newPos;

            MeshRenderer render;

            // mark all cubes as visited
            if (cubes != null)
            {

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

            blinking = false;

            SimpleFader.BeginFadeIn();
        }

        if (focused)
        {    
            transition += .03f;
        } else
        {
            transition -= .03f;
        }

        transition = Mathf.Clamp01(transition);

        size = Mathf.Lerp(minSize, maxSize, Mathf.SmoothStep(0.0f, 1.0f, transition));
        speed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.SmoothStep(0.0f, 1.0f, transition));
        alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.SmoothStep(0.0f, 1.0f, transition));
        emit = Mathf.Lerp(minEmit, maxEmit, Mathf.SmoothStep(0.0f, 1.0f, transition));

    }

    void FixedUpdate()
    {
        cube.transform.localScale = new Vector3(size, size, size);
        var offset = Mathf.Sin(Time.time);
        transform.Rotate(Vector3.up, 1 * speed);
        Vector3 newTransform = originalTransform;
        newTransform.y += offset/12;
        transform.position = newTransform;

        Renderer r = cube.GetComponent<Renderer>();

        if (r != null)
        {
            Color c = r.material.color;
            c.a = alpha;
            r.material.color = c;

            // emit code based on http://answers.unity3d.com/questions/914923/standard-shader-emission-control-via-script.html
            Color emitC = c * Mathf.LinearToGammaSpace(emit);
            r.material.SetColor("_EmissionColor", emitC);
        }
    }
}

