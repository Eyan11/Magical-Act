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
    private PlayerInput inputScript;
    private PlayerTransform transformScript;
    private Camera cam;

    [Header ("Sounds")]
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip titleMusic;
    [SerializeField] private AudioClip endMusic;

    [Header ("Settings")]
    [SerializeField] private float curtainCloseTime;

    public int CurLevel { get; private set; }
    private bool isChangingLevel = false;

    private void Awake() {
        cam = Camera.main;
        transformScript = playerHolder.GetComponent<PlayerTransform>();
        inputScript = playerHolder.GetComponent<PlayerInput>();
        
        // set up title screen
        // TODO: start with curtain closed and open it
        transformScript.ChangeState('N');
        CurLevel = 0;
        SetUpCurrentLevel();
    }

    public void ChangeLevel(int nextLevel) {
        isChangingLevel = true;
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
            SoundManager.current.PlayMusic(titleMusic, 1f);
            return;
        }
        // if setting up end screen
        else if(CurLevel >= levelArr.Length - 1) {
            endScreenUI.SetActive(true);
            SoundManager.current.PlayMusic(endMusic, 1f);
            return;
        }
        

        // music (won't restart if already playing)
        SoundManager.current.PlayMusic(levelMusic, 1f);

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
        isChangingLevel = false;
    }

    private void Update() {
        // restart level if not on title or end screen and not already changing level
        if(inputScript.RestartInput && !isChangingLevel && CurLevel > 0 && CurLevel < levelArr.Length - 1) {
            Debug.Log("LevelManager change state");
            ChangeLevel(CurLevel);
        }
    }
}
