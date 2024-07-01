using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RestartEvent : MonoBehaviour
{

    //global reference to this script
    public static RestartEvent current;
    private void Awake() {
        current = this; 
    }

    public event Action onRestartEvent;

    public void TriggerRestartEvent() {
        if(onRestartEvent != null)
            onRestartEvent();
    }

}
