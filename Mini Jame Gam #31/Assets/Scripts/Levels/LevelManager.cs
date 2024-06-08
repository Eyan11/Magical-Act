using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform[] levelArr;
    [SerializeField] private Transform playerHolder;
    [SerializeField] private GameObject titleScreenUI;
    [SerializeField] private GameObject endScreenUI;
    private PlayerTransform transformScript;
    private Camera cam;

    [Header ("Settings")]
    [SerializeField] private float curtainCloseTime;

    public int CurLevel { get; private set; }

    private void Awake() {
        cam = Camera.main;
        transformScript = playerHolder.GetComponent<PlayerTransform>();
        
        // set up title screen
        // TODO: start with curtain closed and open it
        transformScript.ChangeState('N');
        CurLevel = 0;
        SetUpCurrentLevel();
    }

    public void ChangeLevel(int nextLevel) {

        // clean up current level
        transformScript.ChangeState('N');
        // TODO: start curtain animation

        CurLevel = nextLevel;
        Invoke("SetUpCurrentLevel", curtainCloseTime);

    }

    private void SetUpCurrentLevel() {

        // disable title and end screen in case enabled
        titleScreenUI.SetActive(false);
        endScreenUI.SetActive(false);

        // if setting up title screen
        if(CurLevel <= 0) {
            titleScreenUI.SetActive(true);
            Debug.Log("Title Screen");
            return;
        }
        // if setting up end screen
        else if(CurLevel >= levelArr.Length - 1) {
            endScreenUI.SetActive(true);
            Debug.Log("End Screen");
            return;
        }
        

        Debug.Log("Next Level");

        // move cam to next level
        cam.transform.position = new Vector3(levelArr[CurLevel].position.x, levelArr[CurLevel].position.y, cam.transform.position.z);
    
        // place magician, rabbit, and hat in next level
        if(levelArr[CurLevel].GetChild(0).tag == "Spawn") {
            playerHolder.GetChild(0).position = levelArr[CurLevel].GetChild(0).position;
            playerHolder.GetChild(1).position = levelArr[CurLevel].GetChild(0).position;
            playerHolder.GetChild(2).position = levelArr[CurLevel].GetChild(0).position;
        }
        else
            Debug.LogError("Level " + CurLevel + "Needs first child to be spawn position and have spawn tag");

        // allow movement
        transformScript.ChangeState('M');
    }
}
