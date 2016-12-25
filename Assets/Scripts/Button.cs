using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    public DoorController door;

    public void clickMe()
    {
        door.Open();
    }
}

