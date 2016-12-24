using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

    public GameObject left, right;
    public float yaw;

    public enum State { OPEN, OPENING, CLOSED, CLOSING };
    private State state;

    public float moveTime = 0.25f;

    public float moveDist = 0.85f;
    public float moveAngle = 0f;

    public bool toggle = true;
    public float waitTime = 2f;

    private float timeToClose = 0;

    public GvrAudioSource sound;

    private Vector3 startL, targetL, startR, targetR;

	// Use this for initialization
	void Start () {
        state = State.CLOSED;
        startL = left.transform.position;
        startR = right.transform.position;

        targetL = startL + (Quaternion.Euler(0, moveAngle, 0) * (new Vector3(moveDist, 0, 0)));
        targetR = startR + (Quaternion.Euler(0, moveAngle, 0) * (new Vector3(-moveDist, 0, 0)));
    }
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate()
    {
        if (state == State.OPEN && !toggle && Time.time > timeToClose)
        {
            Close();
        }
    }

    public void Open()
    {
        sound.Play();
        if (state != State.CLOSED)
        {
            return;
        }
        state = State.OPENING;
        StartCoroutine("OpenRoutine");
    }

    public void Close()
    {
        sound.Play();
        if (state != State.OPEN)
        {
            return;
        }
        state = State.CLOSING;
        StartCoroutine("CloseRoutine");
    }

    // https://forum.unity3d.com/threads/open-door-script.19019/
    IEnumerator OpenRoutine()
    {
        var endTime = Time.time + moveTime;

        while (Time.time < endTime)
        {
            left.transform.position = Vector3.Lerp(targetL, startL , Mathf.SmoothStep(0.0f, 1.0f, (endTime - Time.time) / moveTime));
            right.transform.position = Vector3.Lerp(targetR, startR, Mathf.SmoothStep(0.0f, 1.0f, (endTime - Time.time) / moveTime));
            yield return null;
        }

        left.transform.position = targetL;
        right.transform.position = targetR;
        state = State.OPEN;
        timeToClose = Time.time + waitTime;
    }

    IEnumerator CloseRoutine()
    {
        var endTime = Time.time + moveTime;

        while (Time.time < endTime)
        {
            left.transform.position = Vector3.Lerp(startL, targetL, Mathf.SmoothStep(0.0f, 1.0f, (endTime - Time.time) / moveTime));
            right.transform.position = Vector3.Lerp(startR, targetR, Mathf.SmoothStep(0.0f, 1.0f, (endTime - Time.time) / moveTime));
            yield return null;
        }

        left.transform.position = startL;
        right.transform.position = startR;
        state = State.CLOSED;
    }
}
