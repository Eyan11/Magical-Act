using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private UIManager uiScript;
    [SerializeField] private Transform[] levelArr;
    [SerializeField] private Transform playerHolder;
    [SerializeField] private GameObject titleScreenImages;
    [SerializeField] private GameObject titleScreenButtons;
    [SerializeField] private GameObject endScreenImages;
    [SerializeField] private GameObject endScreenButtons;
    private PlayerTransform transformScript;
    private Camera cam;

    [Header ("Sounds")]
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip titleMusic;
    [SerializeField] private AudioClip endMusic;
    [SerializeField] private float musicVol;

    [Header ("Settings")]
    [SerializeField] private float curtainCloseTime;

    public int CurLevel { get; private set; }
    private bool isChangingLevel = false;

    private void Awake() {
        cam = Camera.main;
        transformScript = playerHolder.GetComponent<PlayerTransform>();
        
        // set up title screen
        transformScript.ChangeState('N');
        CurLevel = 0;
        SetUpCurrentLevel();
    }

    public void ChangeLevel(int nextLevel) {
        // ignore if already changing level
        if(isChangingLevel)
            return;
        
        isChangingLevel = true;
        transformScript.ChangeState('N');

        // close curtains
        StartCoroutine(uiScript.CloseCurtainsCoroutine());

        // lower music if changing to a scene with different music
        if(CurLevel <= 0 || nextLevel <= 0 || CurLevel >= levelArr.Length - 1 || nextLevel >= levelArr.Length - 1)
            StartCoroutine(SoundManager.current.LowerMusicVolumeCoroutine(curtainCloseTime));

        // Reset puzzle devices if restarting level
        if(CurLevel == nextLevel)
            Invoke("TriggerRestartEvent", curtainCloseTime);
        
        Invoke("SetUpCurrentLevel", curtainCloseTime);
        CurLevel = nextLevel;
    }

    private void TriggerRestartEvent() {
        RestartEvent.current.TriggerRestartEvent();
    }

    private void SetUpCurrentLevel() {

        // disable title and end screen in case enabled
        titleScreenImages.SetActive(false);
        titleScreenButtons.SetActive(false);
        endScreenImages.SetActive(false);
        endScreenButtons.SetActive(false);

        // move cam to next level
        cam.transform.position = new Vector3(levelArr[CurLevel].position.x, levelArr[CurLevel].position.y, cam.transform.position.z);

        // open curtains
        StartCoroutine(uiScript.OpenCurtainsCoroutine());

        // if setting up title screen
        if(CurLevel <= 0) {
            titleScreenImages.SetActive(true);
            titleScreenButtons.SetActive(true);
            uiScript.HideRestartUI();
            SoundManager.current.PlayMusic(titleMusic, musicVol);
            return;
        }
        // if setting up end screen
        else if(CurLevel >= levelArr.Length - 1) {
            endScreenImages.SetActive(true);
            endScreenButtons.SetActive(true);
            uiScript.HideRestartUI();
            SoundManager.current.PlayMusic(endMusic, musicVol);
            return;
        }
        
        // show restart icon
        uiScript.ShowRestartUI();

        // music (won't restart if already playing)
        SoundManager.current.PlayMusic(levelMusic, musicVol);
    
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

}
