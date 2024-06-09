using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // input
    private InputMap inputMap;
    private bool restartInput = false;

    [Header ("References")]
    [SerializeField] private LevelManager levelScript;
    [SerializeField] private GameObject restartUI;
    [SerializeField] private Animator curtainsAnim;
    private int isOpeningHash;
    private int isClosingHash;

    [Header ("Settings")]
    [SerializeField] private int numLevels;


    private void Awake() {
        // create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.UI.Enable();

        // animations - converts string to int
        isOpeningHash = Animator.StringToHash("isOpening");
        isClosingHash = Animator.StringToHash("isClosing");
    }

    private void Update() {
        // gets hold input (hold for 1 seconds)
        restartInput = inputMap.UI.Restart.triggered;

        // if not on title or start screen, restart level
        if(restartInput && levelScript.CurLevel != 0 && levelScript.CurLevel != (numLevels + 1)) {
            levelScript.ChangeLevel(levelScript.CurLevel);
        }
    }

    public void HideRestartUI() {
        restartUI.SetActive(false);
    }

    public void ShowRestartUI() {
        restartUI.SetActive(true);
    }

    public void OpenCurtains() {
        curtainsAnim.SetBool(isOpeningHash, true);
        curtainsAnim.SetBool(isClosingHash, false);
    }

    public void CloseCurtains() {
        curtainsAnim.SetBool(isOpeningHash, false);
        curtainsAnim.SetBool(isClosingHash, true);
    }


    // Buttons
    public void Quit() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
