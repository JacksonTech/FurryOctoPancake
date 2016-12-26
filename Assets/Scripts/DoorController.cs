/*
Copyright 2016 Cody Jackson

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

    // left and right door objects
    public GameObject left, right;

    public enum State { OPEN, OPENING, CLOSED, CLOSING };
    private State state;

    // speed
    public float moveTime = 0.25f;
    public float moveDist = 0.85f;
    public float moveAngle = 0f;

    public bool toggle = true;

    // only applies if toggle is false.
    // door won't close if you're looking at it!
    public float waitTime = 2f;

    private float timeToClose = 0;

    public GvrAudioSource sound;

    private Vector3 startL, targetL, startR, targetR;

    // used to determine if we're looking at this door
    public Camera cam;

	void Start () {
        state = State.CLOSED;
        startL = left.transform.position;
        startR = right.transform.position;

        // setup target locations
        targetL = startL + (Quaternion.Euler(0, moveAngle, 0) * (new Vector3(moveDist, 0, 0)));
        targetR = startR + (Quaternion.Euler(0, moveAngle, 0) * (new Vector3(-moveDist, 0, 0)));
    }
	
    void FixedUpdate()
    {
        // open and time to close?
        if (state == State.OPEN && !toggle && Time.time > timeToClose)
        {

            // does the camera exist?
            if (cam != null)
            {
                // where is the door center on the screen?
                Vector3 pos = cam.WorldToViewportPoint(transform.position);
                Debug.Log(pos);
                // only close if not looking at door or we're further away than 5 units
                if (!(pos.x >= -1 && pos.x <= 1 && pos.y >= -1 && pos.y <= 1 && pos.z <= 5))
                {
                    Close();
                }
            } else { 
                Close();
            }
        }
    }

    public void Open()
    {
        if (state != State.CLOSED)
        {
            return;
        }
        sound.Play();
        state = State.OPENING;
        StartCoroutine("OpenRoutine");
    }

    public void Close()
    {
        if (state != State.OPEN)
        {
            return;
        }
        sound.Play();
        state = State.CLOSING;
        StartCoroutine("CloseRoutine");
    }

    // inspired by https://forum.unity3d.com/threads/open-door-script.19019/
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
