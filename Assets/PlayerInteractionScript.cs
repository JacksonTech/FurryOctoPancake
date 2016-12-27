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

public class PlayerInteractionScript : MonoBehaviour {

    public Camera cam;
    private Transform head;
    private Vector3 blenderOffset;
    private bool nearPlayer;
    private bool justArrived;
    private Quaternion lastRot;
    private Quaternion turnMyHead;

    // Use this for initialization
    void Start() {
        head = GetComponent<Transform>().Find("Armature/main/torso/neck/head");
        turnMyHead = Quaternion.Euler(-90, 180, -90);
        nearPlayer = false;
    }

    void LateUpdate()
    {
        if (nearPlayer)
        {
            Quaternion newDir = Quaternion.LookRotation(cam.transform.position - head.transform.position);
            newDir = newDir * turnMyHead;
            head.transform.rotation = Quaternion.Slerp(lastRot, newDir, 2.5f*Time.deltaTime);
            lastRot = head.transform.rotation;
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
        }

        if (nearPlayer && !justArrived)
        {
            justArrived = true;
            lastRot = head.transform.rotation;
        }
    }
}
