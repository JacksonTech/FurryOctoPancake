using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

    public GameObject left, right;
    public float yaw;

    public enum State { OPEN, CLOSED };
    private State state;

    public float moveTime = 0.25f;

    public float moveDist = 0.85f;
    public float moveAngle = 0f;


    private Vector3 startL, targetL, startR, targetR;

	// Use this for initialization
	void Start () {
        state = State.CLOSED;
        startL = left.transform.position;
        startR = right.transform.position;

        targetL = startL + (Quaternion.Euler(0, moveAngle, 0) * (new Vector3(moveDist, 0, 0)));
        targetR = startR + (Quaternion.Euler(0, moveAngle, 0) * (new Vector3(-moveDist, 0, 0)));

        Open();
    }
	
	// Update is called once per frame
	void Update () {

	}

    void Open()
    {
        if (state != State.CLOSED)
        {
            return;
        }
        StartCoroutine("OpenRoutine");
    }

    void Close()
    {
        if (state != State.OPEN)
        {
            return;
        }
        StartCoroutine("CloseRoutine");
    }

    // https://forum.unity3d.com/threads/open-door-script.19019/
    IEnumerator OpenRoutine()
    {
        yield return new WaitForSeconds(1);
        var endTime = Time.time + moveTime;

        while (Time.time < endTime)
        {
            left.transform.position = Vector3.Lerp(targetL, startL , Mathf.SmoothStep(0.0f, 1.0f, (endTime - Time.time) / moveTime));
            right.transform.position = Vector3.Lerp(targetR, startR, Mathf.SmoothStep(0.0f, 1.0f, (endTime - Time.time) / moveTime));
            yield return null;
        }

        left.transform.position = targetL;
        right.transform.position = targetR;
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
    }
}
