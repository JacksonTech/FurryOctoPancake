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

public class RobotScript : MonoBehaviour {

    public Camera cam;

    public SceneController sceneController;

    public Canvas chatBubble;

    private Transform head;
    private Vector3 blenderOffset;
    private bool nearPlayer;
    private bool justArrived;
    private bool clickedMe;

    private Quaternion lastRot;
    private Quaternion turnMyHead;

    private Vector3 closedScale = new Vector3(0, 0, 0);
    private Vector3 openScale = new Vector3(0.001f, 0.001f, 0.001f);
    private Vector3 lastScale;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 lastPosition;

    // Use this for initialization
    void Start() {
        head = GetComponent<Transform>().Find("Armature/main/torso/neck/head");
        turnMyHead = Quaternion.Euler(-90, 180, -90);
        nearPlayer = false;

        openPosition = chatBubble.transform.position;
        closedPosition = openPosition - cam.transform.right * 3f;

    }

    void LateUpdate()
    {
        if (nearPlayer)
        {
            Quaternion newDir = Quaternion.LookRotation(cam.transform.position - head.transform.position);
            newDir = newDir * turnMyHead;
            head.transform.rotation = Quaternion.Slerp(lastRot, newDir, 6f*Time.deltaTime);
            lastRot = head.transform.rotation;

            if (clickedMe)
            {
                // make chat bubble appear!
                chatBubble.transform.localScale = Vector3.Slerp(lastScale, openScale, 6f * Time.deltaTime);
                lastScale = chatBubble.transform.localScale;

                chatBubble.transform.position = Vector3.Slerp(lastPosition, openPosition, 6f * Time.deltaTime);
                lastPosition = chatBubble.transform.position;
            }
        } else
        {
            // make chat bubble disappear!
            chatBubble.transform.localScale = Vector3.Slerp(lastScale, closedScale, 6f * Time.deltaTime);
            lastScale = chatBubble.transform.localScale;

            chatBubble.transform.position = Vector3.Slerp(lastPosition, closedPosition, 6f * Time.deltaTime);
            lastPosition = chatBubble.transform.position;
        }
    }

    // click the robot, win a prize!
    public void ClickMe()
    {
        // if near enough
        // eventually limit the "reach" of the ray somehow?
        if (nearPlayer) {
            clickedMe = true;
            sceneController.unlockObject("apartmentDoor");
        }
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, cam.transform.position)) < 5)
        {
            nearPlayer = true;
        } else
        {
            nearPlayer = false;
            justArrived = false;
            clickedMe = false;
           
        }

        if (nearPlayer && !justArrived)
        {
            justArrived = true;
            lastRot = head.transform.rotation;
            lastScale = closedScale;
            lastPosition = closedPosition;
        }
    }
}
