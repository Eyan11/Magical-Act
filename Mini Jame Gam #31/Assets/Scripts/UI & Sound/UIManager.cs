using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // input
    private InputMap inputMap;
    private bool restartInput = false;

    [Header ("References")]
    [SerializeField] private LevelManager levelScript;
    [SerializeField] private GameObject restartUI;
    [SerializeField] private Image curtainImage;
    [SerializeField] private Sprite[] curtainSpriteArr;
    [SerializeField] private float curtainAnimSpeed;
    private int curtainSpriteIndex = 0;

    [Header ("Settings")]
    [SerializeField] private int numLevels;


    private void Awake() {
        // create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.UI.Enable();
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

    public IEnumerator CloseCurtainsCoroutine() {

        curtainSpriteIndex = 0;

        // while not on last sprite
        while(curtainSpriteIndex < curtainSpriteArr.Length) {
            
            curtainImage.sprite = curtainSpriteArr[curtainSpriteIndex];
            curtainSpriteIndex++;

            yield return new WaitForSeconds(curtainAnimSpeed);
        }
        yield break;
    }

    public IEnumerator OpenCurtainsCoroutine() {

        curtainSpriteIndex = curtainSpriteArr.Length - 1;

        // while not on last sprite
        while(curtainSpriteIndex >= 0) {
            
            curtainImage.sprite = curtainSpriteArr[curtainSpriteIndex];
            curtainSpriteIndex--;

            yield return new WaitForSeconds(curtainAnimSpeed);
        }
        yield break;
    }

    // Buttons
    public void Quit() {
        Debug.Log("Quit");
        Application.Quit();
    }

}
