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
