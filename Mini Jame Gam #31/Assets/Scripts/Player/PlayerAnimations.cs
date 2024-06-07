using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header ("References")]
    private PlayerInput inputScript;
    private PlayerMovement movementScript;

    public enum State { Idle, Walk, Jump }
    private State playerState = State.Idle;

    private void OnEnable() {
        playerState = State.Idle;
    }

    private void Update() {
        /* TODO
        if(Mathf.Abs(movementScript.CurVelocity.y) > 0.1) {
            
        }
        if(Mathf.Abs(movementScript.CurVelocity.x) > 0.1) {
            
        }
        */
    }
}
