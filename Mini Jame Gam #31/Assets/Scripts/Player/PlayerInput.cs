using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private InputMap inputMap;

    // vector inputs
    public float MoveInput { get; private set; }

    // button down inputs
    public bool JumpInput { get; private set; }
    public bool TransformInput { get; private set; }
    public bool InteractInput { get; private set; }


    private void Awake() {
        // create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.Player.Enable();
    }

    private void Update() {

        // gets vector2 input from input map
        MoveInput = inputMap.Player.Move.ReadValue<float>();

        // gets button down inputs (true for 1 frame)
        JumpInput = inputMap.Player.Jump.triggered;
        TransformInput = inputMap.Player.Transform.triggered;
        InteractInput = inputMap.Player.Interact.triggered;
    }

}
