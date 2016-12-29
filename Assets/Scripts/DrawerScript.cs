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

public class DrawerScript : MonoBehaviour {

    public Camera cam;

    private Vector3 curPos, closedPos, openPos;

    public SceneController sceneController;

    private bool opening;
	// Use this for initialization
	void Start () {
        openPos = transform.position;
        closedPos = openPos + new Vector3(0, 0, .166f);
        curPos = closedPos;
        transform.position = curPos;
	}   

    // Update is called once per frame
    void Update() {
        
        // did we step away?
        if (Vector3.Distance(closedPos, cam.transform.position) > 2)
        {
            opening = false;
        }
            
        if (opening)
        {
            transform.position = Vector3.Slerp(curPos, openPos, 5*Time.deltaTime);
        } else
        {
            transform.position = Vector3.Slerp(curPos, closedPos, 5*Time.deltaTime);
        }
        curPos = transform.position;
	}

    public void clickMe()
    {
        sceneController.unlockObject("computer");
        opening = true;
    }
}
