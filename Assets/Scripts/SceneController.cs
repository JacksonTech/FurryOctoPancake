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
using System.Collections.Generic;

public class SceneController : MonoBehaviour {

    private Dictionary<string, bool> locked;

    public string[] lockList;

    void Start()
    {
        locked = new Dictionary<string, bool>();

        // manually lock some things by default
        foreach (string s in lockList)
        {
            lockObject(s);
        }
    }

    // explicitly set
    public void setStatus(string item, bool status)
    {
        locked[item] = status;
    }

    // flip flop
    public void toggleLocked(string item)
    {
        if (!locked.ContainsKey(item))
        {
            locked[item] = false;
        } else
        {
            locked[item] = !locked[item];
        }
    }

    // absolute set
    public void lockObject(string item)
    {
        locked[item] = true;
    }

    // absolute set
    public void unlockObject(string item)
    {
        locked[item] = false;
    }

    // unlocked by default
    public bool isLocked(string item)
    {
        if (!locked.ContainsKey(item)) return false;
        return locked[item];
    }

}
