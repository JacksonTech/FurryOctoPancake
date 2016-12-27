using UnityEngine;
using System.Collections;

public class PlayerInteractionScript : MonoBehaviour {

    public Camera cam;
    private Transform head;
    private Vector3 blenderOffset;

    // Use this for initialization
    void Start () {
        head = GetComponent<Transform>().Find("Armature/main/torso/neck/head");
        blenderOffset = new Vector3(-90, 180, -90);
    }
	
    void LateUpdate()
    {
        head.LookAt(cam.transform);
        head.Rotate(blenderOffset);  
    }
}
