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

public class ElizabethComp : MonoBehaviour {

    public GameObject monitor;
    public SceneController sceneController;

    public Material loginScreen;
    public Material errorScreen;
    public Material emailScreen;

    private Material[] mats;
    private float changeMe;

    private bool dontChangeBaby;

    public GameObject exitObject;
	
	// Update is called once per frame
	void Update () {
	    if (Time.time > changeMe && !dontChangeBaby)
        {
            mats = monitor.GetComponent<Renderer>().materials;
            mats[1] = loginScreen;
            monitor.GetComponent<Renderer>().materials = mats;
        }
	}

    public void clickMe()
    {
        GetComponent<GvrAudioSource>().Play();
        if (sceneController.isLocked("computer"))
        {
            mats = monitor.GetComponent<Renderer>().materials;
            mats[1] = errorScreen;
            monitor.GetComponent<Renderer>().materials = mats;
            changeMe = Time.time + 2;
        } else
        {
            mats = monitor.GetComponent<Renderer>().materials;
            mats[1] = emailScreen;
            monitor.GetComponent<Renderer>().materials = mats;
            dontChangeBaby = true;
            exitObject.GetComponent<Collider>().enabled = true;
            exitObject.GetComponentInChildren<Renderer>().enabled = true;
            exitObject.GetComponentInChildren<ParticleSystem>().Play();
            sceneController.unlockObject("elevator");
        }
    }
}
